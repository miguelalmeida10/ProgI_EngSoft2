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
        // int lazy = 1 so i dont have to rename Indexes Get or Indexes Post
        public async Task<IActionResult> Index(string filter="", string search = "", string order = "", string asc="", int page = 1, int lazy = 1)
        {
            #region Search, Sort & Pagination Related Region
                int count = 0;
                var docslist = await _context.Doctor.Include(d => d.Speciality).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Skip(paging.PageSize * (page - 1))
                    .Take(paging.PageSize).ToListAsync();
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                    if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                        docslist = (await _context.Doctor.Include(d => d.Speciality).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                     else if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                        docslist = (await _context.Doctor.Include(d => d.Speciality).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                    if (!string.IsNullOrEmpty(search))
                    {
                        switch (filter)
                        {
                            default:
                            case "All":
                                if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Name.Contains(search) || ds.Email.Contains(search) || ds.Phone.Contains(search) || ds.Speciality.Name.Contains(search)).ToListAsync());
                                else
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Name.Contains(search) || ds.Email.Contains(search) || ds.Phone.Contains(search) || ds.Speciality.Name.Contains(search)).ToListAsync());
                                break;
                            case "Name":
                                if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Name.Contains(search)).ToListAsync());
                                else
                                   docslist = (await _context.Doctor.Include(d => d.Speciality).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Name.Contains(search)).ToListAsync());
                                break;
                            case "Phone":
                                if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Phone.Contains(search)).ToListAsync());
                                else
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Phone.Contains(search)).ToListAsync());
                                break;
                            case "Email":
                                if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Email.Contains(search)).ToListAsync());
                                else
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Email.Contains(search)).ToListAsync());
                                break;
                            case "Speciality":
                                if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Speciality.Name.Contains(search)).ToListAsync());
                                else
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Speciality.Name.Contains(search)).ToListAsync());
                                break;
                        }

                        count = docslist.Count();
                        docslist = docslist.Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToList();
                        
                        ViewData["Search"] = search;
                        ViewData["Filter"] = filter;
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.Doctor.Count() : count;
                #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new DoctorsViewModel { Doctors = docslist, Pagination = paging });
        }

        // Post: Doctors
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string filter, string search, string order, string asc = "", int page = 1)
        {
            order = (string.IsNullOrEmpty(order)) ? "" : order;
            #region Search, Sort & Pagination Related Region
                int count = 0;
                var docslist = await _context.Doctor.Include(d => d.Speciality).OrderBy(d => 
                order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Skip(paging.PageSize * (page - 1))
                    .Take(paging.PageSize).ToListAsync();
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                    if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                        docslist = (await _context.Doctor.Include(d => d.Speciality).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                     else if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                        docslist = (await _context.Doctor.Include(d => d.Speciality).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                    if (!string.IsNullOrEmpty(search))
                    {
                        switch (filter)
                        {
                            default:
                            case "All":
                                if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Name.Contains(search) || ds.Email.Contains(search) || ds.Phone.Contains(search) || ds.Speciality.Name.Contains(search)).ToListAsync());
                                else
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Name.Contains(search) || ds.Email.Contains(search) || ds.Phone.Contains(search) || ds.Speciality.Name.Contains(search)).ToListAsync());
                                break;
                            case "Name":
                                if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Name.Contains(search)).ToListAsync());
                                else
                                   docslist = (await _context.Doctor.Include(d => d.Speciality).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Name.Contains(search)).ToListAsync());
                                break;
                            case "Phone":
                                if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Phone.Contains(search)).ToListAsync());
                                else
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Phone.Contains(search)).ToListAsync());
                                break;
                            case "Email":
                                if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Email.Contains(search)).ToListAsync());
                                else
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Email.Contains(search)).ToListAsync());
                                break;
                            case "Speciality":
                                if(!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderByDescending(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Speciality.Name.Contains(search)).ToListAsync());
                                else
                                    docslist = (await _context.Doctor.Include(d => d.Speciality).OrderBy(d => order.Equals("Name") ? d.Name : order.Equals("Phone") ? d.Phone : order.Equals("Email") ? d.Email : order.Equals("Speciality") ? d.Speciality.Name : d.Name).Where(ds => ds.Speciality.Name.Contains(search)).ToListAsync());
                                break;
                        }

                        count = docslist.Count();
                        docslist = docslist.Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToList();
                        
                        ViewData["Search"] = search;
                        ViewData["Filter"] = filter;
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.Doctor.Count() : count;
                #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

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
        public async Task<IActionResult> Create([Bind("DoctorID,DoctorNumber,Name,Email,CC,Phone,Birthday,WeeklyHours,WorkingDays,DoesNightShifts,LastWorkDay,Address,SpecialityID")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                doctor.Speciality = _context.Speciality.Where(ds => ds.SpecialityID == doctor.SpecialityID).FirstOrDefault();
                _context.Add(doctor);
                _context.DoctorSpecialities.Add(new DoctorSpecialities() { Doctor = doctor, DoctorID = doctor.DoctorID, Speciality = doctor.Speciality, SpecialityID = doctor.Speciality.SpecialityID, Date = DateTime.Now, Type = "Create" });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Speciality"] = new SelectList(_context.Speciality.ToList(), "SpecialityID", "Name", doctor.Speciality != null ? doctor.Speciality : _context.Speciality.FirstOrDefault());
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.Include(d => d.Speciality).Include(d => d.PastSpecialities).Where(d => d.DoctorID == id).FirstOrDefaultAsync();
            if (doctor == null)
            {
                return NotFound();
            }

            ViewData["Speciality"] = new SelectList(_context.Speciality.ToList(), "SpecialityID", "Name", doctor.Speciality != null ? doctor.Speciality : _context.Speciality.FirstOrDefault());
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorID,DoctorNumber,Name,Email,CC,Phone,Birthday,WeeklyHours,WorkingDays,DoesNightShifts,LastWorkDay,Address,SpecialityID,Speciality,PastSpecialities")] Doctor doctor)
        {
            if (id != doctor.DoctorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    doctor.Speciality = _context.Speciality.Where(s => s.SpecialityID == doctor.Speciality.SpecialityID).FirstOrDefault();
                    doctor.SpecialityID = doctor.Speciality.SpecialityID;
                    _context.Update(doctor);

                    if (_context.DoctorSpecialities.Where(d => doctor.DoctorID == id).LastOrDefault() == null || _context.DoctorSpecialities.Where(d=> doctor.DoctorID == id).LastOrDefault().SpecialityID != doctor.SpecialityID) { 
                        _context.DoctorSpecialities.Add(new DoctorSpecialities() { Doctor = doctor, DoctorID = doctor.DoctorID, Speciality = doctor.Speciality, SpecialityID = doctor.SpecialityID, Date = DateTime.Now, Type = "Edit" });
                    }

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

            ViewData["Speciality"] = new SelectList(_context.Speciality.ToList(), "SpecialityID", "Name", doctor.Speciality != null ? doctor.Speciality : _context.Speciality.FirstOrDefault());
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
            var doctor = await _context.Doctor.Include(d => d.Speciality).Include(d => d.PastSpecialities).Where(d => d.DoctorID == id).FirstOrDefaultAsync();
            var docspecialities = await _context.DoctorSpecialities.Where(ds => ds.DoctorID == id).ToListAsync();
            _context.DoctorSpecialities.RemoveRange(docspecialities);
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
