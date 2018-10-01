using Core.Data;
using Data.Mapping;
using Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Text;

namespace Data.DAL
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext() : base("name=DbConnect")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, DatabaseInitializer>());//initial database use test data
        }

        public static DatabaseContext Create()
        {
            return new DatabaseContext();
        }

        //DbContext
        public DbSet<Config> Config { get; set; }

        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuInRoles> MenuInRoles { get; set; }
        public DbSet<DB_LOG> Logs { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }

        //Business

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException exception)
            {
                var sb = new StringBuilder();

                sb.AppendLine(exception.Message);

                foreach (var validationError in exception.EntityValidationErrors)
                {
                    sb.AppendLine(
                        String.Format("Entity \"{0}\" in state \"{1}\", errors:",
                                      validationError.Entry.Entity.GetType().Name,
                                      validationError.Entry.State));

                    foreach (var error in validationError.ValidationErrors)
                    {
                        sb.AppendLine(
                            String.Format("\t(Property: \"{0}\", Error: \"{1}\")",
                                          error.PropertyName, error.ErrorMessage));
                    }
                }

                throw new DbEntityValidationException(sb.ToString(), exception);
            }
        }

        //Auth

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new ConfigMap());
            modelBuilder.Configurations.Add(new MenuMap());
            modelBuilder.Configurations.Add(new MenuInRolesMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}