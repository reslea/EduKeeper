using System.Data.Entity;
using EduKeeper.Entities;

namespace EduKeeper.EntityFramework
{
    public class EduKeeperContext : DbContext
    {
        static EduKeeperContext()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<EduKeeperContext>());
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

        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Courses)
                .WithMany(c => c.Users)
                .Map(cs =>
                        {
                            cs.MapLeftKey("UserRefId");
                            cs.MapRightKey("CourseRefId");
                            cs.ToTable("User_Courses");
                        });

            modelBuilder.Entity<Course>().HasOptional(u => u.Owner).WithMany(g => g.OwnedGroups);
            modelBuilder.Entity<Post>().HasRequired(p => p.Author).WithMany().WillCascadeOnDelete(false);
        }
    }
}
