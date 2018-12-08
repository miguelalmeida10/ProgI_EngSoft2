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
    public class NursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        PagingViewModel paging = new PagingViewModel()
        {
            CurrentPage = 1
        };

        public NursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Nurses
        public async Task<IActionResult> Index(string search = "", int page = 1,int lazy=1)
        {

            #region Search, Sort & Pagination Related Region
                #region Variable to obtain nurses including thier specialities that skips 5 * number of items per page
                    var nurses = await _context.Nurse.Include(d => d.Speciality).Skip(paging.PageSize * (page - 1))
                                    .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                    if (!string.IsNullOrEmpty(search))
                    {
                        nurses = (await _context.Nurse.Include(d => d.Speciality).Where(ds => ds.Name.Contains(search) || ds.Email.Contains(search) || ds.Phone.Contains(search) || ds.Speciality.Name.Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                        ViewData["Search"] = search;
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = _context.Nurse.Count();
                #endregion
            #endregion

            return View(new NurseViewModel { Nurses = nurses, Pagination = paging });
        }

        // Post: Nurses
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string search, int page = 1)
        {

            #region Search, Sort & Pagination Related Region
                #region Variable to obtain nurses including thier specialities that skips 5 * number of items per page
                    var nurses = await _context.Nurse.Include(d => d.Speciality).Skip(paging.PageSize * (page - 1))
                                    .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                    if (!string.IsNullOrEmpty(search))
                    {
                        nurses = (await _context.Nurse.Include(d => d.Speciality).Where(ds => ds.Name.Contains(search) || ds.Email.Contains(search) || ds.Phone.Contains(search) || ds.Speciality.Name.Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                        ViewData["Search"] = search;
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = _context.Nurse.Count();
            #endregion
            #endregion

            ViewData["Search"] = search;

            return View(new NurseViewModel { Nurses = nurses, Pagination = paging });
        }

        // GET: Nurses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nurse = await _context.Nurse.Include(d => d.Speciality)
                .FirstOrDefaultAsync(m => m.NurseID == id);
            if (nurse == null)
            {
                return NotFound();
            }

            return View(nurse);
        }

        // GET: Nurses/Create
        public IActionResult Create()
        {
            ViewData["Speciality"] = new SelectList(_context.Speciality.ToList(), "SpecialityID", "Name", _context.Speciality.FirstOrDefault());
            return View();
        }

        // POST: Nurses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NurseID,NurseNumber,Name,Email,Phone,Birthday,Address,Sons,SpecialityID,BirthdaySon,CC")] Nurse nurse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nurse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Speciality"] = new SelectList(_context.Speciality.ToList(), "SpecialityID", "Name", _context.Speciality.FirstOrDefault());
            return View(nurse);
        }

        // GET: Nurses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nurse = await _context.Nurse.Include(d => d.Speciality).Where(d => d.NurseID == id).FirstOrDefaultAsync();
            if (nurse == null)
            {
                return NotFound();
            }

            ViewData["Speciality"] = new SelectList(_context.Speciality.ToList(), "SpecialityID", "Name", nurse.SpecialityID);
            return View(nurse);
        }

        // POST: Nurses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NurseID,NurseNumber,Name,Email,Phone,Birthday,BirthdaySon,Address,SpecialityID,Sons,CC")] Nurse nurse)
        {
            if (id != nurse.NurseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nurse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NurseExists(nurse.NurseID))
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
            ViewData["Speciality"] = new SelectList(_context.Speciality.ToList(), "SpecialityID", "Name", nurse.SpecialityID);
            return View(nurse);
        }

        // GET: Nurses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nurse = await _context.Nurse.Include(d => d.Speciality)
                .FirstOrDefaultAsync(m => m.NurseID == id);
            if (nurse == null)
            {
                return NotFound();
            }

            return View(nurse);
        }

        // GET: Nurses/DeleteConfirmation/5
        public async Task<IActionResult> DeleteConfirmation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nurse = await _context.Nurse.Include(d => d.Speciality)
                .FirstOrDefaultAsync(m => m.NurseID == id);
            if (nurse == null)
            {
                return NotFound();
            }

            return View(nurse);
        }

        // POST: Nurses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nurse = await _context.Nurse.Include(d => d.Speciality).Where(d => d.NurseID == id).FirstOrDefaultAsync();
            _context.Nurse.Remove(nurse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NurseExists(int id)
        {
            return _context.Nurse.Any(e => e.NurseID == id);
        }
    }
}
