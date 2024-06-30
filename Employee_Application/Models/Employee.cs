using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Application.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
       
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
       
        public int Age { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public char Gender { get; set; }

        //[Range(typeof(bool), "true", "true", ErrorMessage = "You must choose employee status")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "ImageUpload is required.")]
        public string ImageURL { get; set; } = "No Image";

        [ValidateNever]
        [NotMapped]
        public IFormFile Image64 { get; set; }
      
    }

    public class Designation
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
