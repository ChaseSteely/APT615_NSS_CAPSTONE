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
using Microsoft.AspNetCore.Authorization;
using APT615.Models.ViewModels;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

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
        public async Task<IActionResult> TrackedApartments(int? id, int? amenityId)
        {
            var user = await GetCurrentUserAsync();
            var model = new ApartmentIndexData();
            model.Apartments = await _context.Apartments
                .Include(aa => aa.ApartmentAmenities)
                .ThenInclude(a => a.Amenities)
                .Where(m => m.User == user)
                .OrderBy(i => i.Name)
                .ToListAsync();

            if (id != null)
            {
                ViewData["ApartmentId"] = id.Value;
                Apartment apartment = model.Apartments
                .Where(a => a.ApartmentId == id.Value)
                .Single();
                model.Amenities = apartment.ApartmentAmenities
                    .Select(a => a.Amenities);
            }
            return View(model);
        }

        // GET: Map
        public async Task<IActionResult> ApartmentMap(int? id, int? amenityId)
        {
            var user = await GetCurrentUserAsync();
            var model = new ApartmentIndexData();
            model.Apartments = await _context.Apartments
                .Include(aa => aa.ApartmentAmenities)
                .ThenInclude(a => a.Amenities)
                .Where(m => m.User == user)
                .ToListAsync();

            if (id != null)
            {
                ViewData["ApartmentId"] = id.Value;
                Apartment apartment = model.Apartments
                .Where(a => a.ApartmentId == id.Value)
                .Single();
                model.Amenities = apartment.ApartmentAmenities
                    .Select(a => a.Amenities);
            }
            return View(model);
        }



        // GET: Apartments
        public async Task<IActionResult> Index(int? id, int? amenityId)
        {
            var user = await GetCurrentUserAsync();
            var model = new ApartmentIndexData();
            model.Apartments = await _context.Apartments
                .Include(aa => aa.ApartmentAmenities)
                .ThenInclude(a => a.Amenities)
                .Where(m => m.User == user)
                .OrderBy(i => i.Name)
                .ToListAsync();

            if (id != null)
            {
                ViewData["ApartmentId"] = id.Value;
                Apartment apartment = model.Apartments
                .Where(a => a.ApartmentId == id.Value)
                .Single();
                model.Amenities = apartment.ApartmentAmenities
                    .Select(a => a.Amenities);
            }
            return View(model);
        }

        public IActionResult Map(DataTable dataTable)
        {
            List<ApartmentIndexData> mapList = new List<ApartmentIndexData>();
            foreach (DataRow dr in dataTable.Rows)
            {
                ApartmentIndexData MapAddress = new ApartmentIndexData();
                var point = MapAddress;
                MapAddress.Apartment.Latitude = point.Apartment.Latitude;
                MapAddress.Apartment.Longitude = point.Apartment.Longitude;
                mapList.Add(MapAddress);
            }
            return View();
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
            var apartment = new Apartment();
            apartment.ApartmentAmenities = new List<ApartmentAmenity>();
            PopulateAssignedAmenities(apartment);
            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Track([Bind("ApartmentId,Name,Area,Rent,ApplicationFee,PetFee,MiscFees,AdminFee,DateAdded,Note,Bedrooms,Bathrooms,SqFt,Street,City,State,ZipCode,Latitude,Longitude,Website,PhotoUrl,Favorited,Visited")] Apartment apartment, string[] selectedAmenities)
        {
            if (selectedAmenities != null)
            {
                apartment.ApartmentAmenities = new List<ApartmentAmenity>();
                foreach (var amenity in selectedAmenities)
                {
                    var amenityToAdd = new ApartmentAmenity { ApartmentId = apartment.ApartmentId, AmenityId = int.Parse(amenity) };
                }
            }
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                apartment.User = await _userManager.GetUserAsync(HttpContext.User);
                _context.Add(apartment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TrackedApartments));
            }
            PopulateAssignedAmenities(apartment);
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
                .Include(a => a.ApartmentAmenities).ThenInclude(a => a.Amenities)
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
                .ThenInclude(a => a.Amenities)
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
                return RedirectToAction(nameof(TrackedApartments));
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
            Apartment apartment = await _context.Apartments
                .Include(a => a.ApartmentAmenities)
                .SingleAsync(m => m.ApartmentId == id);
            _context.Apartments.Remove(apartment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentExists(int id)
        {
            return _context.Apartments.Any(e => e.ApartmentId == id);
        }
        // GET: Amenities/Create
        public IActionResult CreateAmenity()
        {
            return View();
        }

        // POST: Amenities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAmenity([Bind("AmenityId,Type")] Amenity amenity)
        {
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                amenity.User = await _userManager.GetUserAsync(HttpContext.User);
                var user = await GetCurrentUserAsync();
                var isDuplicate = await _context.Amenities
                   .SingleOrDefaultAsync(a => a.User == user && a.Type == amenity.Type);
                if (isDuplicate != null)
                {
                    ViewData["Message"] = "You Have Already Added This Amenity.";
                    return View(amenity);
                }
                _context.Add(amenity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(amenity);
        }

        // GET: Amenities/Edit/5
        public async Task<IActionResult> EditAmenity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amenity = await _context.Amenities.SingleOrDefaultAsync(m => m.AmenityId == id);
            if (amenity == null)
            {
                return NotFound();
            }
            return View(amenity);
        }

        // POST: Amenities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();
            var amenityToUpdate = await _context.Amenities.SingleOrDefaultAsync(a => a.AmenityId == id && a.User == user);
            if (await TryUpdateModelAsync<Amenity>(
                amenityToUpdate,
                "",
                a => a.Type
                ))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }
            return View(amenityToUpdate);
        }

        // GET: Amenities/Delete/5
        public async Task<IActionResult> DeleteAmenity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amenity = await _context.Amenities
                .SingleOrDefaultAsync(m => m.AmenityId == id);
            if (amenity == null)
            {
                return NotFound();
            }

            return View(amenity);
        }

        // POST: Amenities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAmenity(int id)
        {
            var amenity = await _context.Amenities.SingleOrDefaultAsync(m => m.AmenityId == id);
            _context.Amenities.Remove(amenity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmenityExists(int id)
        {
            return _context.Amenities.Any(e => e.AmenityId == id);
        }
    }
}
