using System.ComponentModel.DataAnnotations;
namespace Belt.Models
{
    public class RegValidator : BaseEntity
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Username cannot be longer than 20 characters.")]
        public string Username {get;set;}

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [DataType(DataType.Password, ErrorMessage = "Invalid password format.")]
        public string Password {get;set;}

        [Required(ErrorMessage = "You must confirm your password.")]
        [DataType(DataType.Password, ErrorMessage = "Invalid password format.")]
        [Compare("Password", ErrorMessage = "Passwords must match.")]
        public string Confirm {get;set;}

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName {get;set;}

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName {get;set;}
    }
}