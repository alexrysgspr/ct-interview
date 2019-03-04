using Ct.Interview.Web.Api;
using Ct.Interview.Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ct_Interview.Test
{
    public class AsxListedCompaniesControllerTest
    {
        public AsxListedCompaniesControllerTest()
        {

        }
        [Fact]
        public async Task Get_By_AsxCode_Should_Return_OkResult_And_AsxListedCompany()
        {
            // Arrange
            var mockService = new Mock<IAsxListedCompaniesService>();
            var expected = new AsxListedCompany
            {
                AsxCode = "MOQ",
                CompanyName = "MOQ LIMITED",
                GicsIndustryGroup = "Software & Services"
            };
            mockService.Setup(s => s.GetByAsxCode(It.IsAny<string>())).ReturnsAsync(expected);
            var mockCache = new Mock<IMemoryCache>();
            var controller = new AsxListedCompaniesController(mockService.Object);

            // Act
            var result = await controller.Get("MOQ");

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value;
            Assert.IsType<AsxListedCompany>(actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_By_AsxCode_Should_Return_NotFoundResult()
        {
            // Arrange
            var mockService = new Mock<IAsxListedCompaniesService>();
            AsxListedCompany expected = null;
            mockService.Setup(s => s.GetByAsxCode(It.IsAny<string>())).ReturnsAsync(expected);
            var controller = new AsxListedCompaniesController(mockService.Object);

            // Act
            var result = await controller.Get("MOQ");
            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Get_By_AsxCode_Should_Return_OkResult_And_List()
        {
            // Arrange
            var mockService = new Mock<IAsxListedCompaniesService>();
            var expected = new List<AsxListedCompany> {
                new AsxListedCompany
                {
                    AsxCode = "MOQ",
                    CompanyName = "MOQ LIMITED",
                    GicsIndustryGroup = "Software & Services"
                }
            };

            mockService.Setup(s => s.GetAll()).ReturnsAsync(expected);
            var controller = new AsxListedCompaniesController(mockService.Object);

            // Act
            var result = await controller.Get();
            // Assert
            var value = (result.Result as OkObjectResult).Value;
            var obj = Assert.IsAssignableFrom<IEnumerable<AsxListedCompany>>(value);
            Assert.NotEmpty(obj);
        }
    }
}
