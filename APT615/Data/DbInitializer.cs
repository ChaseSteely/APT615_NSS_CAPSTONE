using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APT615.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace APT615.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any Aaprtments.
                if (!context.Apartments.Any())
                {
                    var apartments = new Apartment[]
                        {
                            new Apartment
                            {

                            },
                        };

                }

                    return;
            }
        }
    }
}
