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

            var apartment = await _context.Apartments
                .SingleOrDefaultAsync(m => m.ApartmentId == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }
        [HttpPost, ActionName("AddAmenity")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAmenity(int? id)
        {
            Console.WriteLine(id);
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var apartment = await _context.Apartments
                    .SingleOrDefaultAsync(a => a.ApartmentId == id && a.User == user);
                Console.WriteLine(apartment);
                //_context.Add(apartment.ApartmentAmenities);
                //await _context.SaveChangesAsync();
                return View();
            }
            return View();
        }

        // POST: Apartments/Edit/5
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
            var apartmentToUpdate = await _context.Apartments.SingleOrDefaultAsync(a => a.ApartmentId == id && a.User == user);
            if (await TryUpdateModelAsync<Apartment>(
                apartmentToUpdate,
                "",
                a => a.Favorited, a => a.Visited, a => a.Rent, a => a.MiscFees, a => a.Note
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
            return View(apartmentToUpdate);
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
