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
        public string Name { get; set; } = null!;
        public ICollection<TopicDto> Topics { get; set; } = null!;
    }
    public class UpdateCourseModuleDto
    {
        public Guid CourseModuleId { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<TopicDto> Topics { get; set; } = null!;
    }

    public class GetCourseModuleDto
    {
        public Guid CourseModuleId { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<GetTopicDto> Topics { get; set; } = null!;
    }
    public class GetTopicDto
    {
        public Guid TopicId { get; set; }
        public string Author { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
    /*
    public class GetCourseAllModules
    {
        public ICollection<GetCourseModuleDto> GetCourseModules { get; set; }
    }*/

    public class TopicDto
    {
        public string Author { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;

    }
}
