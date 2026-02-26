
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_scraper_practice_.Model.Printer_DB_Models;
using Web_scraper_practice_.Model.Printer_DB_Models.Sub_models;

namespace DB_SCHEMA.Data.PrinterContexts;
public class PrinterContext : DbContext
{
    public PrinterContext(DbContextOptions<PrinterContext> options):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var allEntities = modelBuilder.Model.GetEntityTypes();

        foreach (var entity in allEntities)
        {
            entity.AddProperty("CreatedDate", typeof(DateTime));
            entity.AddProperty("UpdatedDate", typeof(DateTime));
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

            if(entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property("UpdatedDate").CurrentValue= DateTime.Now;
            }
        }

        try
        {
            return base.SaveChanges();
        }catch (Exception ex)
        {
            foreach (var entityEntry in entries)
            {
                entityEntry.Property("UpdatedDate").CurrentValue = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property("CreatedDate").CurrentValue = DateTime.Now;

                    try{entityEntry.Property("QUE").CurrentValue = $"QU3{DateTime.Now.Year}{DateTime.Now.Microsecond}";
                        return base.SaveChanges();
                    } catch{  }
                    try{entityEntry.Property("MODEL_ID").CurrentValue = $"MD{DateTime.Now.Millisecond}{DateTime.Now.Microsecond}";
                        return base.SaveChanges();
                    }
                    catch { }
                    try{entityEntry.Property("PLANT_ID").CurrentValue = $"P{DateTime.Now.Millisecond}";
                        return base.SaveChanges();
                    } catch { }
                    try{entityEntry.Property("MANUFACTOR_ID").CurrentValue = $"M{DateTime.Now.Millisecond}{DateTime.Now.Microsecond}";
                        return base.SaveChanges();
                    } catch { }
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    try { entityEntry.Property("QUE").CurrentValue = $"QU3{DateTime.Now.Millisecond}{DateTime.Now.Microsecond}";
                        return base.SaveChanges();
                    } catch { }
                    try { entityEntry.Property("MODEL_ID").CurrentValue = $"M{DateTime.Now.Year}{DateTime.Now.Microsecond}";
                        return base.SaveChanges();
                    } catch { }
                    try { entityEntry.Property("PLANT_ID").CurrentValue = $"P{DateTime.Now.Millisecond}";
                        return base.SaveChanges();
                    } catch { }
                    try { entityEntry.Property("MANUFACTOR_ID").CurrentValue = $"M{DateTime.Now.Millisecond}";
                        return base.SaveChanges();
                    } catch { }
                }
            }

            return base.SaveChanges();
        }
    }


    public DbSet<PRINTER_MAIN> PRINTER_MAIN  { get; set; } = null!;
    public DbSet<CATEGORY>  CATEGORIES { get; set; } = null!;
    public DbSet<MANUFACTOR> MANUFACTURE { get; set; } = null!;
    public DbSet<MODEL> MODEL { get; set; } = null!;
    public DbSet<PLANT> PLANT { get; set; } = null!;
    
   
}
