using Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateCourseDto
    {
        public Guid CategoryId { get; set; }
        public Guid AuthorId { get; set; }
        public string Author { get; set; } = null!;
        public bool IsPremium { get; set; }
        public double Cost { get; set; }
        public string Title { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int PurchaseNumber { get; set; }
        public int ModulesNumber { get; set; }
        public int ContentVolume { get; set; }
        public bool IsGoLive { get; set; }
    }
        
    public class GetCourseDto
    {
        public Guid CourseId { get; set; }
        public Guid AuthorId { get; set; }
        public Guid CategoryId { get; set; }
        public string Author { get; set; } = null!;
        public bool IsPremium { get; set; }
        public long Cost { get; set; }
        public string Title { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int PurchaseNumber { get; set; }
        public int ModulesNumber { get; set; }
        public int ContentVolume { get; set; }
        public bool IsGoLive { get; set; }
        public DateTime CreatedAt { get; set; }
    }       

    public class CategoryCoursesDto : CreateCourseDto
    {
    
    }

    public class UpdateCourseDto
    {
        public Guid CourseId { get; set; }
        public string? Author { get; set; }
        public Guid? AuthorId { get; set; }
        public string? CategoryId { get; set; }
        public bool IsPremium { get; set; }
        public long Cost { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public int? PurchaseNumber { get; set; }
        public int? ModulesNumber { get; set; }
        public int? ContentVolume { get; set; }
        public bool IsGoLive { get; set; }
    }

    public class CoursePaymentPayloadDto
    {
        public string Name { get; set; } = null!;
        public Guid CourseId { get; set; }
        public long Price { get; set; }
    }
}
