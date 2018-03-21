using APT615.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APT615.Models.ApartmentViewModels
{
    public class TrackedApartmentViewModel
    {
        public List<SelectListItem> Amenities { get; set; }
        public IEnumerable<Apartment> TrackedApartments { get; set; }
        public Apartment Apartments { get; set; }
        public HashSet<Amenity> Amenitiz { get; set; }
        public ApartmentAmenity ApartmentAmenity { get; set; }
        public Amenity Amenity { get; set; }

        public TrackedApartmentViewModel(ApplicationDbContext ctx)
        {
         
            this.Amenities = ctx.Amenities
                .OrderBy(t => t.Type)
                .AsEnumerable()
                .Select(li => new SelectListItem
                {
                    Text = li.Type,
                    Value = li.AmenityId.ToString()
                }).ToList();

        }
    }
}
