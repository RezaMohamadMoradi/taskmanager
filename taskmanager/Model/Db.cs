using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using taskmanager.Model;
using Task = System.Threading.Tasks.Task;

namespace taskmanager.Model
{
    public class Db : IdentityDbContext<User>
    {
        public Db(DbContextOptions<Db> options) : base(options)
        {

        }

        public DbSet<User>Users { get; set; }
        public DbSet<Task>Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
        }
    }


}
