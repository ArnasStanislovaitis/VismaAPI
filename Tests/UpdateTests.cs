using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using VismaTask.Controllers;
using VismaTask.Services;

namespace TestProject1
{
    [TestClass]
    public class UpdateEmployeeTests
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
        public async Task UpdateEmployee_ReturnsBadRequest_WhenEmployeeIsNull()
        {
            // Arrange
            int employeeId = 1;
            Employee employee = null;

            // Act
            var result = await employeeController.UpdateEmployee(employeeId, employee) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task UpdateEmployee_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            int employeeId = 1;
            Employee employee = new Employee
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

            A.CallTo(() => repo.GetByID(employeeId)).Returns(Task.FromResult<Employee>(null));

            // Act
            var result = await employeeController.UpdateEmployee(employeeId, employee) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task UpdateEmployee_ReturnsNoContent_WhenEmployeeIsUpdated()
        {
            // Arrange
            var employeeId = 1;
            var employee = new Employee
            {
                Id = employeeId,
                FirstName = "John",
                LastName = "Doe",
                Birthdate = new DateTime(2000, 1, 1),
                EmploymentDate = new DateTime(2010, 1, 1),
                BossId = 1,
                HomeAddress = "Vilnius",
                CurrentSalary = 1000,
                Role = "accountant"
            };
            var updatedEmployee = new Employee
            {
                Id = employeeId,
                FirstName = "Jane",
                LastName = "Doe",
                Birthdate = new DateTime(2000, 1, 1),
                EmploymentDate = new DateTime(2010, 1, 1),
                BossId = 1,
                HomeAddress = "Kaunas",
                CurrentSalary = 2000,
                Role = "manager"
            };
            A.CallTo(() => repo.GetByID(employeeId)).Returns(employee);
            A.CallTo(() => repo.UpdateEmployee(employee)).Returns(employee);

            // Act
            var result = await employeeController.UpdateEmployee(employeeId, updatedEmployee) as ActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is NoContentResult);
        }
    }
}