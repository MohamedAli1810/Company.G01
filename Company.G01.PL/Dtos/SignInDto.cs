using System.ComponentModel.DataAnnotations;

namespace Company.G01.PL.Dtos
{
    public class SignInDto
    {
        [Required(ErrorMessage = "Email is Reqired")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Reqired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
