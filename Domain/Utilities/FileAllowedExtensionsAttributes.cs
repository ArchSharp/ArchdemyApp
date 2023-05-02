using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utilities
{
    public class FileAllowedExtensionsAttributes : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;
        public FileAllowedExtensionsAttributes(string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_allowedExtensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult($"This Image extension is not allowed!");
                }
            }
            return ValidationResult.Success;
        }
    }
}
