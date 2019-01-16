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
using System.Threading;

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

        public int NumberOfDaysInMonth()
        {
            return DateTime.DaysInMonth(DateTime.Now.Year,DateTime.Now.Month);
        }

        // GET: DoctorShifts/AutoAssign
        public async Task<IActionResult> AutoAssign()
        {
            var shifts = new List<Shift>();
            int ndays = NumberOfDaysInMonth();
            List<DoctorShifts> doctorsInShifts = new List<DoctorShifts>();
            Dictionary<int, int> doctorShifts = new Dictionary<int, int>();
            var docslist = new List<Doctor>();


            _context.Shift.Where(s => s.StartDate.Month == DateTime.Now.Month).OrderBy(s => s.StartDate).ToList().ForEach(s =>
              {
                  if (_context.DoctorShifts.Where(ds => ds.Shift.StartDate.Month == DateTime.Now.Month).Count()<=0)
                  {
                      CleanWeek(docslist);
                      if (s.Name.Contains("Manha") || s.Name.Contains("Tarde"))
                      {
                          var temp = _context.Doctor.Include(d => d.Shifts).Include(d => d.PreviousShifts).ToList()
                                    .Where(d => d.WeeklyHours < 40 &&
                                               d.WorkingDays < 3 &&
                                               d.LastWorkDay.DayOfYear != s.StartDate.DayOfYear).ToList();
                          var tempList = new List<Doctor>();

                          if (temp.Count > 0)
                          {
                              for (int i = 0; i < 3; i++)
                              {
                                  var docid = new Random().Next(1, temp.Max(dd => dd.DoctorID) + 1);
                                  while (!temp.Exists(d => d.DoctorID == docid) && !tempList.Exists(d => d.DoctorID == docid))
                                      docid = new Random().Next(1, temp.Max(dd => dd.DoctorID) + 1);
                                  tempList.Add(temp.FirstOrDefault(d => d.DoctorID == docid));
                              }
                              tempList.ForEach(d => { d.WeeklyHours += 8; d.WorkingDays++; d.LastWorkDay = s.StartDate; docslist.Add(d); doctorsInShifts.Add(new DoctorShifts() { Doctor = d, DoctorID = d.DoctorID, Shift = s, ShiftID = s.ShiftID }); });
                          }

                          if (docslist.Count() == 3)
                          {
                              _context.Doctor.UpdateRange(docslist);
                              _context.SaveChanges();
                              docslist.Clear();
                          }
                      }
                      else if (s.Name.Contains("Noite"))
                      {
                          var temp = _context.Doctor.Include(d => d.Shifts).Include(d => d.PreviousShifts).ToList()
                                    .Where(d => d.WeeklyHours < 40 &&
                                               d.WorkingDays < 3 &&
                                                 d.DoesNightShifts &&
                                               d.LastWorkDay.DayOfYear != s.StartDate.DayOfYear).ToList();
                          var tempList = new List<Doctor>();

                          if (temp.Count > 0)
                          {
                              for (int i = 0; i < 3; i++)
                              {
                                  var docid = new Random().Next(1, temp.Max(dd => dd.DoctorID) + 1);
                                  while (!temp.Exists(d => d.DoctorID == docid))
                                      docid = new Random().Next(1, temp.Max(dd => dd.DoctorID) + 1);
                                  tempList.Add(temp.FirstOrDefault(d => d.DoctorID == docid));
                              }
                              tempList.ForEach(d => { d.WeeklyHours += 8; d.WorkingDays++; d.LastWorkDay = s.StartDate; docslist.Add(d); doctorsInShifts.Add(new DoctorShifts() { Doctor = d, DoctorID = d.DoctorID, Shift = s, ShiftID = s.ShiftID }); });
                          }

                          if (docslist.Count() == 3)
                          {
                              _context.Doctor.AddRange(docslist);
                              _context.SaveChanges();
                              docslist.Clear();
                          }
                      }
                  }
              });

            if (doctorsInShifts.Count > 0)
            {
                _context.DoctorShifts.UpdateRange(doctorsInShifts);
                await _context.SaveChangesAsync();
            }

            await _context.DoctorShifts.Include(ds => ds.Shift).Where(ds => ds.Shift.StartDate.Month == DateTime.Now.Month && ds.Shift.StartDate.Year == DateTime.Now.Year).ForEachAsync(ds => shifts.Add(ds.Shift));

            #region Pagination Data initialized
            paging.CurrentPage = 1;
            paging.TotalItems = shifts.Count();
            #endregion

            docslist = null;

            return View(new ShiftsViewModel
            {
                Shifts = shifts,
                Pagination = paging
            });
        }

        private void CleanWeek(List<Doctor> docslist)
        {
            _context.Shift.Where(s => s.StartDate.Month == DateTime.Now.Month).OrderBy(s => s.StartDate).ToList().ForEach(s =>
            {
                if (s.StartDate.DayOfWeek == DayOfWeek.Monday)
                {
                    _context.Doctor.Where(d => d.WeeklyHours == 40).ToList().ForEach(dd => { dd.WeeklyHours = 0; docslist.Add(dd); });
                }

                if (_context.Doctor.Where(d => d.WorkingDays == 3 && s.StartDate.Subtract(d.LastWorkDay).Days > 0).Count() > 0)
                {
                    _context.Doctor.Where(d => d.WorkingDays == 3 && s.StartDate.Subtract(d.LastWorkDay).Days > 0).ToList().ForEach(dd =>
                    {
                        dd.WorkingDays = 0;
                        docslist.Add(dd);
                    });
                }

                if (docslist.Count() > 0)
                {
                    try
                    {
                        _context.Doctor.UpdateRange(docslist);
                        _context.SaveChanges();
                        docslist.Clear();
                    }
                    catch
                    {
                    }
                }
            });
        }

        // GET: DoctorShifts
        // int lazy = 1 so i dont have to rename Indexes Get or Indexes Post
        public async Task<IActionResult> Index(string search = "", string filter = "", string order = "", string asc = "", int page = 1)
        {
            #region Deactivate and Remove from list of old shifts from index and activate new ones if its a new day
            var sh = _context.Shift;
            var list = new List<Shift>();
            await sh.ForEachAsync(s =>
            {
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
            #region Variable to obtain doctorshifts including thier specialities that skips 5 * number of items per page
            var applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).Where(ds => ds.Shift.Active && !ds.Shift.Ended && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).Skip(paging.PageSize * (page - 1))
                        .Take(paging.PageSize).ToListAsync();
            if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(ds => order.Equals("Doctor.Name") ? ds.Doctor.Name : ds.Shift.Name).Where(ds => ds.Shift.Active && !ds.Shift.Ended && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).Skip(paging.PageSize * (page - 1))
                        .Take(paging.PageSize).ToListAsync();
            else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(ds => order.Equals("Doctor.Name") ? ds.Doctor.Name : ds.Shift.Name).Where(ds => ds.Shift.Active && !ds.Shift.Ended && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).Skip(paging.PageSize * (page - 1))
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
                            applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(ds => order.Equals("Doctor.Name") ? ds.Doctor.Name : ds.Shift.Name).Where(ds => (ds.Doctor.Name.Contains(search) || ds.Shift.Name.Contains(search)) && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).ToListAsync();
                        else
                            applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(ds => order.Equals("Doctor.Name") ? ds.Doctor.Name : ds.Shift.Name).Where(ds => (ds.Doctor.Name.Contains(search) || ds.Shift.Name.Contains(search)) && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).ToListAsync();
                        break;
                    case "Name":
                        if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                            applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(ds => order.Equals("Doctor.Name") ? ds.Doctor.Name : ds.Shift.Name).Where(ds => ds.Doctor.Name.Contains(search) && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).ToListAsync();
                        else
                            applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(ds => order.Equals("Doctor.Name") ? ds.Doctor.Name : ds.Shift.Name).Where(ds => ds.Doctor.Name.Contains(search) && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).ToListAsync();
                        break;
                    case "Shift":
                        if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                            applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(ds => order.Equals("Doctor.Name") ? ds.Doctor.Name : ds.Shift.Name).Where(ds => ds.Shift.Name.Contains(search) && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).ToListAsync();
                        else
                            applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(ds => order.Equals("Doctor.Name") ? ds.Doctor.Name : ds.Shift.Name).Where(ds => ds.Shift.Name.Contains(search) && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).ToListAsync();
                        break;
                }

                count = applicationDbContext.Count();
                applicationDbContext = applicationDbContext.Skip(paging.PageSize * (page - 1))
                        .Take(paging.PageSize).ToList();
                ViewData["Filter"] = filter;
                ViewData["Search"] = search;
            }
            #endregion

            #region Pagination Data initialized
            paging.CurrentPage = page;
            paging.TotalItems = string.IsNullOrWhiteSpace(search) ? _context.DoctorShifts.Count() : count;
            #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new DoctorShiftsViewModel { DoctorShifts = applicationDbContext, Pagination = paging });
        }

        // Post: DoctorShifts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string search, string filter, string order, string asc = "", int page = 1, int lazy = 1)
        {
            order = (string.IsNullOrEmpty(order)) ? "" : order;
            #region Deactivate and Remove from list of old shifts from index and activate new ones if its a new day
            var sh = _context.Shift;
            var list = new List<Shift>();
            await sh.ForEachAsync(s =>
            {
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
            #region Variable to obtain doctorshifts including thier specialities that skips 5 * number of items per page
            var applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).Where(ds => ds.Shift.Active && !ds.Shift.Ended && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).Skip(paging.PageSize * (page - 1))
                        .Take(paging.PageSize).ToListAsync();
            if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(ds => order).Where(ds => ds.Shift.Active && !ds.Shift.Ended && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).Skip(paging.PageSize * (page - 1))
                        .Take(paging.PageSize).ToListAsync();
            else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(ds => order).Where(ds => ds.Shift.Active && !ds.Shift.Ended && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).Skip(paging.PageSize * (page - 1))
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
                            applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(ds => order).Where(ds => (ds.Doctor.Name.Contains(search) || ds.Shift.Name.Contains(search)) && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).ToListAsync();
                        else
                            applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(ds => order).Where(ds => (ds.Doctor.Name.Contains(search) || ds.Shift.Name.Contains(search)) && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).ToListAsync();
                        break;
                    case "Name":
                        if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                            applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(ds => order).Where(ds => ds.Doctor.Name.Contains(search) && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).ToListAsync();
                        else
                            applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(ds => order).Where(ds => ds.Doctor.Name.Contains(search) && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).ToListAsync();
                        break;
                    case "Shift":
                        if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                            applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderByDescending(ds => order).Where(ds => ds.Shift.Name.Contains(search) && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).ToListAsync();
                        else
                            applicationDbContext = await _context.DoctorShifts.Include(d => d.Doctor).Include(d => d.Shift).OrderBy(ds => order).Where(ds => ds.Shift.Name.Contains(search) && ds.Shift.StartDate.Year == DateTime.Now.Year && ds.Shift.StartDate.Month == DateTime.Now.Month).ToListAsync();
                        break;
                }

                count = applicationDbContext.Count();
                applicationDbContext = applicationDbContext.Skip(paging.PageSize * (page - 1))
                        .Take(paging.PageSize).ToList();
                ViewData["Filter"] = filter;
                ViewData["Search"] = search;
            }
            #endregion

            #region Pagination Data initialized
            paging.CurrentPage = page;
            paging.TotalItems = string.IsNullOrWhiteSpace(search) ? _context.DoctorShifts.Count() : count;
            #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

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
            ViewData["ShiftID"] = new SelectList(_context.Shift.Where(s => (s.Active && !s.Ended || !s.Active && !s.Ended) && s.StartDate.Year == DateTime.Now.Year && s.StartDate.Month == DateTime.Now.Month).OrderBy(s => s.StartDate), "ShiftID", "Name");
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
            ViewData["ShiftID"] = new SelectList(_context.Shift.Where(s => s.Active && !s.Ended || !s.Active && !s.Ended), "ShiftID", "Name", doctorShifts.ShiftID);
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
