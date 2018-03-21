using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APT615.Data;
using APT615.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using APT615.Models.ApartmentViewModels.Track;
using APT615.Models.ApartmentViewModels;
using Microsoft.AspNetCore.Authorization;
using APT615.Models.ViewModels;

namespace APT615.Controllers
{
    public class ApartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _hostingEnvironment;

        public ApartmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = environment;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Apartments
        public async Task<IActionResult> TrackedApartments()
        {
            var user = await GetCurrentUserAsync();
            var model = new TrackedApartmentViewModel(_context);
            try
            {
                model.TrackedApartments = _context.Apartments
                    .Include(aa => aa.ApartmentAmenities)
                    .ThenInclude(a => a.Amenitiz)
                    .Where(m => m.User == user)
                    .ToList();
            }
            catch (NullReferenceException)
            {
                model.TrackedApartments = null;
            }

            return View(model);
        }

        public ICollection<Apartment> GetUserTrackedApartments(ApplicationUser user)
        {
            var TrackedApts = _context.Apartments
                .Where(m => m.User == user)
                .ToList();
            return (TrackedApts);
        }

        // GET: Apartments
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var AllApartments = await _context.Apartments
                .Where(m => m.User == user).ToListAsync();
            if (AllApartments == null)
            {
                return NotFound();
            }

            return View(AllApartments);
        }

        // GET: Apartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments
                .SingleOrDefaultAsync(m => m.ApartmentId == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // GET: Apartments/Track
        public IActionResult Track()
        {
            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Track([Bind("ApartmentId,Name,Area,Rent,ApplicationFee,PetFee,MiscFees,AdminFee,DateAdded,Note,Bedrooms,Bathrooms,SqFt,Street,City,State,ZipCode,Latitude,Longitude,Website,PhotoUrl,Favorited,Visited")] Apartment apartment)
        {
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                apartment.User = await _userManager.GetUserAsync(HttpContext.User);
                _context.Add(apartment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(apartment);
        }

        // GET: Apartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();
                var apartment = await _context.Apartments
                    .Include(a => a.ApartmentAmenities).ThenInclude(a => a.Amenitiz)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(m => m.ApartmentId == id && m.User == user);
            if (apartment == null)
            {
                ViewData["Message"] = "You Haven't Added Any Amenities.";
                return View(apartment);
            }
            PopulateAssignedAmenities(apartment);
            return View(apartment);

        }

        private void PopulateAssignedAmenities(Apartment apartment)
        {
            var allAmenities = _context.Amenities;
            var apartmentAmenities = new HashSet<int?>(apartment.ApartmentAmenities.Select(a => a.AmenityId));
            var viewModel = new List<AssignedAmenities>();
            if (allAmenities == null)
            {
                ViewData["Message"] = "You Haven't Added Any Amenities.";
            }
            foreach (var amenity in allAmenities)
            {
                viewModel.Add(new AssignedAmenities
                {
                    AmenityId = amenity.AmenityId,
                    Type = amenity.Type,
                    HasAmenity = apartmentAmenities.Contains(amenity.AmenityId)
                });
            }
            
            ViewData["Amenities"] = viewModel;

        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedAmenities)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();
            var apartmentToUpdate = await _context.Apartments
                .Include(i => i.ApartmentAmenities)
                .ThenInclude(a => a.Amenitiz)
                .SingleOrDefaultAsync(a => a.ApartmentId == id && a.User == user);
            if (await TryUpdateModelAsync<Apartment>(
                apartmentToUpdate,
                "",
                a => a.Favorited, a => a.Visited, a => a.Rent, a => a.MiscFees, a => a.Note, a => a.AdminFee,
                a => a.ApplicationFee, a => a.Area, a => a.SqFt, a => a.Website, a => a.Bathrooms,
                a => a.Bedrooms, a => a.PetFee, a => a.PhotoUrl
                ))
            {
                UpdateApartmentAmenities(selectedAmenities, apartmentToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes.");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateApartmentAmenities(selectedAmenities, apartmentToUpdate);
            PopulateAssignedAmenities(apartmentToUpdate);
            return View(apartmentToUpdate);

        }

        private void UpdateApartmentAmenities(string[] selectedAmenities, Apartment apartmentToUpdate)
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


        // GET: Apartments/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments
                   .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ApartmentId == id);
            if (apartment == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again.";
            }

            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apartment = await _context.Apartments.SingleOrDefaultAsync(m => m.ApartmentId == id);
            _context.Apartments.Remove(apartment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentExists(int id)
        {
            return _context.Apartments.Any(e => e.ApartmentId == id);
        }
    }
}
