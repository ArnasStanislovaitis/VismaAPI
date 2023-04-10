using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using VismaTask.Controllers;
using VismaTask.Services;

namespace TestProject1
{
    
    [TestClass]
    public class GetByIdTests
    {
        private IEmployeeService repo;
        private EmployeeController employeeController;

        [TestInitialize]
        public void TestInitialize()
        {
            repo = A.Fake<IEmployeeService>();
            employeeController = new EmployeeController(repo);
        }

        [TestMethod]
        public async Task GetById_ReturnsOkResult_WhenEmployeeExists()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Dug",
                Birthdate = new DateTime(2000, 1, 1),
                EmploymentDate = new DateTime(2010, 1, 1),
                BossId = 1,
                HomeAddress = "Vilnius",
                CurrentSalary = 1000,
                Role = "accountant"
            };
            A.CallTo(() => repo.GetByID(employee.Id)).Returns(employee);

            // Act
            var result = await employeeController.GetById(employee.Id) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(employee, result.Value);
        }

        [TestMethod]
        public async Task GetById_ReturnsNotFoundResult_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = 1;
            A.CallTo(() => repo.GetByID(employeeId)).Returns(Task.FromResult<Employee>(null));

            // Act
            var result = await employeeController.GetById(employeeId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
                
        }
    }    
}