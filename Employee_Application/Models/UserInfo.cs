using System.ComponentModel.DataAnnotations;

namespace Employee_Application.Models
{
    public class UserInfo
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        
        public string Password { get; set; }
    }
}
