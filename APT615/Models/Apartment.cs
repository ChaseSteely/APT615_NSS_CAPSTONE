using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace APT615.Models
{
    public class Apartment
    {
        // Primary Key
        [Key]
        public int ApartmentId { get; set; }

        //Apartment Name
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        //Neighborhood
        [StringLength(50)]
        public string Area { get; set; }

        //Monthly Rent
        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Rent { get; set; }

        //Application Fee
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double ApplicationFee { get; set; }

        //Pet Fee
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double PetFee { get; set; }

        //Misc Monthly Fees
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double MiscFees { get; set; }

        //Admin Fee
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double AdminFee { get; set; }

        //Date created will default to current date with getdate() on application context file
        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateAdded { get; set; }

        //Apartment Notes
        [StringLength(255)]
        public string Note { get; set; }

        //Number of bedrooms
        [Required]
        public int Bedrooms { get; set; }

        //Number of bathrooms
        [Required]
        public int Bathrooms { get; set; }

        //Squre Feet
        [Required]
        public int SqFt { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public string Website { get; set; }

        public string PhotoUrl { get; set; }

        public bool Favorited { get; set; }

        public bool Visited { get; set; }

        //Identity magic for the user
        [Required]
        public ApplicationUser User { get; set; }

        //entity magic - reference to the Amenities of this Apartment
        public virtual ICollection<ApartmentAmenity> ApartmentAmenities { get; set; }


    }
}
