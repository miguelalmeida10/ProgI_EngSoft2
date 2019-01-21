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
    public class VacationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        PagingViewModel paging = new PagingViewModel()
        {
            CurrentPage = 1
        };

        public VacationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vacations
        // int lazy = 1 so i dont have to rename Indexes Get or Indexes Post
        public async Task<IActionResult> Index(string search = "", string filter = "", string order = "", string asc = "", int page = 1, int lazy = 1)
        {
            order = (string.IsNullOrEmpty(order)) ? ViewData["Order"] as String : order;
            #region Search, Sort & Pagination Related Region
            int count = 0;
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                    var holi = await _context.Vacations.Include(v => v.Doctor).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                    if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                        holi = await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Skip(paging.PageSize * (page - 1))
                                        .Take(paging.PageSize).ToListAsync();
                    else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                        holi = await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Skip(paging.PageSize * (page - 1))
                                        .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                if (!string.IsNullOrEmpty(search) || !string.IsNullOrEmpty(order))
                    {
                        switch (filter)
                        {
                            case "All":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Where(ds => ds.Doctor.Name.ToString().Contains(search) || ds.StartDate.ToString().Contains(search) || ds.EndDate.ToString().Contains(search) || ds.Doctor.VacationDays.ToString().Contains(search))
                                        .ToListAsync());
                                else
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Where(ds => ds.Doctor.Name.ToString().Contains(search) || ds.StartDate.ToString().Contains(search) || ds.EndDate.ToString().Contains(search) || ds.Doctor.VacationDays.ToString().Contains(search))
                                        .ToListAsync());
                                break;
                            case "Doctor":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Where(ds => ds.Doctor.Name.ToString().Contains(search))
                                        .ToListAsync());
                                else
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Where(ds => ds.Doctor.Name.ToString().Contains(search))
                                        .ToListAsync());
                                break;
                            case "StartDate":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Where(ds => ds.StartDate.ToString().Contains(search))
                                        .ToListAsync());
                                else
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Where(ds => ds.StartDate.ToString().Contains(search))
                                        .ToListAsync());
                                break;
                            case "EndDate":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Where(ds => ds.EndDate.ToString().Contains(search))
                                        .ToListAsync());
                                else
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Where(ds => ds.EndDate.ToString().Contains(search))
                                        .ToListAsync());
                                break;
                            case "VacationDays":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Where(ds => ds.Doctor.VacationDays.ToString().Contains(search))
                                        .ToListAsync());
                                else
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Where(ds => ds.Doctor.VacationDays.ToString().Contains(search))
                                        .ToListAsync());
                                break;
                            default:
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Where(ds => ds.Doctor.Name.ToString().Contains(search) || ds.StartDate.ToString().Contains(search) || ds.EndDate.ToString().Contains(search) || ds.Doctor.VacationDays.ToString().Contains(search))
                                        .ToListAsync());
                                else
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Where(ds => ds.Doctor.Name.ToString().Contains(search) || ds.StartDate.ToString().Contains(search) || ds.EndDate.ToString().Contains(search) || ds.Doctor.VacationDays.ToString().Contains(search))
                                        .ToListAsync());
                                break;
                        }

                switch (order)
                        {
                            case "Doctor":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                                    holi = holi.OrderBy(d => d.Doctor.Name).ToList();

                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = holi.OrderByDescending(d => d.Doctor.Name).ToList();
                                break;
                            case "StartDate":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                                    holi = holi.OrderBy(d => d.StartDate).ToList();

                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = holi.OrderByDescending(d => d.StartDate).ToList();
                                break;
                            case "EndDate":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                                    holi = holi.OrderBy(d => d.EndDate).ToList();

                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = holi.OrderByDescending(d => d.EndDate).ToList();
                                break;
                            case "VacationDays":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                                    holi = holi.OrderBy(d => d.Doctor.VacationDays).ToList();

                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = holi.OrderByDescending(d => d.Doctor.VacationDays).ToList();
                                break;
                            default:
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                                    holi = holi.OrderBy(d => d.Doctor.Name).ToList();
                            
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = holi.OrderByDescending(d => d.Doctor.Name).ToList();
                                break;
                        }
                        count = holi.Count();
                        holi = holi.Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToList();
                        ViewData["Search"] = search;
                        ViewData["Filter"] = filter;
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.Vacations.Count() : count;
                #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new VacationsViewModel { Vacations = holi, Pagination = paging });
        }

        // Post: Vacations
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string search, string filter, string order = "", string asc = "", int page = 1)
        {
            order = (string.IsNullOrEmpty(order)) ? ViewData["Order"] as String : order;
            #region Search, Sort & Pagination Related Region
            int count = 0;
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                    var holi = await _context.Vacations.Include(v => v.Doctor).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                    if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                        holi = await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Skip(paging.PageSize * (page - 1))
                                        .Take(paging.PageSize).ToListAsync();
                    else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                        holi = await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Skip(paging.PageSize * (page - 1))
                                        .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                if (!string.IsNullOrEmpty(search) || !string.IsNullOrEmpty(order))
                    {
                        switch (filter)
                        {
                            case "All":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Where(ds => ds.Doctor.Name.ToString().Contains(search) || ds.StartDate.ToString().Contains(search) || ds.EndDate.ToString().Contains(search) || ds.Doctor.VacationDays.ToString().Contains(search))
                                        .ToListAsync());
                                else
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Where(ds => ds.Doctor.Name.ToString().Contains(search) || ds.StartDate.ToString().Contains(search) || ds.EndDate.ToString().Contains(search) || ds.Doctor.VacationDays.ToString().Contains(search))
                                        .ToListAsync());
                                break;
                            case "Doctor":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Where(ds => ds.Doctor.Name.ToString().Contains(search))
                                        .ToListAsync());
                                else
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Where(ds => ds.Doctor.Name.ToString().Contains(search))
                                        .ToListAsync());
                                break;
                            case "StartDate":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Where(ds => ds.StartDate.ToString().Contains(search))
                                        .ToListAsync());
                                else
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Where(ds => ds.StartDate.ToString().Contains(search))
                                        .ToListAsync());
                                break;
                            case "EndDate":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Where(ds => ds.EndDate.ToString().Contains(search))
                                        .ToListAsync());
                                else
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Where(ds => ds.EndDate.ToString().Contains(search))
                                        .ToListAsync());
                                break;
                            case "VacationDays":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Where(ds => ds.Doctor.VacationDays.ToString().Contains(search))
                                        .ToListAsync());
                                else
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Where(ds => ds.Doctor.VacationDays.ToString().Contains(search))
                                        .ToListAsync());
                                break;
                            default:
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderByDescending(h => order).Where(ds => ds.Doctor.Name.ToString().Contains(search) || ds.StartDate.ToString().Contains(search) || ds.EndDate.ToString().Contains(search) || ds.Doctor.VacationDays.ToString().Contains(search))
                                        .ToListAsync());
                                else
                                    holi = (await _context.Vacations.Include(v => v.Doctor).OrderBy(h => order).Where(ds => ds.Doctor.Name.ToString().Contains(search) || ds.StartDate.ToString().Contains(search) || ds.EndDate.ToString().Contains(search) || ds.Doctor.VacationDays.ToString().Contains(search))
                                        .ToListAsync());
                                break;
                        }

                switch (order)
                        {
                            case "Doctor":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                                    holi = holi.OrderBy(d => d.Doctor.Name).ToList();

                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = holi.OrderByDescending(d => d.Doctor.Name).ToList();
                                break;
                            case "StartDate":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                                    holi = holi.OrderBy(d => d.StartDate).ToList();

                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = holi.OrderByDescending(d => d.StartDate).ToList();
                                break;
                            case "EndDate":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                                    holi = holi.OrderBy(d => d.EndDate).ToList();

                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = holi.OrderByDescending(d => d.EndDate).ToList();
                                break;
                            case "VacationDays":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                                    holi = holi.OrderBy(d => d.Doctor.VacationDays).ToList();

                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = holi.OrderByDescending(d => d.Doctor.VacationDays).ToList();
                                break;
                            default:
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                                    holi = holi.OrderBy(d => d.Doctor.Name).ToList();
                            
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    holi = holi.OrderByDescending(d => d.Doctor.Name).ToList();
                                break;
                        }
                        count = holi.Count();
                        holi = holi.Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToList();
                        ViewData["Search"] = search;
                        ViewData["Filter"] = filter;
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.Vacations.Count() : count;
                #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new VacationsViewModel { Vacations = holi, Pagination = paging });
        }

        // GET: Vacations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Vacations = await _context.Vacations.Include(v => v.Doctor)
                .FirstOrDefaultAsync(m => m.VacationID == id);
            if (Vacations == null)
            {
                return NotFound();
            }

            return View(Vacations);
        }

        // GET: Vacations/Create
        public IActionResult Create()
        {
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Name", _context.Doctor.FirstOrDefault().DoctorID);
            return View();
        }

        // POST: Vacations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VacationID,DoctorID,StartDate,EndDate")] Vacations Vacations)
        {
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Name", _context.Doctor.FirstOrDefault().DoctorID);

            if (ModelState.IsValid)
            {
                if (_context.Vacations.Where(v => v.DoctorID == Vacations.DoctorID).Count() != 0)
                {
                    return View(Vacations);
                }
                var doctor= _context.Doctor.Where(d => d.DoctorID == Vacations.DoctorID).FirstOrDefault();
                // Used doctor to calculate and update remaining vacation days, example: doctor.VacationDays-=Vacation.StartDate-Vacation.EndDate
                doctor.VacationDays-=DateTime.Parse(Vacations.EndDate).Subtract(DateTime.Parse(Vacations.EndDate)).Days;
                doctor.Vacations = Vacations;
                _context.Update(doctor);
                _context.Add(Vacations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Vacations);
        }

        // GET: Vacations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Name", _context.Doctor.FirstOrDefault().DoctorID);
            if (id == null)
            {
                return NotFound();
            }

            var Vacations = await _context.Vacations.FindAsync(id);
            if (Vacations == null)
            {
                return NotFound();
            }
            return View(Vacations);
        }

        // POST: Vacations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VacationID,DoctorID,StartDate,EndDate")] Vacations Vacations)
        {
            ViewData["DoctorID"] = new SelectList(_context.Doctor, "DoctorID", "Name", _context.Doctor.FirstOrDefault().DoctorID);
            if (id != Vacations.VacationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var doctor = _context.Doctor.Where(d => d.DoctorID == Vacations.DoctorID).FirstOrDefault();
                    // Used doctor to calculate and update remaining vacation days, example: doctor.VacationDays-=Vacation.StartDate-Vacation.EndDate
                    //doctor.VacationDays -= DateTime.Parse(Vacations.EndDate).Subtract(DateTime.Parse(Vacations.EndDate)).Days;
                    doctor.Vacations = Vacations;
                    _context.Update(doctor);

                    _context.Update(Vacations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacationsExists(Vacations.VacationID))
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
            return View(Vacations);
        }

        // GET: Vacations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Vacations = await _context.Vacations.Include(v=>v.Doctor)
                .FirstOrDefaultAsync(m => m.VacationID == id);
            if (Vacations == null)
            {
                return NotFound();
            }

            return View(Vacations);
        }

        // GET: Vacations/DeleteConfirmation/5
        public async Task<IActionResult> DeleteConfirmation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Vacations = await _context.Vacations
                .FirstOrDefaultAsync(m => m.VacationID == id);
            if (Vacations == null)
            {
                return NotFound();
            }

            return View(Vacations);
        }

        // POST: Vacations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Vacations = await _context.Vacations.FindAsync(id);

            var doctor = _context.Doctor.Where(d => d.DoctorID == Vacations.DoctorID).FirstOrDefault();
            // Used doctor to calculate and update remaining vacation days, example: doctor.VacationDays-=Vacation.StartDate-Vacation.EndDate
            //doctor.VacationDays = calculated days; // see seed data AddVacationDays
            doctor.Vacations = null;
            _context.Update(doctor);
            _context.Vacations.Remove(Vacations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacationsExists(int id)
        {
            return _context.Vacations.Any(e => e.VacationID == id);
        }
    }
}
