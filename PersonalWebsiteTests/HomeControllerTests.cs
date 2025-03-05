using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalWebsite.Controllers;
using PersonalWebsite.Models;
using PersonalWebsite.Services.FileManagement;
using PersonalWebsite.Services.GithubApi;

namespace PersonalWebsiteTests
{
    public class HomeControllerTests
    {
        [Fact]
        public void CheckIfViewDataIsFilled()
        {
            // Arrange
            var mock = new Mock<ILogger<HomeController>>();
            JsonFileService jsonFileService = new JsonFileService();
            RepositoryDownloaderService repositoryDownloader = new RepositoryDownloaderService();
            ILogger<HomeController> logger = mock.Object;
            var controller = new HomeController(logger, jsonFileService, repositoryDownloader);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.Count >= 1);
        }
    }
}
