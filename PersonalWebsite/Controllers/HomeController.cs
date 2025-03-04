using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
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

        private readonly JsonFileService jsonFileService;

        private readonly RepositoryDownloaderService repositoryDownloader;

        private readonly List<Repositories> Repos = new List<Repositories>();

        public HomeController(ILogger<HomeController> logger, JsonFileService jsonFileService, RepositoryDownloaderService repositoryDownloader)
        {
            _logger = logger;
            this.jsonFileService = jsonFileService;
            this.repositoryDownloader = repositoryDownloader;
            if (Repos.Count == 0)
            {
                Repos = this.repositoryDownloader.DownloadAllRepositories().Result;
            }
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
            return View();
        }

        public ActionResult GetDynamicContent(int index)
        {
            ViewBag.ConvertedReadme = new List<string>();
            if (index <= Repos.Count)
            {
                ViewBag.ConvertedReadme = ReadmeMdToHtmlConverter.Convert(this.repositoryDownloader.DownloadReadme(Repos[index].name).Result, Repos[index].name);
                index++;
                return PartialView("PortfolioRepository");
            }
            else
            {
                return Content("");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
