using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using White.Lagoon.Application.Common.Interfaces;
using White.Lagoon.Application.Common.Utility;
using White.Lagoon.Domain.Entities;

namespace White.Lagoon.infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole> roleManager,
                             ApplicationDbContext db )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {

                    _db.Database.Migrate();
                }

                if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();
                   
                    
                      _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "admin@dotnetmastery.com",
                        Email = "admin@dotnetmastery.com",
                        Name = "Ibrahim",
                        NormalizedUserName = "ADMIN@DOTNETMASTERY.COM",
                        NormalizedEmail = "ADMIN@DOTNETMASTERY.COM",
                        PhoneNumber = "1112223333",
                    }, "Admin123*").GetAwaiter().GetResult();

                    ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@dotnetmastery.com");
                    _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

                }
              

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
