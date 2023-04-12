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
        public Guid courseId { get; set; }
        public string ownerId { get; set; }
        public string categoryId { get; set; }
        public bool isPremium { get; set; }
        public string instructor { get; set; }
        public double cost { get; set; }
        public string title { get; set; }
        public string image {get; set; }
        public int purchaseNumber { get; set; }
        public int modulesNumber { get; set; }
        public int contentVolume { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.ToUniversalTime();
        public DateTime? UpdatedAt { get; set; } = DateTime.Now.ToUniversalTime();
    }
}
