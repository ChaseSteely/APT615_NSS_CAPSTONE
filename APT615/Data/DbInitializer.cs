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
                if (!context.Amenities.Any())
                {
                    var amenity = new Amenity[]
                        {
                            new Amenity
                            {
                                Type = "Washer/Dryer"
                            },
                            new Amenity
                            {
                                Type = "Pool"
                            },
                            new Amenity
                            {
                                Type = "Fitness Center"
                            },
                            new Amenity
                            {
                                Type = "Covered Parking"
                            },
                            new Amenity
                            {
                                Type = "High-Speed Internet"
                            },
                            new Amenity
                            {
                                Type = "WiFi"
                            },
                            new Amenity
                            {
                                Type = "Utilities Included"
                            },
                            new Amenity
                            {
                                Type = "Water Included"
                            },
                            new Amenity
                            {
                                Type = "Balcony"
                            },
                            new Amenity
                            {
                                Type = "Walk-In Closet"
                            },
                            new Amenity
                            {
                                Type = "Dishwasher"
                            },
                             new Amenity
                            {
                                Type = "Package Service"
                            },
                              new Amenity
                            {
                                Type = "Rooftop Lounge"
                            },
                               new Amenity
                            {
                                Type = "Hardwood Floors"
                            },
                                  new Amenity
                            {
                                Type = "Grilling Area"
                            }
                        };
                    foreach (Amenity a in amenity)
                    {
                        context.Amenities.Add(a);
                    }
                    context.SaveChanges();
                }

                    return;
            }
        }
    }
}
