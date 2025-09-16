using FIN.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FIN.Repository
{
    public class FinContext(DbContextOptions<FinContext> options) : DbContext(options)
    {
        public DbSet<User> users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            /// Seeding user data
            User user = new User();
            user.Id = 1;
            user.Firstname = "Alexander";
            user.Lastname = "Agu";
            user.Email = "ahrity67@gmail.com";
            user.Password = "al3x@gu2024#";
            user.Phone = "0784322389";
            user.Enabled = false;
            modelBuilder.Entity<User>().HasData(user);
        }
    }
}
