using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateCourseModuleDtos 
    {
        //public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; }
        public ICollection<TopicDto> Topics { get; set; }
    }
    public class UpdateCourseModuleDto
    {
        public Guid CourseModuleId { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; }
        public ICollection<TopicDto> Topics { get; set; }
    }

    public class GetCourseModuleDto
    {
        public Guid CourseModuleId { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; }
        public ICollection<TopicDto> Topics { get; set; }
    }
    /*
    public class GetCourseAllModules
    {
        public ICollection<GetCourseModuleDto> GetCourseModules { get; set; }
    }*/

    public class TopicDto
    {
        //public Guid Id { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

    }
}
