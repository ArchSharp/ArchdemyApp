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
        public Guid courseId { get; set; }
        public ICollection<EachModule> courseModules { get; set;}
    }

    public class EachModule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public string courseTitle { get; set; }
        public ICollection<CourseUrl> urls { get; set; }

        //Navigation property
        public CourseModule CourseModule { get; set; }
        [ForeignKey("CourseModule")]
        public Guid courseId { get; set; }
    }

    public class CourseUrl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public string title { get; set; }
        public string url { get; set; }

        //Foreign key
        public EachModule EachModule { get; set; }
        [ForeignKey("CourseModule")]
        public Guid courseId { get; set; }
    }

}
