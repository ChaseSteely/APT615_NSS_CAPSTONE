using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APT615.Models
{
    public class Amenity
    {
        // Primary Key
        [Key]
        public int AmenityId { get; set; }

        //String to reference the type of account ie VISA / PAYPAL
        [Required]
        [StringLength(55)]
        public string Type { get; set; }

        //entity magic - reference to the Amenities of this Apartment aka Join Table
        public virtual ICollection<ApartmentAmenity> ApartmentAmenity { get; set; }
    }
}