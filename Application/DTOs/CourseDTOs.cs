using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateCourseDto
    {
        public string ownerId { get; set; }
        public string categoryId { get; set; }
        public bool isPremium { get; set; }
        public string instructor { get; set; }
        public double cost { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public int purchaseNumber { get; set; }
        public int modulesNumber { get; set; }
        public int contentVolume { get; set; }
    }

    public class GetCourseDto : CreateCourseDto
    {
        //public string courseId { get; set; }
    }

    public class CategoryCoursesDto : CreateCourseDto
    {
    
    }
}
