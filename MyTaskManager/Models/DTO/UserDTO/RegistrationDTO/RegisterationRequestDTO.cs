using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyTaskManager.Models.DTO.UserDTO.RegistrationDTO
{
    public class RegisterationRequestDTO
    {
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Минимальная длина пароля может составлять 8 символов")]
        [MaxLength(20, ErrorMessage = "Максимальная длина пароля может составлять 20 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Пароли должны совпадать")]
        [DataType(DataType.Password)]
        public string RequirePassword { get; set; } = string.Empty;
    }
}
