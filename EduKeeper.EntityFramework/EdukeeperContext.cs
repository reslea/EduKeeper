using System.Data.Entity;
using EduKeeper.Entities;

namespace EduKeeper.EntityFramework
{
    public class EduKeeperContext : DbContext
    {
        static EduKeeperContext()
        {
            Database.SetInitializer<EduKeeperContext>(new CreateDatabaseIfNotExists<EduKeeperContext>());
        }
        
        public EduKeeperContext()
            : base("EduKeeperDB")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Error> Errors { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany<Course>(u => u.Courses)
                .WithMany(c => c.Users)
                .Map(cs =>
                        {
                            cs.MapLeftKey("UserRefId");
                            cs.MapRightKey("CourseRefId");
                            cs.ToTable("User_Courses");
                        });

            modelBuilder.Entity<User>().HasOptional(u => u.Group).WithMany(g => g.MainCourses);
            modelBuilder.Entity<Course>().HasOptional(u => u.Owner).WithMany(g => g.OwnedGroups);
        }
    }
}
