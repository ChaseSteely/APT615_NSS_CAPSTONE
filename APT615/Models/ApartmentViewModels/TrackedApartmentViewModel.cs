using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APT615.Models.ApartmentViewModels
{
    public class TrackedApartmentViewModel
    {
        public ICollection<Apartment> TrackedApartments { get; set; }
    }
}
