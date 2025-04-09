using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalWebsite.Controllers;
using PersonalWebsite.Interfaces;
using PersonalWebsite.Models;
using PersonalWebsite.Services;

namespace PersonalWebsiteTests
{
    public class HomeControllerTests
    {
        [Fact]
        public void CheckIfViewDataIsFilled()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HomeController>>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockRepositoryDownloader = new RepositoryDownloaderService(mockHttpClientService.Object);

            JsonFileService jsonFileService = new JsonFileService();
            ILogger<HomeController> logger = mockLogger.Object;
            var controller = new HomeController(logger, jsonFileService, mockRepositoryDownloader);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.Count >= 1);
        }
    }
}
