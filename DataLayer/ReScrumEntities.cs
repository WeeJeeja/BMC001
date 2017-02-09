using DataLayer.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace DataLayer
{
    public class ReScrumEntities : DbContext
    {
        public ReScrumEntities()
            : base("BMC001")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ReScrumEntities>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Booking> Booking { get; set; }

    }
}
