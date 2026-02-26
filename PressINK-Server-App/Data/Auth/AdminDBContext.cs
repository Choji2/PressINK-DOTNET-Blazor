using PressINK_Server_App.Model.Auth._Models;
using Microsoft.EntityFrameworkCore;

namespace PressINK_Server_App.Data.Auth
{
    public class AdminDBContext: DbContext 
    {
        public AdminDBContext(DbContextOptions<AdminDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var allEntities = modelBuilder.Model.GetEntityTypes();

            foreach (var entity in allEntities)
            {
                try
                {
                    entity.AddProperty("CreatedDate", typeof(DateTime));
                    entity.AddProperty("UpdatedDate", typeof(DateTime));

                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.ToString());
                }

            }
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e =>
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                entityEntry.Property("UpdatedDate").CurrentValue = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property("CreatedDate").CurrentValue = DateTime.Now;
                }

            }

            return base.SaveChanges();
        }

        public DbSet<AdminDB> AdminDB { get; set; }


    }
}
