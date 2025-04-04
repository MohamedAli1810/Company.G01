using System.ComponentModel.DataAnnotations;

namespace Company.G01.PL.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "User Name is Reqired")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "First Name is Reqired")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Reqired")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Reqired")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Reqired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is Reqired")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "Confirm Password dose not match the passwoed !!")]
        public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; }
    }
}
