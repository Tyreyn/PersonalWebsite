using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PersonalWebsite.Helpers;
using PersonalWebsite.Models;
using PersonalWebsite.Models.DataObjects;
using PersonalWebsite.Services.FileManagement;
using PersonalWebsite.Services.GithubApi;

namespace PersonalWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private List<Repositories> Repos = new List<Repositories>();

        private readonly string repositoryCookieKey = "repos";

        private readonly JsonFileService jsonFileService;

        private readonly RepositoryDownloaderService repositoryDownloader;

        private readonly string convertedReadmeCookieKey = "readme";

        private readonly List<string> ConvertedReadmeList = new List<string>();

        public HomeController(ILogger<HomeController> logger, JsonFileService jsonFileService, RepositoryDownloaderService repositoryDownloader)
        {
            _logger = logger;
            this.jsonFileService = jsonFileService;
            this.repositoryDownloader = repositoryDownloader;

        }

        [Route("")]
        public IActionResult Index()
        {
            PersonalInformationModel personalInformationModel = this.jsonFileService.GetPersonalInformationFromFile();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (ProgrammingLanguage pl in personalInformationModel.Skills.ProgrammingLanguages)
            {
                StringBuilder ratingStringBuilder = new StringBuilder();
                int tmp = int.Parse(pl.Proficiency);
                for (int i = 0; i < 10; i++)
                {
                    ratingStringBuilder.Append(tmp > 0 ? "<span class=\"bi bi-circle-fill\"></span>\r\n" : "<span class=\"bi bi-circle\"></span>\r\n");
                    tmp--;
                }
                stringBuilder.Append(pl.Name + " Proficiency " + ratingStringBuilder.ToString() + "<br></br>");
            }

            ViewData["ProgrammingLanguage"] = stringBuilder.ToString();
            stringBuilder.Clear();
            foreach (ToolFramework tf in personalInformationModel.Skills.ToolsFrameworks)
            {
                stringBuilder.Append(tf.Name + ", ");
            }
            ViewData["ToolFramework"] = stringBuilder.ToString();
            return View(personalInformationModel);
        }

        public IActionResult Portfolio()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(repositoryCookieKey)))
            {
                List<Repositories> repos = this.repositoryDownloader.DownloadAllRepositories().Result;
                HttpContext.Session.SetString(repositoryCookieKey, JsonSerializer.Serialize(repos));
            }
            return View();
        }

        public ActionResult GetDynamicContent(int index)
        {
            ViewBag.ConvertedReadme = new List<string>();
            Repos = JsonSerializer.Deserialize<List<Repositories>>(HttpContext.Session.GetString(repositoryCookieKey));
            if (index < Repos.Count)
            {
                string convertedReadmeString = ReadmeMdToHtmlConverter.Convert(
                    this.repositoryDownloader.DownloadReadme(Repos[index].name).Result,
                    Repos[index].name,
                    this.repositoryDownloader.DownloadLanguages(Repos[index].languages_url).Result,
                    Repos[index].description);
                ConvertedReadmeList.Add(convertedReadmeString);
                ViewBag.ConvertedReadme = convertedReadmeString;
                Console.WriteLine(index);
                index++;
                return PartialView("PortfolioRepository");
            }
            else
            {
                return Content("");
            }

        }

        public IActionResult LoadReadmePartial()
        {
            ViewBag.ConvertedReadme = HttpContext.Session.GetString(convertedReadmeCookieKey);
            return PartialView("PortfolioRepository");
        }

        [HttpPost]
        public IActionResult SetReadmeCookie(string convertedReadme)
        {
            if (!string.IsNullOrEmpty(convertedReadme))
                HttpContext.Session.SetString(convertedReadmeCookieKey, convertedReadme);
            return Content("");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
