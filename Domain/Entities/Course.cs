using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CourseId { get; set; }        
        public string AuthorId { get; set; }        
        public string CategoryId { get; set; }
        public string Author { get; set; }
        public bool IsPremium { get; set; }
        public long Cost { get; set; }
        public string Title { get; set; }
        public string Image {get; set; }
        public int PurchaseNumber { get; set; }
        public int ModulesNumber { get; set; }
        public int ContentVolume { get; set; }
        public ICollection<CourseModule> CourseModules { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.ToUniversalTime();
        public DateTime? UpdatedAt { get; set; } = DateTime.Now.ToUniversalTime();
    }
}
