using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.DAL.Entities.Identity;

namespace Talabat.DAL.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed Elfayoumi",
                    UserName = "ahmedelfayoumi97",
                    Email = "ahmedelfayoumi203@gmail.com",
                    PhoneNumber = "01093422098",
                    Address = new Address()
                    {
                        FristName = "Ahmed",
                        LastName = "Elfayoumi",
                        Country = "Egypt",
                        City = "El-bagour",
                        Street = "10 Tahrir St."
                    }
                };
                await userManager.CreateAsync(user, "Pa$$w0rd"); //P@ssword
            }
        }
    }
}
