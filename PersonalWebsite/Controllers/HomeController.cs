using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using PersonalWebsite.Helpers;
using PersonalWebsite.Interfaces;
using PersonalWebsite.Models;
using PersonalWebsite.Models.DataObjects;
using PersonalWebsite.Services;

namespace PersonalWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private List<Repositories> Repos = new List<Repositories>();

        private readonly string repositoryCookieKey = "repos";

        private readonly JsonFileService jsonFileService;

        private readonly IRepositoryDownloaderService repositoryDownloader;

        private readonly string convertedReadmeCookieKey = "readme";

        private readonly List<string> ConvertedReadmeList = new List<string>();

        public HomeController(ILogger<HomeController> logger, JsonFileService jsonFileService, IRepositoryDownloaderService repositoryDownloader)
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
                stringBuilder.Append("∘ " + pl.Name + " proficiency: " + pl.Proficiency + "<br />");
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
