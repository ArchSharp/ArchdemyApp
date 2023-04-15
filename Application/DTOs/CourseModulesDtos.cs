using Domain.Entities;
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
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; }
        public ICollection<TopicDto> Topics { get; set; }
    }

    public class TopicDto
    {
        //public Guid Id { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

    }
}
