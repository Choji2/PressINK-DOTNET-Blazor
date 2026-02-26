using PressINK_Server_App.Model.API_Form_Models;
using Microsoft.EntityFrameworkCore;
using Web_scraper_practice_.Model.Printer_DB_Models.Sub_models;

namespace PressINK_Server_App.Data.API_Forms
{
    public class API_FormContext: DbContext
    {

        public API_FormContext(DbContextOptions<API_FormContext> options) : base(options)
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


        public DbSet<API_Templates> API_Templates { get; set; }
        public DbSet<API_Template_Attributes> API_Template_Attributes { get; set; }
        public DbSet<Model_Attributes> Model_Attributes { get; set; }
        public DbSet<Attribute_Type> Attribute_Type { get; set; }


    }
}
