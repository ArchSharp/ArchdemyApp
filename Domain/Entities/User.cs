using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Utilities;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
        public string ZipCode { get; set; }
        //[FileMaxResolutionAttributes(1)]
        //[FileAllowedExtensionsAttributes(new string[] {".jpg",".png","jpeg"})]
        //public IFormFile? Image { get; set; }
        public bool IsInstructor { get; set; }
        public List<string>? CartCourses { get; set; }
        public List<string>? PurchasedCourses { get; set; }
        public string? TwoFactorSecretKey { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
