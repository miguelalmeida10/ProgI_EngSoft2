using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalScheduling.Data;
using HospitalScheduling.Models.ViewModels;

namespace HospitalScheduling.Models
{
    public class PastShiftsController : Controller
    {
        private readonly ApplicationDbContext _context;
        PagingViewModel paging = new PagingViewModel()
        {
            CurrentPage = 1
        };

        public PastShiftsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PastShifts
        public async Task<IActionResult> Index(string search = "", int page = 1, int lazy = 1)
        {
            #region Search, Sort & Pagination Related Region
                #region Variable to obtain past shifts including thier specialities that skips 5 * number of items per page
                    var applicationDbContext = await _context.PastShifts.Include(p => p.Doctor).Include(p => p.Shift).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                    if (!string.IsNullOrEmpty(search))
                    {
                        applicationDbContext = (await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).Where(ds => ds.Doctor.Name.Contains(search) || ds.Shift.Name.Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = _context.PastShifts.Count();
                #endregion
            #endregion

            return View(new PastShiftsViewModel { PastShifts = applicationDbContext, Pagination = paging });
        }

        // Post: PastShifts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            #region Search, Sort & Pagination Related Region
                #region Variable to obtain past shifts including thier specialities that skips 5 * number of items per page, filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                    var applicationDbContext = await _context.PastShifts.Include(p => p.Doctor).Include(p => p.Shift).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and f
                    if (!string.IsNullOrEmpty(search))
                    {
                        applicationDbContext = (await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).Where(ds => ds.Doctor.Name.Contains(search) || ds.Shift.Name.Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = _context.PastShifts.Count();
            #endregion
            #endregion

            ViewData["Search"] = search;
            return View(new PastShiftsViewModel { PastShifts = applicationDbContext, Pagination = paging });
        }

        // GET: PastShifts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastShifts = await _context.PastShifts
                .Include(p => p.Doctor)
                .Include(p => p.Shift)
                .FirstOrDefaultAsync(m => m.HistoryID == id);
            if (pastShifts == null)
            {
                return NotFound();
            }

            return View(pastShifts);
        }

        // GET: PastShifts/Create
        public IActionResult Create()
        {
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Phone");
            ViewData["ShiftID"] = new SelectList(_context.Shift, "ShiftID", "ShiftID");
            return View();
        }

        // POST: PastShifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HistoryID,DoctorID,ShiftID,ShiftEndDate")] PastShifts pastShifts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pastShifts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Phone", pastShifts.DoctorID);
            ViewData["ShiftID"] = new SelectList(_context.Shift, "ShiftID", "ShiftID", pastShifts.ShiftID);
            return View(pastShifts);
        }

        // GET: PastShifts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastShifts = await _context.PastShifts.FindAsync(id);
            if (pastShifts == null)
            {
                return NotFound();
            }
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Phone", pastShifts.DoctorID);
            ViewData["ShiftID"] = new SelectList(_context.Shift, "ShiftID", "ShiftID", pastShifts.ShiftID);
            return View(pastShifts);
        }

        // POST: PastShifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HistoryID,DoctorID,ShiftID,ShiftEndDate")] PastShifts pastShifts)
        {
            if (id != pastShifts.HistoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pastShifts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PastShiftsExists(pastShifts.HistoryID))
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
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Phone", pastShifts.DoctorID);
            ViewData["ShiftID"] = new SelectList(_context.Shift, "ShiftID", "ShiftID", pastShifts.ShiftID);
            return View(pastShifts);
        }

        // GET: PastShifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastShifts = await _context.PastShifts
                .Include(p => p.Doctor)
                .Include(p => p.Shift)
                .FirstOrDefaultAsync(m => m.HistoryID == id);
            if (pastShifts == null)
            {
                return NotFound();
            }

            return View(pastShifts);
        }

        // POST: PastShifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pastShifts = await _context.PastShifts.FindAsync(id);
            _context.PastShifts.Remove(pastShifts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PastShiftsExists(int id)
        {
            return _context.PastShifts.Any(e => e.HistoryID == id);
        }
    }
}
