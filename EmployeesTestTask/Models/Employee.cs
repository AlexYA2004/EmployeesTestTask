using System.ComponentModel.DataAnnotations;

namespace EmployeesTestTask.Models;


    
public class Employee
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Имя обязательно для заполнения")]
    [StringLength(50, ErrorMessage = "Имя не может превышать 50 символов")]
    [RegularExpression(@"^[а-яА-ЯёЁ\s\-]+$", ErrorMessage = "Имя может содержать только буквы, пробелы и тире")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Фамилия обязательна для заполнения")]
    [StringLength(50, ErrorMessage = "Фамилия не может превышать 50 символов")]
    [RegularExpression(@"^[а-яА-ЯёЁ\s\-]+$", ErrorMessage = "Фамилия может содержать только буквы, пробелы и тире")]
    public string LastName { get; set; }

    [StringLength(50, ErrorMessage = "Отчество не может превышать 50 символов")]
    [RegularExpression(@"^[а-яА-ЯёЁ\s\-]*$", ErrorMessage = "Отчество может содержать только буквы, пробелы и тире")]
    public string MiddleName { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; }

    public List<EmploymentPeriod> EmploymentPeriods { get; set; } = new List<EmploymentPeriod>();

    public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    
    public DateTime? CurrentEmploymentStartDate => 
        EmploymentPeriods?.OrderByDescending(ep => ep.StartDate)
                         .FirstOrDefault()?.StartDate;
                         
    public DateTime? CurrentEmploymentEndDate => 
        EmploymentPeriods?.OrderByDescending(ep => ep.StartDate)
                         .FirstOrDefault()?.EndDate;
                         
    public bool IsCurrentlyEmployed => 
        EmploymentPeriods?.Any(ep => ep.IsCurrent) == true;
}

public class EmploymentPeriod
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Дата начала трудоустройства обязательна")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    public string Position { get; set; }

    public DateTime CreatedDate { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public bool IsCurrent => EndDate == null || EndDate > DateTime.Today;
}



