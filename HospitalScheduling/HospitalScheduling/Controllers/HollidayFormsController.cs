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
    public class HollidayFormsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HollidayFormsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HollidayForms
        public async Task<IActionResult> Index()
        {
            return View(await _context.HollidayForm.ToListAsync());
        }

        // Post: HollidayForms
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return View(await _context.HollidayForm.Where(ds => ds.VacationID.ToString().Contains(search)).ToListAsync());
            }

            return View(await _context.HollidayForm.ToListAsync());
        }

        // GET: HollidayForms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hollidayForm = await _context.HollidayForm
                .FirstOrDefaultAsync(m => m.VacationID == id);
            if (hollidayForm == null)
            {
                return NotFound();
            }

            return View(hollidayForm);
        }

        // GET: HollidayForms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HollidayForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VacationID,DateStart,DateEnd")] HollidayForm hollidayForm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hollidayForm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hollidayForm);
        }

        // GET: HollidayForms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hollidayForm = await _context.HollidayForm.FindAsync(id);
            if (hollidayForm == null)
            {
                return NotFound();
            }
            return View(hollidayForm);
        }

        // POST: HollidayForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VacationID,DateStart,DateEnd")] HollidayForm hollidayForm)
        {
            if (id != hollidayForm.VacationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hollidayForm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HollidayFormExists(hollidayForm.VacationID))
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
            return View(hollidayForm);
        }

        // GET: HollidayForms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hollidayForm = await _context.HollidayForm
                .FirstOrDefaultAsync(m => m.VacationID == id);
            if (hollidayForm == null)
            {
                return NotFound();
            }

            return View(hollidayForm);
        }

        // POST: HollidayForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hollidayForm = await _context.HollidayForm.FindAsync(id);
            _context.HollidayForm.Remove(hollidayForm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HollidayFormExists(int id)
        {
            return _context.HollidayForm.Any(e => e.VacationID == id);
        }
    }
}
