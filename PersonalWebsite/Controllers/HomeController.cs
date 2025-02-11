using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Models;
using PersonalWebsite.Models.DataObjects;
using PersonalWebsite.Services.FileManagement;

namespace PersonalWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly JsonFileService jsonFileService;

        public HomeController(ILogger<HomeController> logger, JsonFileService jsonFileService)
        {
            _logger = logger;
            this.jsonFileService = jsonFileService;
        }

        [Route("")]
        public IActionResult Index()
        {
            PersonalInformationModel personalInformationModel = this.jsonFileService.GetPersonalInformationFromFile();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (ProgrammingLanguage pl in personalInformationModel.Skills.ProgrammingLanguages)
            {
                stringBuilder.Append(pl.Name + " Proficiency" + pl.Proficiency + ", ");
            }
            ViewData["ProgrammingLanguage"] = stringBuilder.ToString();
            stringBuilder.Clear();
            foreach (ToolFramework tf in personalInformationModel.Skills.ToolsFrameworks)
            {
                stringBuilder.Append(tf.Name +  ", ");
            }
            ViewData["ToolFramework"] = stringBuilder.ToString();

            return View(personalInformationModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
