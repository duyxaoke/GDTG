using Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Data.DAL
{
    public class DatabaseInitializer : DbMigrationsConfiguration<DatabaseContext>
    {
        public DatabaseInitializer()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DatabaseContext context)
        {
            if (!context.Roles.Any(r => r.Name == "Administrator"))
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);

                var role = new ApplicationRole { Name = "Administrator" };
                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "User"))
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);
                var role = new ApplicationRole { Name = "User" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Manager"))
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);
                var role = new ApplicationRole { Name = "Manager" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Anonymous"))
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);
                var role = new ApplicationRole { Name = "Anonymous" };
                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    Name = "Tran Duy",
                    Email = "duytran1402@gmail.com",
                    Balance = 0,
                    EmailConfirmed = true
                };

                manager.Create(user, "123456");
                manager.AddToRole(user.Id, "Administrator");
            }
            if (!context.Users.Any(u => u.UserName == "user"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "user",
                    Name = "user",
                    Email = "user@gmail.com",
                    Balance = 0,
                    EmailConfirmed = true
                };

                manager.Create(user, "123456");
                manager.AddToRole(user.Id, "User");
            }

            //TestDataSeed(context).GetAwaiter().GetResult();

            base.Seed(context);
        }
    }
}