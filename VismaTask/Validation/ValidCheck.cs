using System.ComponentModel.DataAnnotations;
using VismaTask.Data;

namespace VismaTask.Validation
{
    public class DateOfBirthAttribute : ValidationAttribute
    {
        public int MinAge { get; set; }
        public int MaxAge { get; set; }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            var val = (DateTime)value;

            if (val.AddYears(MinAge) > DateTime.Now)
                return false;

            return (val.AddYears(MaxAge) > DateTime.Now);
        }
    }

    public class FirstNameNotEqualLastNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var employee = (Employee)validationContext.ObjectInstance;

            if (employee.FirstName == employee.LastName)
            {
                return new ValidationResult("First name cannot be equal to last name.");
            }

            return ValidationResult.Success;
        }
    }

    public class EmploymentDateAttribute : ValidationAttribute
    {
        private DateTime minDate = new DateTime(2000, 1, 1);

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            var val = (DateTime)value;

            if (val > DateTime.Now)
                return false;

            return val >= minDate;
        }
    }
    public class UniqueCEORoleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (EmployeeDbContext)validationContext.GetService(typeof(EmployeeDbContext))!;
            var employee = (Employee)validationContext.ObjectInstance;

            if (employee.Role.Trim().ToUpper() == "CEO" && dbContext.Employees.Any(e => e.Role == "CEO" && e.Id != employee.Id))
            {
                return new ValidationResult("There can be only 1 employee with CEO role");
            }

            return ValidationResult.Success;
        }
    }

    public class CEOHasNoBossAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
                var employee = (Employee)validationContext.ObjectInstance;

                if (employee.Role.Trim().ToUpper() != "CEO" && employee.BossId == 0)
                {
                    return new ValidationResult("Only the CEO role has no boss");
                }

                return ValidationResult.Success;
        }
    }
}