using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APT615.Models.ViewModels
{
    public class ApartmentIndexData
    {
        public IEnumerable<Apartment> Apartments { get; set; }
        public IEnumerable<Amenity> Amenities { get; set; }
    }
}
