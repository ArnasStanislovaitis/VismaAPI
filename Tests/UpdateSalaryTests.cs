using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using VismaTask.Controllers;
using VismaTask.Services;

namespace TestProject1
{
    [TestClass]
    public class UpdateEmployeeSalaryTests
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
        public async Task UpdateEmployeeSalary_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            decimal newSalary = 2000;
            A.CallTo(() => repo.GetByID(A<int>._)).Returns(Task.FromResult<Employee>(null));

            // Act
            var result = await employeeController.UpdateEmployeeSalary(1, newSalary) as ActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public async Task UpdateEmployeeSalary_ReturnsNoContent_WhenEmployeeSalaryIsUpdated()
        {
            // Arrange
            int employeeId = 1;
            decimal newSalary = 2000;
            var employee = new Employee
            {
                Id = employeeId,
                FirstName = "John",
                LastName = "Doe",
                Birthdate = new DateTime(1990, 1, 1),
                EmploymentDate = new DateTime(2010, 1, 1),
                BossId = 1,
                HomeAddress = "123 Main St.",
                CurrentSalary = 1000,
                Role = "Manager"
            };
            A.CallTo(() => repo.GetByID(employeeId)).Returns(employee);

            // Act
            var result = await employeeController.UpdateEmployeeSalary(employeeId, newSalary) as ActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is NoContentResult);
            Assert.AreEqual(employee.CurrentSalary, newSalary);
            A.CallTo(() => repo.UpdateEmployee(employee)).MustHaveHappenedOnceExactly();
        }
    }
}