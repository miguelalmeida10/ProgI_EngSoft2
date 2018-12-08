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
    public class DoctorShiftsController : Controller
    {
        private readonly ApplicationDbContext _context;
        PagingViewModel paging = new PagingViewModel()
        {
            CurrentPage = 1
        };

        public DoctorShiftsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DoctorShifts
        public async Task<IActionResult> Index(string search = "", int page = 1, int lazy = 1)
        {
            #region Clean Ended Shifts
            var shiftlist = await _context.Shift.Include(d => d.Doctors).Where(s => !s.Ended).ToListAsync();
            var list = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).ToListAsync();
            List<DoctorShifts> shiftsApagar = new List<DoctorShifts>();
            List<PastShifts> pastShiftsApagar = new List<PastShifts>();
            List<Shift> shiftActualizar = new List<Shift>();

            list.ForEach(m =>
            {
                bool activo = DateTime.Now.Subtract(m.Shift.ShiftStartHour.AddDays(DateTime.Now.DayOfYear - 1).AddYears(DateTime.Now.Year - 1)).Milliseconds > 0 && m.Shift.ShiftStartHour.AddDays(DateTime.Now.DayOfYear - 1).AddYears(DateTime.Now.Year - 1).AddHours(m.Shift.DurationHours).AddMinutes(m.Shift.DurationMinutes).Subtract(DateTime.Now).TotalMilliseconds > 0 && m.Shift.EndDate.Date.CompareTo(m.Shift.StartDate) < 0;
                if (!activo && m.Shift.EndDate.Date.CompareTo(DateTime.Now) < 0)
                {
                    m.Shift.Active = false;
                    shiftActualizar.Add(m.Shift);

                    var docshifts = list.Where(s => s.Shift == m.Shift).FirstOrDefault();
                    if (docshifts != null)
                    {
                        shiftsApagar.Add(list.Where(s => s.Shift == m.Shift).FirstOrDefault());
                        pastShiftsApagar.Add(new PastShifts(list.Where(s => s.Shift == m.Shift).FirstOrDefault()));
                    }
                }
                else if (activo && m.Shift.EndDate.Date.CompareTo(DateTime.Now) < 0)
                {
                    m.Shift.Active = true;
                    shiftActualizar.Add(m.Shift);
                }
                else if (m.Shift.EndDate.Date.CompareTo(DateTime.Now) >= 0)
                {
                    m.Shift.Ended = true;
                    shiftActualizar.Add(m.Shift);

                    var docshifts = list.Where(s => s.Shift == m.Shift).FirstOrDefault();
                    if (docshifts != null)
                    {
                        shiftsApagar.Add(list.Where(s => s.Shift == m.Shift).FirstOrDefault());
                        pastShiftsApagar.Add(new PastShifts(list.Where(s => s.Shift == m.Shift).FirstOrDefault()));
                    }
                }
            });

            _context.Shift.UpdateRange(shiftActualizar);
            _context.PastShifts.AddRange(pastShiftsApagar);
            _context.DoctorShifts.RemoveRange(shiftsApagar);
            await _context.SaveChangesAsync();
            #endregion

            #region Search, Sort & Pagination Related Region
                #region Variable to obtain doctorshifts including thier specialities that skips 5 * number of items per page
                    var applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                    if (!string.IsNullOrEmpty(search))
                    {
                        applicationDbContext = (await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).Where(ds => ds.Doctor.Name.Contains(search) || ds.Shift.Name.Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = _context.DoctorShifts.Count();
                #endregion
            #endregion

            return View(new DoctorShiftsViewModel { DoctorShifts = applicationDbContext, Pagination = paging });
        }

        // Post: DoctorShifts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            #region Search, Sort & Pagination Related Region
                #region Variable to obtain doctorshifts including thier specialities that skips 5 * number of items per page
                    var applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                    if (!string.IsNullOrEmpty(search))
                    {
                        applicationDbContext = (await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).Where(ds => ds.Doctor.Name.Contains(search) || ds.Shift.Name.Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = _context.DoctorShifts.Count();
            #endregion
            #endregion

            ViewData["Search"] = search;
            return View(new DoctorShiftsViewModel { DoctorShifts = applicationDbContext, Pagination = paging });
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


        // GET: DoctorShifts/DeleteConfirmation/5
        public async Task<IActionResult> DeleteConfirmation(int? id)
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
