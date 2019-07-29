using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CountryClubMVC.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {

        }
        public DbSet<User> Users { get; set; }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .HasMany(x => x.Followers).WithMany(x => x.Following)
        //        .Map(x => x.ToTable("Followers")
        //            .MapLeftKey("UserId")
        //            .MapRightKey("FollowerId"));
        //}
        public DbSet<Post> Posts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Family> Familys { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public System.Data.Entity.DbSet<CountryClubMVC.Models.Followership> Followerships { get; set; }
    }
}