using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APT615.Data;
using APT615.Models;

namespace APT615.Controllers
{
    public class ApartmentAmenitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApartmentAmenitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ApartmentAmenities
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ApartmentAmenities.Include(a => a.Amenitiz).Include(a => a.Apartment);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ApartmentAmenities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartmentAmenity = await _context.ApartmentAmenities
                .Include(a => a.Amenitiz)
                .Include(a => a.Apartment)
                .SingleOrDefaultAsync(m => m.ApartAmenityId == id);
            if (apartmentAmenity == null)
            {
                return NotFound();
            }

            return View(apartmentAmenity);
        }

        // GET: ApartmentAmenities/Create
        public IActionResult Create()
        {
            ViewData["AmenityId"] = new SelectList(_context.Amenities, "AmenityId", "Type");
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "ApartmentId", "Name");
            return View();
        }

        // POST: ApartmentAmenities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApartAmenityId,ApartmentId,AmenityId")] ApartmentAmenity apartmentAmenity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(apartmentAmenity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AmenityId"] = new SelectList(_context.Amenities, "AmenityId", "Type", apartmentAmenity.AmenityId);
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "ApartmentId", "Name", apartmentAmenity.ApartmentId);
            return View(apartmentAmenity);
        }

        // GET: ApartmentAmenities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartmentAmenity = await _context.ApartmentAmenities.SingleOrDefaultAsync(m => m.ApartAmenityId == id);
            if (apartmentAmenity == null)
            {
                return NotFound();
            }
            ViewData["AmenityId"] = new SelectList(_context.Amenities, "AmenityId", "Type", apartmentAmenity.AmenityId);
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "ApartmentId", "Name", apartmentAmenity.ApartmentId);
            return View(apartmentAmenity);
        }

        // POST: ApartmentAmenities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApartAmenityId,ApartmentId,AmenityId")] ApartmentAmenity apartmentAmenity)
        {
            if (id != apartmentAmenity.ApartAmenityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apartmentAmenity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartmentAmenityExists(apartmentAmenity.ApartAmenityId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AmenityId"] = new SelectList(_context.Amenities, "AmenityId", "Type", apartmentAmenity.AmenityId);
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "ApartmentId", "Name", apartmentAmenity.ApartmentId);
            return View(apartmentAmenity);
        }

        // GET: ApartmentAmenities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartmentAmenity = await _context.ApartmentAmenities
                .Include(a => a.Amenitiz)
                .Include(a => a.Apartment)
                .SingleOrDefaultAsync(m => m.ApartAmenityId == id);
            if (apartmentAmenity == null)
            {
                return NotFound();
            }

            return View(apartmentAmenity);
        }

        // POST: ApartmentAmenities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apartmentAmenity = await _context.ApartmentAmenities.SingleOrDefaultAsync(m => m.ApartAmenityId == id);
            _context.ApartmentAmenities.Remove(apartmentAmenity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentAmenityExists(int id)
        {
            return _context.ApartmentAmenities.Any(e => e.ApartAmenityId == id);
        }
    }
}
