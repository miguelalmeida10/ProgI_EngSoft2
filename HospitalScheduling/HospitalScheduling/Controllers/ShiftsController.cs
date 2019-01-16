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

        /// <summary>
        /// Adds shifts for every day of the current year if not already populated with this years shifts or at all
        /// </summary>
        public void GenerateShifts()
        {
            Task newShifts = Task.Run(() =>
            {
                if (_context.Shift.Count() == 0 || _context.Shift.LastOrDefault().StartDate.Year < DateTime.Now.Year)
                {
                    List<Shift> shiftsOfYear = new List<Shift>();
                    for (int i = DateTime.Now.Month; i <= 12; i++)
                    {
                        int maxDias = (DateTime.IsLeapYear(DateTime.Now.Year) && i == 2) ? 29 : 28;
                        int maxDiasMes = 0;

                        switch (i)
                        {
                            case 4:
                            case 6:
                            case 9:
                            case 11:
                                maxDiasMes = 30;
                                break;
                            case 1:
                            case 3:
                            case 5:
                            case 7:
                            case 8:
                            case 10:
                            case 12:
                                maxDiasMes = 31;
                                break;
                        };

                        maxDias = (!DateTime.IsLeapYear(DateTime.Now.Year) && i == 2) ? 28 : maxDiasMes;

                        for (int j = DateTime.Now.Day; j <= maxDias; j++)
                        {
                            shiftsOfYear.Add(new Shift { Name = new DateTime(DateTime.Now.Year, i, j, 8, 0, 0).ToLongDateString() + " - Manha", StartDate = new DateTime(DateTime.Now.Year, i, j, 8, 0, 0), Active = (j == DateTime.Now.Day ? true : false), Ended = false });
                            shiftsOfYear.Add(new Shift { Name = new DateTime(DateTime.Now.Year, i, j, 8, 0, 0).ToLongDateString() + " - Tarde", StartDate = new DateTime(DateTime.Now.Year, i, j, 16, 0, 0), Active = (j == DateTime.Now.Day ? true : false), Ended = false });
                            shiftsOfYear.Add(new Shift { Name = new DateTime(DateTime.Now.Year, i, j, 8, 0, 0).ToLongDateString() + " - Noite", StartDate = new DateTime(DateTime.Now.Year, i, j, 23, 59, 59), Active = (j == DateTime.Now.Day ? true : false), Ended = false });
                        }
                    }
                    _context.Shift.AddRange(shiftsOfYear);
                }
            });

            newShifts.Wait();

            _context.SaveChanges();
        }

        // GET: Shifts
        // int lazy = 1 so i dont have to rename Indexes Get or Indexes Post
        public async Task<IActionResult> Index(string search = "", string filter = "", string order = "", string asc = "", int page = 1, int lazy = 1)
        {
            GenerateShifts();
            #region Deactivate and Remove from list of old shifts from index and activate new ones if its a new day
                var sh = _context.Shift;
                var list = new List<Shift>();
                await sh.ForEachAsync(s => {
                    if (s.StartDate.Subtract(DateTime.Now).Hours < 0 && s.StartDate.Year <= DateTime.Now.Year && s.StartDate.Month <= DateTime.Now.Month)
                    {
                        s.Active = false;
                        s.Ended = true;
                        list.Add(s);
                    }
                    else if (s.StartDate.Subtract(DateTime.Now).Hours >= 0 && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month)
                    {
                        s.Active = true;
                        s.Ended = false;
                        list.Add(s);
                    }
                    else
                    {
                        s.Active = false;
                        s.Ended = false;
                        list.Add(s);
                    }
                });
                _context.Shift.UpdateRange(list);
                await _context.SaveChangesAsync();
            #endregion

            #region Search, Sort & Pagination Related Region
            int count = 0;
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                   string oldOrder = order;
                   order = (order.Equals("Name")?"StartDate":order);
                   var shifts = await _context.Shift.OrderBy(d=> d.StartDate.Day).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Skip(paging.PageSize * (page - 1))
                        .Take(paging.PageSize).ToListAsync();
                    if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                        shifts = await _context.Shift.OrderBy(d=> d.StartDate.Day).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync();
                    else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                        shifts = await _context.Shift.OrderByDescending(d => d.StartDate.Day).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Skip(paging.PageSize * (page - 1))
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
                                    shifts = await _context.Shift.OrderByDescending(d => d.StartDate.Day).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Where(ds => ds.Name.Contains(search) || ds.ShiftID.ToString().Contains(search)).ToListAsync();
                                else
                                    shifts = await _context.Shift.OrderBy(d=> d.StartDate.Day).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Where(ds => ds.Name.Contains(search) || ds.ShiftID.ToString().Contains(search)).ToListAsync();
                                break;
                            case "Name":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    shifts = await _context.Shift.OrderByDescending(d => d.StartDate.Day).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Where(ds => ds.Name.Contains(search)).ToListAsync();
                                else
                                    shifts = await _context.Shift.OrderBy(d=> d.StartDate.Day).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Where(ds => ds.Name.Contains(search)).ToListAsync();
                                break;
                            case "ID":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    shifts = await _context.Shift.OrderByDescending(d => d.StartDate.Day).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Where(ds => ds.ShiftID.ToString().Contains(search)).ToListAsync();
                                else
                                    shifts = await _context.Shift.OrderBy(d=> d.StartDate.Day).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Where(ds => ds.ShiftID.ToString().Contains(search)).ToListAsync();
                                break;
                        }
               
                        count = shifts.Count();
                        shifts = shifts.Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToList();
                        ViewData["Search"] = search;
                        ViewData["Filter"] = filter;
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.Shift.Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Count() : count;
                #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : oldOrder;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new ShiftsViewModel { Shifts = shifts, Pagination = paging });
        }

        // Post: Shifts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string search, string filter, string order = "", string asc = "", int page = 1)
        {
            order = (string.IsNullOrEmpty(order)) ? "" : order;            
            #region Deactivate and Remove from list of old shifts from index and activate new ones if its a new day
                var sh = _context.Shift;
                var list = new List<Shift>();
                await sh.ForEachAsync(s => {
                    if (s.StartDate.Subtract(DateTime.Now).Hours < 0 && s.StartDate.Year <= DateTime.Now.Year && s.StartDate.Month <= DateTime.Now.Month)
                    {
                        s.Active = false;
                        s.Ended = true;
                        list.Add(s);
                    }
                    else if (s.StartDate.Subtract(DateTime.Now).Hours >= 0 && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month)
                    {
                        s.Active = true;
                        s.Ended = false;
                        list.Add(s);
                    }
                    else
                    {
                        s.Active = false;
                        s.Ended = false;
                        list.Add(s);
                    }
                });
                _context.Shift.UpdateRange(list);
                await _context.SaveChangesAsync();
            #endregion

            #region Search, Sort & Pagination Related Region
            int count = 0;
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                   string oldOrder = order;
                   order = (order.Equals("Name")?"StartDate":order);
                   var shifts = await _context.Shift.OrderBy(d=> d.StartDate.Day).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Skip(paging.PageSize * (page - 1))
                        .Take(paging.PageSize).ToListAsync();
                    if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                        shifts = await _context.Shift.OrderBy(d=> d.StartDate.Day).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync();
                    else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                        shifts = await _context.Shift.OrderByDescending(d => d.StartDate.Day).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Skip(paging.PageSize * (page - 1))
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
                                    shifts = await _context.Shift.OrderByDescending(d => d.StartDate.Day).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Where(ds => ds.Name.Contains(search) || ds.ShiftID.ToString().Contains(search)).ToListAsync();
                                else
                                    shifts = await _context.Shift.OrderBy(d=> d.StartDate.Day).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Where(ds => ds.Name.Contains(search) || ds.ShiftID.ToString().Contains(search)).ToListAsync();
                                break;
                            case "Name":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    shifts = await _context.Shift.OrderByDescending(d => d.StartDate.Day).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Where(ds => ds.Name.Contains(search)).ToListAsync();
                                else
                                    shifts = await _context.Shift.OrderBy(d=> d.StartDate.Day).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Where(ds => ds.Name.Contains(search)).ToListAsync();
                                break;
                            case "ID":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    shifts = await _context.Shift.OrderByDescending(d => d.StartDate.Day).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Where(ds => ds.ShiftID.ToString().Contains(search)).ToListAsync();
                                else
                                    shifts = await _context.Shift.OrderBy(d=> d.StartDate.Day).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("StartDate") ? d.StartDate.ToString() : order.Equals("TimeLeft") ? d.StartDate.ToString() : order.Equals("EndDate") ? d.StartDate.ToString() : d.Name).Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Where(ds => ds.ShiftID.ToString().Contains(search)).ToListAsync();
                                break;
                        }
               
                        count = shifts.Count();
                        shifts = shifts.Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToList();
                        ViewData["Search"] = search;
                        ViewData["Filter"] = filter;
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.Shift.Where(s => s.Active && !s.Ended && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).Count() : count;
                #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : oldOrder;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

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
        public async Task<IActionResult> Create([Bind("ShiftID,Name,StartDate,EndDate,ShiftStart,ShiftStartHour,ShiftStartMinute,DurationMinutes,DurationHours")] Shift shift)
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
        public async Task<IActionResult> Edit(int id, [Bind("ShiftID,Name,StartDate,EndDate,ShiftStart,ShiftStartHour,ShiftStartMinute,DurationMinutes,DurationHours")] Shift shift)
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
