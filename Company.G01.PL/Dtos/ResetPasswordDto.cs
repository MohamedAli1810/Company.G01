using System.ComponentModel.DataAnnotations;

namespace Company.G01.PL.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Password is Reqired")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is Reqired")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password dose not match the passwoed !!")]
        public string ConfirmPassword { get; set; }
    }
}
