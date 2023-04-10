using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VismaTask.Services;


namespace VismaTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService repo;
        public EmployeeController(IEmployeeService repo)
        {
            this.repo = repo;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Employee))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            
            if (employee == null)
            {
                return BadRequest();
            }
            try
            {
                Employee? addedEmployee = await repo.AddEmployee(employee);
                if (addedEmployee == null)
                {
                    return BadRequest("Repository failed to create customer");
                }
                else
                {
                    return Ok();
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAllEmployess()
        {
            try
            {
                var employees = await repo.GetAll();
                return Ok(employees);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                Employee? employee = await repo.GetByID(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Employee? existing = await repo.GetByID(id);

                if (existing == null)
                {
                    return NotFound();
                }
                bool? deleted = await repo.DeleteEmployee(id);

                if (deleted.HasValue && deleted.Value)
                {
                    return new NoContentResult();
                }
                else
                {
                    return BadRequest($"Customer {id} was found but failed to delete.");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }
                Employee? existing = await repo.GetByID(id);

                if (existing == null)
                {
                    return NotFound();
                }
                existing.EmploymentDate = employee.EmploymentDate;
                existing.LastName = employee.LastName;
                existing.FirstName = employee.FirstName;
                existing.Birthdate = employee.Birthdate;
                existing.BossId = employee.BossId;
                existing.Role = employee.Role;
                existing.CurrentSalary = employee.CurrentSalary;
                existing.HomeAddress = employee.HomeAddress;

                await repo.UpdateEmployee(existing);

                return new NoContentResult();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("update/salary/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateEmployeeSalary(int id, [FromBody] decimal newSalary)
        {
            try
            {
                if (newSalary == null)
                {
                    return BadRequest();
                }
                Employee? existing = await repo.GetByID(id);

                if (existing == null)
                {
                    return NotFound();
                }
                existing.CurrentSalary = newSalary;

                await repo.UpdateEmployee(existing);

                return new NoContentResult();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("boss/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByBossId(int id)
        {
            try
            {
                var foundEmployees = await repo.GetByBossId(id);

                if (!foundEmployees.Any())
                {
                    return NotFound();
                }

                return Ok(foundEmployees);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("role/{role}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCountAndAvgSalary(string role)
        {
            if (role.IsNullOrEmpty())
            {
                return BadRequest();
            }
            try
            {
                var (count, averageSalary) = await repo.GetCountAndAvgSalary(role);

                if (count == 0)
                {
                    return NotFound();
                }

                var result = new { count, averageSalary };

                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("namedate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByNameAndDate(string name, string startDateStr, string endDateStr)
        {
            try
            {
                if (name.IsNullOrEmpty())
                {
                    return BadRequest();
                }

                if (!DateTime.TryParse(startDateStr, out DateTime startDate))
                {
                    return BadRequest("Invalid start date format. Please use yyyy-mm-dd.");
                }
                if (!DateTime.TryParse(endDateStr, out DateTime endDate))
                {
                    return BadRequest("Invalid end date format. Please use yyyy-mm-dd.");
                }
                var employees = await repo.GetByNameAndDate(name, startDate, endDate);

                if (employees == null)
                {
                    return NotFound();
                }

                return Ok(employees);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}