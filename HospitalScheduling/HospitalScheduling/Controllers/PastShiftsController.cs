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
        // int lazy = 1 so i dont have to rename Indexes Get or Indexes Post
        public async Task<IActionResult> Index(string search = "", string filter = "", string order = "", string asc = "", int page = 1, int lazy = 1)
        {            
            #region Deactivate and Remove from list of old shifts from index and activate new ones if its a new day
                var sh = _context.DoctorShifts.Include(d => d.Shift).Include(d => d.Doctor);
                var list = new List<PastShifts>();
                await sh.ForEachAsync(s => {
                    if (s.Shift.Ended && _context.PastShifts.Where(ps=> ps.Doctor == s.Doctor && ps.Shift == s.Shift && ps.ShiftEndDate == s.Shift.StartDate && ps.ShiftID == s.ShiftID && ps.DoctorID == s.DoctorID).FirstOrDefault() == null){
                        list.Add(new PastShifts() { Shift = s.Shift, Doctor = s.Doctor, DoctorID = s.DoctorID, ShiftEndDate = s.Shift.StartDate, ShiftID = s.ShiftID });
                    }
                });
                _context.PastShifts.UpdateRange(list);
                await _context.SaveChangesAsync();
            #endregion

            #region Search, Sort & Pagination Related Region
                int count = 0;
                #region Variable to obtain past shifts including thier specialities that skips 5 * number of items per page
                    var applicationDbContext = await _context.PastShifts.Include(p => p.Doctor).Include(p => p.Shift).OrderBy(p => order).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                    if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                        applicationDbContext = await _context.PastShifts.Include(p => p.Doctor).Include(p => p.Shift).OrderBy(p => order).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync();
                    else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                        applicationDbContext = await _context.PastShifts.Include(p => p.Doctor).Include(p => p.Shift).OrderByDescending(p => order).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                if (!string.IsNullOrEmpty(search))
                    {
                        switch (filter)
                        {
                            default:
                            case "All":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    applicationDbContext = await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(p => order).Where(ds => ds.Doctor.Name.Contains(search) || ds.Shift.Name.Contains(search)).ToListAsync();
                                else
                                    applicationDbContext = await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(p => order).Where(ds => ds.Doctor.Name.Contains(search) || ds.Shift.Name.Contains(search)).ToListAsync();
                                break;
                        case "Name":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    applicationDbContext = await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(p => order).Where(ds => ds.Doctor.Name.Contains(search)).ToListAsync();
                                else
                                    applicationDbContext = await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(p => order).Where(ds => ds.Doctor.Name.Contains(search)).ToListAsync();
                                break;
                        case "Shift":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    applicationDbContext = await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(p => order).Where(ds => ds.Shift.Name.Contains(search)).ToListAsync();
                                else
                                   applicationDbContext = await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(p => order).Where(ds => ds.Shift.Name.Contains(search)).ToListAsync();
                                break;
                        }
               
                        count = applicationDbContext.Count();
                        applicationDbContext = applicationDbContext.Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToList();
                        ViewData["Search"] = search;
                        ViewData["Filter"] = filter;
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.PastShifts.Count() : count;
                #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new PastShiftsViewModel { PastShifts = applicationDbContext, Pagination = paging });
        }

        // Post: PastShifts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string search, string filter, string order = "", string asc = "", int page = 1)
        {
            order = (string.IsNullOrEmpty(order)) ? "" : order;            
            #region Deactivate and Remove from list of old shifts from index and activate new ones if its a new day
                var sh = _context.DoctorShifts.Include(d => d.Shift).Include(d => d.Doctor);
                var list = new List<PastShifts>();
                await sh.ForEachAsync(s => {
                    if (s.Shift.Ended && _context.PastShifts.Where(ps=> ps.Doctor == s.Doctor && ps.Shift == s.Shift && ps.ShiftEndDate == s.Shift.StartDate && ps.ShiftID == s.ShiftID && ps.DoctorID == s.DoctorID).FirstOrDefault() == null){
                        list.Add(new PastShifts() { Shift = s.Shift, Doctor = s.Doctor, DoctorID = s.DoctorID, ShiftEndDate = s.Shift.StartDate, ShiftID = s.ShiftID });
                    }
                });
                _context.PastShifts.UpdateRange(list);
                await _context.SaveChangesAsync();
            #endregion
            #region Search, Sort & Pagination Related Region
                int count = 0;
                #region Variable to obtain past shifts including thier specialities that skips 5 * number of items per page
                    var applicationDbContext = await _context.PastShifts.Include(p => p.Doctor).Include(p => p.Shift).OrderBy(p => order).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                    if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                        applicationDbContext = await _context.PastShifts.Include(p => p.Doctor).Include(p => p.Shift).OrderBy(p => order).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync();
                    else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                        applicationDbContext = await _context.PastShifts.Include(p => p.Doctor).Include(p => p.Shift).OrderByDescending(p => order).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                if (!string.IsNullOrEmpty(search))
                    {
                        switch (filter)
                        {
                            default:
                            case "All":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    applicationDbContext = await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(p => order).Where(ds => ds.Doctor.Name.Contains(search) || ds.Shift.Name.Contains(search)).ToListAsync();
                                else
                                    applicationDbContext = await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(p => order).Where(ds => ds.Doctor.Name.Contains(search) || ds.Shift.Name.Contains(search)).ToListAsync();
                                break;
                        case "Name":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    applicationDbContext = await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(p => order).Where(ds => ds.Doctor.Name.Contains(search)).ToListAsync();
                                else
                                    applicationDbContext = await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(p => order).Where(ds => ds.Doctor.Name.Contains(search)).ToListAsync();
                                break;
                        case "Shift":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    applicationDbContext = await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(p => order).Where(ds => ds.Shift.Name.Contains(search)).ToListAsync();
                                else
                                   applicationDbContext = await _context.PastShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(p => order).Where(ds => ds.Shift.Name.Contains(search)).ToListAsync();
                                break;
                        }
               
                        count = applicationDbContext.Count();
                        applicationDbContext = applicationDbContext.Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToList();
                        ViewData["Search"] = search;
                        ViewData["Filter"] = filter;
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.PastShifts.Count() : count;
                #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

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
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Name");
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
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Name", pastShifts.DoctorID);
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
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Name", pastShifts.DoctorID);
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
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Name", pastShifts.DoctorID);
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
