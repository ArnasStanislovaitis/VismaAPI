using System.ComponentModel.DataAnnotations;
using v = VismaTask.Validation;

public class Employee
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [v.FirstNameNotEqualLastName]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    [v.FirstNameNotEqualLastName]
    public string LastName { get; set; }

    [Required]
    [v.DateOfBirth(MinAge = 18, MaxAge = 70)]
    public DateTime Birthdate { get; set; }

    [Required]
    [v.EmploymentDate]
    public DateTime EmploymentDate { get; set; }
    [v.CEOHasNoBoss]
    public int BossId { get; set; }

    [Required]
    public string HomeAddress { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal CurrentSalary { get; set; }

    [Required]
    [v.UniqueCEORole]
    public string Role { get; set; }
}
