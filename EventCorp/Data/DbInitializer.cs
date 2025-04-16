using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCorpModels.Data
{
        public static class DbInitializer
        {
            public static async Task Initialize(CE2DbContext context,
                                             UserManager<User> userManager,
                                             RoleManager<IdentityRole> roleManager)
            {
                context.Database.EnsureCreated();

            string[] roleNames = { "ADMINISTRADOR", "ORGANIZADOR", "USUARIO" };

            foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                var adminEmail = "admin@inventory.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    var admin = new User
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        NombreCompleto = "System Administrator",
                        EmailConfirmed = true
                    };

                    string adminPassword = "12341234";
                    var createAdmin = await userManager.CreateAsync(admin, adminPassword);

                    if (createAdmin.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "ADMIN");
                    }
                }

                var organizerEmail = "organizador@inventory.com";
                var organizerUser = await userManager.FindByEmailAsync(organizerEmail);

                if (organizerUser == null)
                {
                    var organizer = new User
                    {
                        UserName = organizerEmail,
                        Email = organizerEmail,
                        NombreCompleto = "Organizer User",
                        EmailConfirmed = true
                    };

                    string organizerPassword = "12341234";
                    var createOrganizer = await userManager.CreateAsync(organizer, organizerPassword);

                    if (createOrganizer.Succeeded)
                    {
                        await userManager.AddToRoleAsync(organizer, "ORGANIZADOR");
                    }
                }

                var userEmail = "user@inventory.com";
                var regularUser = await userManager.FindByEmailAsync(userEmail);

                if (regularUser == null)
                {
                    var user = new User
                    {
                        UserName = userEmail,
                        Email = userEmail,
                        NombreCompleto = "Regular User",
                        EmailConfirmed = true
                    };

                    string userPassword = "12341234";
                    var createUser = await userManager.CreateAsync(user, userPassword);

                    if (createUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "USER");
                    }
                }
            }
        }
    }
