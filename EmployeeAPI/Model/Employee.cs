using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Employee
{
    public int EmployeeID { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Date of Birth is required.")]
    public DateTime DOB { get; set; }

    public int Age { get; set; }

    [Required(ErrorMessage = "Designation is required.")]
    public string Designation { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    public char Gender { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public bool Status { get; set; }

    [Required(ErrorMessage = "ImageUpload is required.")]
    public string ImageURL { get; set; }

}
