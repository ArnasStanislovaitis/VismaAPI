using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using VismaTask.Controllers;
using VismaTask.Services;

namespace TestProject1
{
    [TestClass]
    public class DeleteTests
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
        public async Task Delete_ReturnsNotFoundResult_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = 1;
            A.CallTo(() => repo.GetByID(employeeId)).Returns(Task.FromResult<Employee>(null));

            // Act
            var result = await employeeController.Delete(employeeId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            A.CallTo(() => repo.GetByID(employeeId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repo.DeleteEmployee(employeeId)).MustNotHaveHappened();
        }

        [TestMethod]
        public async Task Delete_ReturnsNoContentResult_WhenEmployeeIsDeleted()
        {
            // Arrange
            var employeeId = 1;
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
            A.CallTo(() => repo.GetByID(employeeId)).Returns(employee);
            A.CallTo(() => repo.DeleteEmployee(employeeId)).Returns(true);

            //Act
            var result = await employeeController.Delete(employeeId) as ActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is NoContentResult);            
        }
        
        [TestMethod]
        public async Task Delete_ReturnsBadRequestResult_WhenEmployeeExistsButDeletionFails()
        {
            // Arrange
            var employeeId = 1;
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

            // Act
            var result = await employeeController.Delete(employeeId) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual($"Customer {employeeId} was found but failed to delete.", result.Value);
            A.CallTo(() => repo.GetByID(employeeId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repo.DeleteEmployee(employeeId)).MustHaveHappenedOnceExactly();
        }
    }
}