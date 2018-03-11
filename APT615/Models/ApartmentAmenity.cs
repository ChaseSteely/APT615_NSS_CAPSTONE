using System.ComponentModel.DataAnnotations;

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
        public Amenity Amenity { get; set; }
    }
}