using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Helpers;
using PersonalWebsite.Interfaces;
using PersonalWebsite.Models;
using PersonalWebsite.Models.DataObjects;
using PersonalWebsite.Services;

namespace PersonalWebsite.Controllers
{
    public class HomeController(JsonFileService jsonFileService, IRepositoryDownloaderService repositoryDownloader) : Controller
    {
        private List<Repositories> Repos = [];

        private readonly string repositoryCookieKey = "repos";
        private readonly string convertedReadmeCookieKey = "readme";

        private readonly List<string> ConvertedReadmeList = [];

        [Route("")]
        public IActionResult Index()
        {
            PersonalInformationModel personalInformationModel = jsonFileService.GetPersonalInformationFromFile();
            StringBuilder stringBuilder = new();
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

        public async Task<IActionResult> Portfolio()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(repositoryCookieKey)))
            {
                var repos = await repositoryDownloader.DownloadAllRepositories();
                HttpContext.Session.SetString(repositoryCookieKey, JsonSerializer.Serialize(repos));
            }
            return View();
        }

        public async Task<ActionResult> GetDynamicContent(int index)
        {
            ViewBag.ConvertedReadme = new List<string>();
            var reposJson = HttpContext.Session.GetString(repositoryCookieKey);
            if (string.IsNullOrEmpty(reposJson))
            {
                return Content("");
            }

            var deserializedRepos = JsonSerializer.Deserialize<List<Repositories>>(reposJson);
            if (deserializedRepos is null)
            {
                return Content("");
            }

            Repos = deserializedRepos;
            if (index < Repos?.Count)
            {
                var convertedReadmeString = ReadmeMdToHtmlConverter.Convert(
                    await repositoryDownloader.DownloadReadme(Repos[index].name),
                    Repos[index].name,
                    repositoryDownloader.DownloadLanguages(Repos[index].languages_url).Result,
                    Repos[index].description,
                    Repos[index].html_url);
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
