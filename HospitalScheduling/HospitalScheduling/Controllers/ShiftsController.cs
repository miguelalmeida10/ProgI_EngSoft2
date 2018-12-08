using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalScheduling.Data;
using HospitalScheduling.Models;
using HospitalScheduling.Models.ViewModels;

namespace HospitalScheduling.Controllers
{
    public class ShiftsController : Controller
    {
        private readonly ApplicationDbContext _context;
        PagingViewModel paging = new PagingViewModel()
        {
            CurrentPage = 1
        };

        public ShiftsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Shifts
        public async Task<IActionResult> Index(string search = "", int page = 1, int lazy = 1)
        {
            #region Clean Ended Shifts
                var shiftlist = await _context.Shift.Include(d => d.Doctors).Where(s => !s.Ended).ToListAsync();
                var list = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).ToListAsync();
                List<DoctorShifts> shiftsApagar = new List<DoctorShifts>();
                List<PastShifts> pastShiftsApagar = new List<PastShifts>();
                List<Shift> shiftActualizar = new List<Shift>();

                shiftlist.ForEach(m =>
                {
                    bool activo = DateTime.Now.Subtract(m.ShiftStartHour.AddDays(DateTime.Now.DayOfYear - 1).AddYears(DateTime.Now.Year - 1)).Milliseconds > 0 && m.ShiftStartHour.AddDays(DateTime.Now.DayOfYear - 1).AddYears(DateTime.Now.Year - 1).AddHours(m.DurationHours).AddMinutes(m.DurationMinutes).Subtract(DateTime.Now).TotalMilliseconds > 0 && m.EndDate.Date.CompareTo(m.StartDate) < 0;
                    if (!activo && m.EndDate.Date.CompareTo(DateTime.Now) < 0)
                    {
                        m.Active = false;
                        shiftActualizar.Add(m);
                        var docshifts = list.Where(s => s.Shift == m).FirstOrDefault();
                        if (docshifts != null)
                        {
                            shiftsApagar.Add(list.Where(s => s.Shift == m).FirstOrDefault());
                            pastShiftsApagar.Add(new PastShifts(list.Where(s => s.Shift == m).FirstOrDefault()));
                        }
                    }
                    else if (activo && m.EndDate.Date.CompareTo(DateTime.Now) < 0)
                    {
                        m.Active = true;
                        shiftActualizar.Add(m);
                    }
                    else if (m.EndDate.Date.CompareTo(DateTime.Now) >= 0)
                    {
                        m.Ended = true;
                        shiftActualizar.Add(m);
                        var docshifts = list.Where(s => s.Shift == m).FirstOrDefault();
                        if (docshifts != null)
                        {
                            shiftsApagar.Add(list.Where(s => s.Shift == m).FirstOrDefault());
                            pastShiftsApagar.Add(new PastShifts(list.Where(s => s.Shift == m).FirstOrDefault()));
                        }
                    }
                });


                _context.Shift.UpdateRange(shiftActualizar);
                _context.PastShifts.AddRange(pastShiftsApagar);
                _context.DoctorShifts.RemoveRange(shiftsApagar);
                await _context.SaveChangesAsync();
            #endregion
            
            #region Search, Sort & Pagination Related Region
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                   var shifts = await _context.Shift.Where(s => s.Active && !s.Ended).Skip(paging.PageSize * (page - 1))
                        .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                    if (!string.IsNullOrEmpty(search))
                    {
                        shifts = (await _context.Shift.Where(s => s.Active && !s.Ended).Where(ds => ds.Name.Contains(search) || ds.ShiftID.ToString().Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = _context.Shift.Count();
                #endregion
            #endregion

            return View(new ShiftsViewModel { Shifts = shifts, Pagination = paging });
        }

        // Post: Shifts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string search, int page = 1)
        {

            #region Search, Sort & Pagination Related Region
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                   var shifts = await _context.Shift.Where(s => s.Active && !s.Ended).Skip(paging.PageSize * (page - 1))
                        .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                    if (!string.IsNullOrEmpty(search))
                    {
                        shifts = (await _context.Shift.Where(s => s.Active && !s.Ended).Where(ds => ds.Name.Contains(search) || ds.ShiftID.ToString().Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = _context.Shift.Count();
            #endregion
            #endregion

            ViewData["Search"] = search;
            return View(new ShiftsViewModel { Shifts = shifts, Pagination = paging });
        }

        // GET: Shifts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shift
                .FirstOrDefaultAsync(m => m.ShiftID == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // GET: Shifts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShiftID,Name,StartDate,EndDate,ShiftStartHour,DurationMinutes,DurationHours")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shift);
        }

        // GET: Shifts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shift.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }
            return View(shift);
        }

        // POST: Shifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShiftID,Name,StartDate,EndDate,ShiftStartHour,DurationMinutes,DurationHours")] Shift shift)
        {
            if (id != shift.ShiftID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShiftExists(shift.ShiftID))
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
            return View(shift);
        }

        // GET: Shifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shift
                .Where(s => s.Active && !s.Ended)
                .FirstOrDefaultAsync(m => m.ShiftID == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // GET: Shifts/DeleteConfirmation/5
        public async Task<IActionResult> DeleteConfirmation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shift
                .Where(s => s.Active && !s.Ended)
                .FirstOrDefaultAsync(m => m.ShiftID == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // POST: Shifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shift = await _context.Shift.FindAsync(id);
            _context.Shift.Remove(shift);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShiftExists(int id)
        {
            return _context.Shift.Where(s => s.Active && !s.Ended).Any(e => e.ShiftID == id);
        }
    }
}
