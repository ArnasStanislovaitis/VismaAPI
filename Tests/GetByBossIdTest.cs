using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using VismaTask.Controllers;
using VismaTask.Services;

namespace TestProject1
{
    public class GetByBossIdTest
    {
        [TestMethod]
        public async Task GetByBossId_ReturnsNotFound_WhenNoEmployeesExist()
        {
            // Arrange
            int bossId = 1;
            var repo = A.Fake<IEmployeeService>();
            A.CallTo(() => repo.GetByBossId(bossId)).Returns(new List<Employee>());

            var employeeController = new EmployeeController(repo);

            // Act
            var result = await employeeController.GetByBossId(bossId) as ActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is NotFoundResult);
        }
    }
}