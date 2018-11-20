using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class DummyData
    {
        public static async Task Initialize(ApplicationDbContext context,
                              UserManager<ApplicationUser> userManager,
                              RoleManager<ApplicationRole> roleManager)
        {
            context.Database.EnsureCreated();

            string role1 = "Admin";
            string desc1 = "This is the administrator role";

            string role2 = "Child";
            string desc2 = "This is the child role";

            string password = "P@$$w0rd";

            if (await roleManager.FindByNameAsync(role1) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role1, desc1, DateTime.Now));
            }
            if (await roleManager.FindByNameAsync(role2) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role2, desc2, DateTime.Now));
            }
            
            if (await userManager.FindByNameAsync("santa") == null)
            {
                var user = new ApplicationUser
                {
                    // inherited from IdentityUser
                    Email = "santa@np.com",
                    UserName = "santa",

                    // ApplicationUSer attributes
                    FirstName = "Saint",
                    LastName = "Nick",
                    BirthDate = new DateTime(2018, 11, 19),
                    Street = "Yew St",
                    City = "Vancouver",
                    Province = "BC",
                    PostalCode = "V3U E2Y",
                    Country = "Canada",
                    Latitude = 0.0,
                    Longitude = 0.0,
                    IsNaughty = false,
                    DateCreated = DateTime.Now
                    
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }

            if (await userManager.FindByNameAsync("tim") == null)
            {
                var user = new ApplicationUser
                {
                    // inherited from IdentityUser
                    UserName = "tim",
                    Email = "time@np.com",

                    // ApplicationUSer attributes
                    FirstName = "tim",
                    LastName = "twoshoes",
                    BirthDate = new DateTime(1997, 06, 19),
                    Street = "Well St",
                    City = "Vancouver",
                    Province = "BC",
                    PostalCode = "V8U R9Y",
                    Country = "Canada",
                    Latitude = 60.0,
                    Longitude = 50.5,
                    IsNaughty = true,
                    DateCreated = DateTime.Now,
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }
        }
    }
}
