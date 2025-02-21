using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalWebsite.Controllers;
using PersonalWebsite.Models;
using PersonalWebsite.Services.FileManagement;

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
            ILogger<HomeController> logger = mock.Object;
            var controller = new HomeController(logger, jsonFileService);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.Count >= 1);
        }
    }
}
