using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Token;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseModule>()
                .HasMany(a => a.Topics);
            //.HasForeignKey(b => b.courseId);
            modelBuilder.Entity<Course>()
                .HasMany(a => a.CourseModules);

            /*modelBuilder.Entity<Topic>()
                .HasMany(b => b.urls)
                .WithOne(c => c.EachModule);*/
                //.HasForeignKey(c => c.courseId);
                
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseModule> CoursesModules { get; set;}
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
    }
}
