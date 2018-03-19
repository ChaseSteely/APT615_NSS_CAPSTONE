using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace APT615.Models
{
    public class Amenity
    {
        // Primary Key
        [Key]
        public int AmenityId { get; set; }

        //String to reference the type of Amenity
        [Required]
        [StringLength(55)]
        public string Type { get; set; }

        //Identity magic for the user
        [Required]
        public ApplicationUser User { get; set; }

        //entity magic - reference to the Amenities of this Apartment aka Join Table
        public virtual ICollection<ApartmentAmenity> ApartmentAmenities { get; set; }
    }
}