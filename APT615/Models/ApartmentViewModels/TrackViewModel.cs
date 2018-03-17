using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APT615.Models.ApartmentViewModels.Track
{
    public class TrackViewModel
    {
        public Apartment Apartment { get; set; }
        public HashSet<Amenity> Amenities { get; set; }

        public TrackViewModel()
        {
            Amenities = new HashSet<Amenity>();
        }
    }
}
