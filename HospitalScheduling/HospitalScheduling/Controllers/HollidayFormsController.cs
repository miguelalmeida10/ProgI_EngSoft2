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
    public class HollidayFormsController : Controller
    {
        private readonly ApplicationDbContext _context;
        PagingViewModel paging = new PagingViewModel()
        {
            CurrentPage = 1
        };

        public HollidayFormsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HollidayForms
        // int lazy = 1 so i dont have to rename Indexes Get or Indexes Post
        public async Task<IActionResult> Index(string search = "", string filter = "", string order = "", string asc = "", int page = 1, int lazy = 1)
        {
            #region Search, Sort & Pagination Related Region
                int count = 0;
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                    var holi = await _context.HollidayForm.Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                    if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                        holi = await _context.HollidayForm.OrderBy(h => order).Skip(paging.PageSize * (page - 1))
                                        .Take(paging.PageSize).ToListAsync();
                    else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                        holi = await _context.HollidayForm.OrderByDescending(h => order).Skip(paging.PageSize * (page - 1))
                                        .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                if (!string.IsNullOrEmpty(search))
                    {
                        if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                            holi = (await _context.HollidayForm.OrderByDescending(h => order).Where(ds => ds.VacationID.ToString().Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                        else
                            holi = (await _context.HollidayForm.OrderBy(h=>order).Where(ds => ds.VacationID.ToString().Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());                            
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.HollidayForm.Count() : count;
            #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new HollidayFormViewModel { HollidayForms = holi, Pagination = paging });
        }

        // Post: HollidayForms
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string search, string filter, string order = "", string asc = "", int page = 1)
        {
            order = (string.IsNullOrEmpty(order)) ? "" : order;
            #region Search, Sort & Pagination Related Region
            int count = 0;
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                    var holi = await _context.HollidayForm.Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                    if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                        holi = await _context.HollidayForm.OrderBy(h => order).Skip(paging.PageSize * (page - 1))
                                        .Take(paging.PageSize).ToListAsync();
                    else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                        holi = await _context.HollidayForm.OrderByDescending(h => order).Skip(paging.PageSize * (page - 1))
                                        .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                if (!string.IsNullOrEmpty(search))
                    {
                        if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                            holi = (await _context.HollidayForm.OrderByDescending(h => order).Where(ds => ds.VacationID.ToString().Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                        else
                            holi = (await _context.HollidayForm.OrderBy(h=>order).Where(ds => ds.VacationID.ToString().Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());                            
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.HollidayForm.Count() : count;
                #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new HollidayFormViewModel { HollidayForms = holi, Pagination = paging });
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

        // GET: HollidayForms/DeleteConfirmation/5
        public async Task<IActionResult> DeleteConfirmation(int? id)
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
