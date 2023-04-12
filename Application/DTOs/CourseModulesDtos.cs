using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CourseModuleDtos 
    {
        [Key]
        public Guid courseId { get; set; }
        public ICollection<EachModuleDto> courseModules { get; set; }
    }

    public class EachModuleDto
    {
        [Key]
        public Guid courseId { get; set; }
        public string courseTitle { get; set; }
        public ICollection<CourseUrlDto> urls { get; set; }

    }

    public class CourseUrlDto
    {
        [Key]
        public Guid courseId { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }
}
