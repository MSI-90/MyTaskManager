using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyTaskManager.Models.DTO.User.RegistrationDTO
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
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password))]
        public string RequirePassword { get; set; } = string.Empty;

        //[HiddenInput]
        //[MaxLength(0)]
        //public string Role { get; set; } = string.Empty;
    }
}
