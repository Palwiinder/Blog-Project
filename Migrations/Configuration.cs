namespace BlogAssignment.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using BlogAssignment.Models;
    using BlogAssignment.Models.Domain;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BlogAssignment.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "BlogAssignment.Models.ApplicationDbContext";
        }

        protected override void Seed(BlogAssignment.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var roleManager =
               new RoleManager<IdentityRole>(
                   new RoleStore<IdentityRole>(context));

            //UserManager, used to manage users
            var userManager =
                new UserManager<ApplicationUser>(
                        new UserStore<ApplicationUser>(context));

            //Adding admin role if it doesn't exist.
            if (!context.Roles.Any(p => p.Name == "Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                roleManager.Create(adminRole);
            }

            //Creating the adminuser
            ApplicationUser adminUser;

            if (!context.Users.Any(
                p => p.UserName == "admin@blog.com"))
            {
                adminUser = new ApplicationUser();
                adminUser.UserName = "admin@blog.com";
                adminUser.Email = "admin@blog.com";
                userManager.Create(adminUser, "Password-1");
            }
            else
            {
                adminUser = context
                    .Users
                    .First(p => p.UserName == "admin@blog.com");
            }

            //Make sure the user is on the admin role
            if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }
            if (!context.Roles.Any(p => p.Name == "Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                roleManager.Create(adminRole);
            }

            //Creating the moderateuser
            ApplicationUser moderateUser;

            if (!context.Users.Any(
                p => p.UserName == "moderator@blog.com"))
            {
                moderateUser = new ApplicationUser();
                moderateUser.UserName = "moderator@blog.com";
                moderateUser.Email = "moderator@blog.com";
                userManager.Create(moderateUser, "Password-1");
            }
            else
            {
                moderateUser = context
                    .Users
                    .First(p => p.UserName == "moderator@blog.com");
            }

            //Make sure the user is on the moderate role
            if (!userManager.IsInRole(moderateUser.Id, "Moderate"))
            {
                userManager.AddToRole(moderateUser.Id, "Moderate");
            }
            if (!context.Roles.Any(p => p.Name == "Moderate"))
            {
                var moderateRole = new IdentityRole("Moderate");
                roleManager.Create(moderateRole);
            }
        }
    }

}

