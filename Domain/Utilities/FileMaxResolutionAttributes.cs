using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utilities
{
    public class FileMaxResolutionAttributes : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public FileMaxResolutionAttributes(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > (1000000 * _maxFileSize))
                {
                    return new ValidationResult($"Maximum allowed Image size is {_maxFileSize} MB.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
