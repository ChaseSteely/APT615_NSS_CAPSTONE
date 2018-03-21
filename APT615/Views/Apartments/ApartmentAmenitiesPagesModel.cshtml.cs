using APT615.Data;
using APT615.Models;
using APT615.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APT615.Views.Apartments
{
    public class ApartmentAmenitiesPageModel : PageModel
    {
        public List<AssignedAmenities> AssignedAmenitiesList;

        public void PopulateAssignedAmenities(ApplicationDbContext _context, Apartment apartment)
        {
            var allAmenities = _context.Amenities;
            var apartmentAmenities = new HashSet<int?>(
                apartment.ApartmentAmenities
                .Select(a => a.AmenityId)
                );
            AssignedAmenitiesList = new List<AssignedAmenities>();
            foreach (var amenity in allAmenities)
            {
                AssignedAmenitiesList.Add(new AssignedAmenities
                {
                    AmenityId = amenity.AmenityId,
                    Type = amenity.Type,
                    HasAmenity = apartmentAmenities.Contains(amenity.AmenityId)
                });
            }

        }

        public void UpdateApartmentAmenities(ApplicationDbContext _context, string[] selectedAmenities, Apartment apartmentToUpdate)
        {
            if (selectedAmenities == null)
            {
                apartmentToUpdate.ApartmentAmenities = new List<ApartmentAmenity>();
                return;
            }

            var selectedAmenitiesHS = new HashSet<string>(selectedAmenities);
            var apartmentAmenities = new HashSet<int?>(apartmentToUpdate.ApartmentAmenities.Select(a => a.AmenityId));
            foreach (var amenity in _context.Amenities)
            {
                if (selectedAmenitiesHS.Contains(amenity.AmenityId.ToString()))
                {
                    if (!apartmentAmenities.Contains(amenity.AmenityId))
                    {
                        apartmentToUpdate.ApartmentAmenities.Add(
                            new ApartmentAmenity
                            {
                                ApartmentId = apartmentToUpdate.ApartmentId,
                                AmenityId = amenity.AmenityId
                            });
                    }
                }
                else
                {
                    if (apartmentAmenities.Contains(amenity.AmenityId))
                    {
                        ApartmentAmenity amenityToRemove
                            = apartmentToUpdate
                                .ApartmentAmenities
                                .SingleOrDefault(i => i.AmenityId == amenity.AmenityId);
                        _context.Remove(amenityToRemove);
                    }
                }
            }
        }
    }
}
