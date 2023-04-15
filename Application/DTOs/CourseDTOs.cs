using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateCourseDto
    {
        public string Author { get; set; }
        public string AuthorId { get; set; }
        public string CategoryId { get; set; }
        public bool IsPremium { get; set; }
        public double Cost { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int PurchaseNumber { get; set; }
        public int ModulesNumber { get; set; }
        public int ContentVolume { get; set; }
    }

    public class GetCourseDto : CreateCourseDto
    {
        //public string courseId { get; set; }
    }

    public class CategoryCoursesDto : CreateCourseDto
    {
    
    }

    public class UpdateCourseDto : CreateCourseDto
    {
        public Guid CourseId { get; set; }
    }
}
