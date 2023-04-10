using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using VismaTask.Controllers;
using VismaTask.Services;

namespace TestProject1
{
    public class GetCountAndAverageTests
    {
        [TestClass]
        public class EmployeeControllerTests
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
            public async Task GetCountAndAvgSalary_ReturnsBadRequest_WhenRoleIsNull()
            {
                // Arrange
                string role = null;

                // Act
                var result = await employeeController.GetCountAndAvgSalary(role) as ActionResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result is BadRequestResult);
            }

            [TestMethod]
            public async Task GetCountAndAvgSalary_ReturnsNotFound_WhenNoEmployeesFound()
            {
                // Arrange
                string role = "accountant";
                A.CallTo(() => repo.GetCountAndAvgSalary(role)).Returns(Task.FromResult((count: 0, averageSalary: 0m)));

                // Act
                var result = await employeeController.GetCountAndAvgSalary(role) as ActionResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result is NotFoundResult);
            }

            [TestMethod]
            public async Task GetCountAndAvgSalary_ReturnsOkResultWithCountAndAvgSalary()
            {
                // Arrange
                string role = "accountant";
                var count = 5;
                var averageSalary = 5000m;
                A.CallTo(() => repo.GetCountAndAvgSalary(role)).Returns((count, averageSalary));

                // Act
                var result = await employeeController.GetCountAndAvgSalary(role) as ActionResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result is OkObjectResult);                
            }
        }
    }
}