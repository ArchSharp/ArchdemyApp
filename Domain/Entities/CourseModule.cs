using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CourseModule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CourseModuleId { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Topic> Topics { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }

    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TopicId { get; set; }
        public string Name { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Url { get; set; } = null!;

        //Navigation property
        public CourseModule CourseModule { get; set; } = null!;
        public Guid CourseModuleId { get; set; }
    }

}
