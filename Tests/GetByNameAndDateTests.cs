using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using VismaTask.Controllers;
using VismaTask.Services;

namespace TestProject1
{
    
    [TestClass]
    public class GetByNameAndDateTests
    {
        private IEmployeeService repo;
        private EmployeeController employeeController;

        [TestInitialize]
        public void Initialize()
        {
            repo = A.Fake<IEmployeeService>();
            employeeController = new EmployeeController(repo);
        }

        [TestMethod]
        public async Task GetByNameAndDate_ReturnsBadRequest_WhenNameIsNull()
        {
            // Arrange
            string name = null;
            string startDateStr = "2022-01-01";
            string endDateStr = "2022-01-31";

            // Act
            var result = await employeeController.GetByNameAndDate(name, startDateStr, endDateStr);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task GetByNameAndDate_ReturnsBadRequest_WhenStartDateIsInvalidFormat()
        {
            // Arrange
            string name = "John";
            string startDateStr = "2022-13-01"; // Invalid month
            string endDateStr = "2022-01-31";

            // Act
            var result = await employeeController.GetByNameAndDate(name, startDateStr, endDateStr);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual("Invalid start date format. Please use yyyy-mm-dd.", (result as BadRequestObjectResult)?.Value);
        }

        [TestMethod]
        public async Task GetByNameAndDate_ReturnsBadRequest_WhenEndDateIsInvalidFormat()
        {
            // Arrange
            string name = "John";
            string startDateStr = "2022-01-01";
            string endDateStr = "2022-31-01"; // Invalid day

            // Act
            var result = await employeeController.GetByNameAndDate(name, startDateStr, endDateStr);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual("Invalid end date format. Please use yyyy-mm-dd.", (result as BadRequestObjectResult)?.Value);
        }        
        [TestMethod]
        public async Task GetByNameAndDate_ReturnsOkResult_WhenEmployeesExist()
        {
            // Arrange
            string name = "John";
            string startDateStr = "2022-01-01";
            string endDateStr = "2022-01-31";
            var employees = new List<Employee> { new Employee { Id = 1, FirstName = "John", LastName = "Doe" } };
            A.CallTo(() => repo.GetByNameAndDate(name, A<DateTime>.Ignored, A<DateTime>.Ignored)).Returns(employees);

            // Act
            var result = await employeeController.GetByNameAndDate(name, startDateStr, endDateStr);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(employees, okResult.Value);
        }
    }    
}