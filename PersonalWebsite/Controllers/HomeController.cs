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
            StringBuilder stringBuilder = new StringBuilder("<h5>");
            foreach (ProgrammingLanguage pl in personalInformationModel.Skills.ProgrammingLanguages)
            {
                StringBuilder ratingStringBuilder = new StringBuilder();
                int tmp = int.Parse(pl.Proficiency);
                for(int i = 0; i < 10; i++)
                {
                    ratingStringBuilder.Append(tmp > 0 ? "<span class=\"bi bi-circle-fill\"></span>\r\n" : "<span class=\"bi bi-circle\"></span>\r\n");
                    tmp--;
                }
                stringBuilder.Append(pl.Name + " Proficiency " + ratingStringBuilder.ToString() + "<br></br>");
            }
            stringBuilder.Append("</h5>");
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
