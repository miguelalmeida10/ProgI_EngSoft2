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
    public class SpecialitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        PagingViewModel paging = new PagingViewModel()
        {
            CurrentPage = 1
        };

        public SpecialitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Specialities
        // int lazy = 1 so i dont have to rename Indexes Get or Indexes Post
        public async Task<IActionResult> Index(string search = "",string filter="", string order = "", string asc = "", int page = 1, int lazy = 1)
        {
            #region Search, Sort & Pagination Related Region
                int count = 0;
                #region Variable to obtain nurses including thier specialities that skips 5 * number of items per page
                    var specialities = await _context.Speciality.OrderBy(sp => sp.Name).Skip(paging.PageSize * (page - 1))
                        .Take(paging.PageSize).ToListAsync();
                    if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                        specialities = await _context.Speciality.OrderBy(sp => sp.Name).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync();
                    else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                        specialities = await _context.Speciality.OrderByDescending(sp => sp.Name).Skip(paging.PageSize * (page - 1))
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
                                    specialities = await _context.Speciality.OrderByDescending(sp => sp.Name).Where(ds => ds.Name.Contains(search) || ds.SpecialityID.ToString().Contains(search)).ToListAsync();
                                else
                                    specialities = await _context.Speciality.OrderBy(sp => sp.Name).Where(ds => ds.Name.Contains(search) || ds.SpecialityID.ToString().Contains(search)).ToListAsync();
                                break;
                            case "ID":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    specialities = await _context.Speciality.OrderByDescending(sp => sp.Name).Where(ds => ds.SpecialityID.ToString().Contains(search)).ToListAsync();
                                else
                                    specialities = await _context.Speciality.OrderBy(sp => sp.Name).Where(ds => ds.SpecialityID.ToString().Contains(search)).ToListAsync();
                                break;
                            case "Name":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    specialities = await _context.Speciality.OrderByDescending(sp => sp.Name).Where(ds => ds.Name.Contains(search)).ToListAsync();
                                else
                                    specialities = await _context.Speciality.OrderBy(sp => sp.Name).Where(ds => ds.Name.Contains(search)).ToListAsync();
                                break;                            
                        }
               
                        count = specialities.Count();
                        specialities = specialities.Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToList();
                        ViewData["Search"] = search;
                        ViewData["Filter"] = filter;
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.Speciality.Count() : count;
                #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new SpecialitiesViewModel() { Specialities = specialities, Pagination = paging });
        }

        // Post: Specialities
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string search, string filter, string order = "", string asc = "", int page = 1)
        {
            #region Search, Sort & Pagination Related Region
                int count = 0;
                #region Variable to obtain nurses including thier specialities that skips 5 * number of items per page
                    var specialities = await _context.Speciality.OrderBy(sp => sp.Name).Skip(paging.PageSize * (page - 1))
                        .Take(paging.PageSize).ToListAsync();
                    if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                        specialities = await _context.Speciality.OrderBy(sp => sp.Name).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync();
                    else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                        specialities = await _context.Speciality.OrderByDescending(sp => sp.Name).Skip(paging.PageSize * (page - 1))
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
                                    specialities = await _context.Speciality.OrderByDescending(sp => sp.Name).Where(ds => ds.Name.Contains(search) || ds.SpecialityID.ToString().Contains(search)).ToListAsync();
                                else
                                    specialities = await _context.Speciality.OrderBy(sp => sp.Name).Where(ds => ds.Name.Contains(search) || ds.SpecialityID.ToString().Contains(search)).ToListAsync();
                                break;
                            case "ID":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    specialities = await _context.Speciality.OrderByDescending(sp => sp.Name).Where(ds => ds.SpecialityID.ToString().Contains(search)).ToListAsync();
                                else
                                    specialities = await _context.Speciality.OrderBy(sp => sp.Name).Where(ds => ds.SpecialityID.ToString().Contains(search)).ToListAsync();
                                break;
                            case "Name":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    specialities = await _context.Speciality.OrderByDescending(sp => sp.Name).Where(ds => ds.Name.Contains(search)).ToListAsync();
                                else
                                    specialities = await _context.Speciality.OrderBy(sp => sp.Name).Where(ds => ds.Name.Contains(search)).ToListAsync();
                                break;                            
                        }
               
                        count = specialities.Count();
                        specialities = specialities.Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToList();
                        ViewData["Search"] = search;
                        ViewData["Filter"] = filter;
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.Speciality.Count() : count;
                #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new SpecialitiesViewModel() { Specialities = specialities, Pagination = paging });
        }

        // GET: Specialities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speciality = await _context.Speciality
                .FirstOrDefaultAsync(m => m.SpecialityID == id);
            if (speciality == null)
            {
                return NotFound();
            }

            return View(speciality);
        }

        // GET: Specialities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Specialities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecialityID,Name")] Speciality speciality)
        {
            if (ModelState.IsValid)
            {
                _context.Add(speciality);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(speciality);
        }

        // GET: Specialities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speciality = await _context.Speciality.FindAsync(id);
            if (speciality == null)
            {
                return NotFound();
            }
            return View(speciality);
        }

        // POST: Specialities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpecialityID,Name")] Speciality speciality)
        {
            if (id != speciality.SpecialityID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(speciality);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialityExists(speciality.SpecialityID))
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
            return View(speciality);
        }

        // GET: Specialities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speciality = await _context.Speciality
                .FirstOrDefaultAsync(m => m.SpecialityID == id);
            if (speciality == null)
            {
                return NotFound();
            }

            return View(speciality);
        }

        // GET: Specialities/DeleteConfirmation/5
        public async Task<IActionResult> DeleteConfirmation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speciality = await _context.Speciality
                .FirstOrDefaultAsync(m => m.SpecialityID == id);
            if (speciality == null)
            {
                return NotFound();
            }

            return View(speciality);
        }

        // POST: Specialities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var speciality = await _context.Speciality.FindAsync(id);
            _context.Speciality.Remove(speciality);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialityExists(int id)
        {
            return _context.Speciality.Any(e => e.SpecialityID == id);
        }
    }
}
