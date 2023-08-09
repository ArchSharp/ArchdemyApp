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
        public ICollection<CourseModule> CourseModules { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.ToUniversalTime();
        public DateTime? UpdatedAt { get; set; } = DateTime.Now.ToUniversalTime();
    }
}
