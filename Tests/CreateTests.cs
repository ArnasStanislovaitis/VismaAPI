using FakeItEasy;
using VismaTask.Services;
using VismaTask.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestProject1
{
    [TestClass]
    public class CreateTests
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
        public async Task AddEmployee_ReturnsOkResult()
        {
            // Arrange
            var employee = new Employee {
                Id = 1,
                FirstName = "John",
                LastName = "Dug",
                Birthdate = new DateTime(2000, 1, 1),
                EmploymentDate = new DateTime(2010, 1, 1),
                BossId = 1,
                HomeAddress = "Vilnius",
                CurrentSalary = 1000,
                Role="accountant" 
            };
            A.CallTo(() => repo.AddEmployee(A<Employee>.Ignored)).Returns(employee);

            // Act
            var result = await employeeController.AddEmployee(employee) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task AddEmployee_ReturnsBadRequest_WhenEmployeeIsNull()
        {
            // Arrange

            // Act
            var result = await employeeController.AddEmployee(null) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);        
        }    
    }
}