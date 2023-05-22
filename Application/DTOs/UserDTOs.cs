using Domain.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateUserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        //[FileMaxResolutionAttributes(1)]
        //[FileAllowedExtensionsAttributes(new string[] { ".jpg", ".png", "jpeg" })]
        //public IFormFile Image { get; set; }
    }

    public class UpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public List<string>? CartCourses { get; set; }
        public List<string>? PurchasedCourses { get; set; }
    }

    public class LoginUserDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
    public class ChangePasswordDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string CurrentPassword { get; set; } = null!;
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters!")]
        public string NewPassword { get; set; } = null!;
    }

    public class ResetPasswordDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters!")]
        public string Password { get; set; } = null!;
        [Required]
        public string Token { get; set; } = null!;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;
    }

    public class ForgotPasswordDto
    {
        [Required]
        public string Email { get; set; } = null!;
    }

    public class TokenDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }

    public class RefreshTokenDto
    {
        public string Token { get; set; } = null!;
    }
}
