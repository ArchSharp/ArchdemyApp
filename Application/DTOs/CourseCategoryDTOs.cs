using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class GetCategoryDTO
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<GetCourseDto> Courses { get; set; } = null!;
    }

    public class GetAllCategoryDTO
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
