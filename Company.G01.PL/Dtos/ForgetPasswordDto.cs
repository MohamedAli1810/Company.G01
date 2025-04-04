using System.ComponentModel.DataAnnotations;

namespace Company.G01.PL.Dtos
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "Email is Reqired")]
        [EmailAddress]
        public string Email { get; set; }

    }
}
