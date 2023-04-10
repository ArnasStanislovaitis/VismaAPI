using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaTask.Controllers;
using VismaTask.Services;

namespace TestProject1
{
    [TestClass]
    public class GetAllTest
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
        public async Task GetAllEmployees_ReturnsOkResult()
        {
            // Arrange
            var employees = new List<Employee> { new Employee{
                Id = 1,
                FirstName = "John",
                LastName = "Dug",
                Birthdate = new DateTime(2000, 1, 1),
                EmploymentDate = new DateTime(2010, 1, 1),
                BossId = 1,
                HomeAddress = "Vilnius",
                CurrentSalary = 1000,
                Role="accountant"
            }
        };
            A.CallTo(() => repo.GetAll()).Returns(employees);

            // Act
            var result = await employeeController.GetAllEmployess() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(employees, result.Value);
        }        
    }

}
