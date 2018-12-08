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
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        PagingViewModel paging = new PagingViewModel()
        {
            CurrentPage = 1
        };

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index(string search = "", int page = 1, int lazy = 1)
        {
            #region Search, Sort & Pagination Related Region
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                    var docslist = await _context.Doctor.Include(d => d.Speciality).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                    if (!string.IsNullOrEmpty(search))
                    {
                        docslist = await _context.Doctor.Include(d => d.Speciality).Where(ds => ds.Name.Contains(search) || ds.Email.Contains(search) || ds.Phone.Contains(search) || ds.Speciality.Name.Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = _context.Doctor.Count();
                #endregion
            #endregion

            return View(new DoctorsViewModel { Doctors = docslist, Pagination = paging });
        }

        // Post: Doctors
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string search, int page=1)
        {
            #region Search, Sort & Pagination Related Region
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                    var docslist = await _context.Doctor.Include(d => d.Speciality).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                    if (!string.IsNullOrEmpty(search))
                    {
                        docslist = await _context.Doctor.Include(d => d.Speciality).Where(ds => ds.Name.Contains(search) || ds.Email.Contains(search) || ds.Phone.Contains(search) || ds.Speciality.Name.Contains(search)).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync();
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = _context.Doctor.Count();
            #endregion
            #endregion

            ViewData["Search"] = search;
            return View(new DoctorsViewModel { Doctors = docslist, Pagination = paging });
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.Include(d => d.Speciality)
                .FirstOrDefaultAsync(m => m.DoctorID == id);
            if (doctor == null)
            {
                return NotFound();
            }


            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            ViewData["Speciality"] = new SelectList(_context.Speciality.ToList(), "SpecialityID", "Name", _context.Speciality.FirstOrDefault());
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorID,DoctorNumber,Name,Email,CC,Phone,Birthday,DoesNightShifts,LastWorkDay,WorkingDays,WeeklyHours,Address,SpecialityID")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Speciality"] = new SelectList(_context.Speciality.ToList(), "SpecialityID", "Name", _context.Speciality.FirstOrDefault());
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.Include(d => d.Speciality).Where(d => d.DoctorID == id).FirstOrDefaultAsync();
            if (doctor == null)
            {
                return NotFound();
            }

            ViewData["Speciality"] = new SelectList(_context.Speciality.ToList(), "SpecialityID", "Name", doctor.SpecialityID);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorID,DoctorNumber,Name,Email,CC,Phone,Birthday,DoesNightShifts,LastWorkDay,WorkingDays,WeeklyHours,Address,SpecialityID")] Doctor doctor)
        {
            if (id != doctor.DoctorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.DoctorID))
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

            ViewData["Speciality"] = new SelectList(_context.Speciality.ToList(), "SpecialityID", "Name", doctor.SpecialityID);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.Include(d => d.Speciality)
                .FirstOrDefaultAsync(m => m.DoctorID == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/DeleteConfirmation/5
        public async Task<IActionResult> DeleteConfirmation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.Include(d => d.Speciality)
                .FirstOrDefaultAsync(m => m.DoctorID == id);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctor.Include(d => d.Speciality).Where(d => d.DoctorID == id).FirstOrDefaultAsync();
            _context.Doctor.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctor.Any(e => e.DoctorID == id);
        }
    }
}
