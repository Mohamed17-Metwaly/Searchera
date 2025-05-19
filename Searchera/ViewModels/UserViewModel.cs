using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Searchera.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name id Required!")]
        [MaxLength(70, ErrorMessage = "The Max Length of Name is 70!")]
        [MinLength(3, ErrorMessage = "The Min Length of Name is 3!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Email is Required!")]
        [EmailAddress(ErrorMessage = "wrong in Email!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The Password is Required!")]
        [DataType(DataType.Password, ErrorMessage = "it is wrong password!")]
        public string Password { get; set; }
        public string ProfileImage { get; set; }
        [NotMapped]
        public IFormFile ProfileImageFile { get; set; }
        public string Role { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Choose your role!")]
        public int RoleID { get; set; }

    }
}
