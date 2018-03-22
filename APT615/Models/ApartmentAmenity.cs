using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace APT615.Models
{
    public class ApartmentAmenity
    {
        //Primary Key
        [Key]
        public int ApartAmenityId { get; set; }

        //Foreign Key Relations - Made nullable because of cascade delete
        public int? ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public int? AmenityId { get; set; }
        public Amenity Amenities { get; set; }
    }
}