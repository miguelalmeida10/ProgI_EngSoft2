using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalScheduling.Data;
using HospitalScheduling.Models;

namespace HospitalScheduling.Controllers
{
    public class DoctorShiftsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorShiftsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DoctorShifts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift);
            return View(await applicationDbContext.ToListAsync());
        }

        // Post: DoctorShifts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return View(await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).Where(ds => ds.Doctor.Name.Contains(search) || ds.Shift.Name.Contains(search)).ToListAsync());
            }

            return View(await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).ToListAsync());
        }

        // GET: DoctorShifts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorShifts = await _context.DoctorShifts
                .Include(d => d.Doctor)
                .Include(d => d.Shift)
                .FirstOrDefaultAsync(m => m.DoctorShiftID == id);
            if (doctorShifts == null)
            {
                return NotFound();
            }

            return View(doctorShifts);
        }

        // GET: DoctorShifts/Create
        public IActionResult Create()
        {
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Name");
            ViewData["ShiftID"] = new SelectList(_context.Shift, "ShiftID", "Name");
            return View();
        }

        // POST: DoctorShifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorShiftID,DoctorID,ShiftID")] DoctorShifts doctorShifts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctorShifts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Name", doctorShifts.DoctorID);
            ViewData["ShiftID"] = new SelectList(_context.Shift, "ShiftID", "Name", doctorShifts.ShiftID);
            return View(doctorShifts);
        }

        // GET: DoctorShifts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorShifts = await _context.DoctorShifts.FindAsync(id);
            if (doctorShifts == null)
            {
                return NotFound();
            }
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Name", doctorShifts.DoctorID);
            ViewData["ShiftID"] = new SelectList(_context.Shift, "ShiftID", "Name", doctorShifts.ShiftID);
            return View(doctorShifts);
        }

        // POST: DoctorShifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorShiftID,DoctorID,ShiftID")] DoctorShifts doctorShifts)
        {
            if (id != doctorShifts.DoctorShiftID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctorShifts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorShiftsExists(doctorShifts.DoctorShiftID))
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
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Name", doctorShifts.DoctorID);
            ViewData["ShiftID"] = new SelectList(_context.Shift, "ShiftID", "Name", doctorShifts.ShiftID);
            return View(doctorShifts);
        }

        // GET: DoctorShifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorShifts = await _context.DoctorShifts
                .Include(d => d.Doctor)
                .Include(d => d.Shift)
                .FirstOrDefaultAsync(m => m.DoctorShiftID == id);
            if (doctorShifts == null)
            {
                return NotFound();
            }

            return View(doctorShifts);
        }

        // POST: DoctorShifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctorShifts = await _context.DoctorShifts.FindAsync(id);
            _context.DoctorShifts.Remove(doctorShifts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorShiftsExists(int id)
        {
            return _context.DoctorShifts.Any(e => e.DoctorShiftID == id);
        }
    }
}
