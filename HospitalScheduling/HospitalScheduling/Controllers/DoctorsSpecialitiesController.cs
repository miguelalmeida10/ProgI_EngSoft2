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
    public class DoctorSpecialitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        PagingViewModel paging = new PagingViewModel()
        {
            CurrentPage = 1
        };

        public DoctorSpecialitiesController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult NewNotFound()
        {
            return View("NotFound");
        }


        // GET: DoctorSpecialities
        public async Task<IActionResult> Index(int id = 0,string filter = "", string search = "", string order = "", string asc = "", int page = 1, int lazy = 1)
        {
            #region Search, Sort & Pagination Related Region
                int count = 0;
                var applicationDbContext = await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Skip(paging.PageSize * (page - 1))
                    .Take(paging.PageSize).ToListAsync();
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                    applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync());
                else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                    applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync());
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                if (!string.IsNullOrEmpty(search))
                {
                    switch (filter)
                    {
                        default:
                        case "All":
                            if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search) || ds.Speciality.Name.Contains(search)).ToListAsync());
                            else
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search) || ds.Speciality.Name.Contains(search)).ToListAsync());
                            break;
                        case "DoctorsName":
                            if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search)).ToListAsync());
                            else
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search)).ToListAsync());
                            break;
                        case "SpecialitiesName":
                            if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Speciality.Name.Contains(search)).ToListAsync());
                            else
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Speciality.Name.Contains(search)).ToListAsync());
                            break;
                    }

                    count = applicationDbContext.Count();
                    applicationDbContext = applicationDbContext.Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToList();

                    ViewData["Search"] = search;
                    ViewData["Filter"] = filter;
                }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.DoctorSpecialities.Count() : count;
            #endregion
            #endregion
            
            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new DoctorSpecialitiesViewModel { DoctorSpecialities = id!=0 ? applicationDbContext.Where(s=>s.DoctorID==id) : applicationDbContext,Pagination = paging });
        }

        // Post: DoctorSpecialities
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string filter, string search, int id = 0, string order = "", string asc = "", int page = 1)
        {
            order = (string.IsNullOrEmpty(order)) ? "" : order;
            #region Search, Sort & Pagination Related Region
                int count = 0;
                var applicationDbContext = await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Skip(paging.PageSize * (page - 1))
                    .Take(paging.PageSize).ToListAsync();
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                    applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync());
                else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                    applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync());
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                if (!string.IsNullOrEmpty(search))
                {
                    switch (filter)
                    {
                        default:
                        case "All":
                            if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search) || ds.Speciality.Name.Contains(search)).ToListAsync());
                            else
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search) || ds.Speciality.Name.Contains(search)).ToListAsync());
                            break;
                        case "DoctorsName":
                            if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search)).ToListAsync());
                            else
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search)).ToListAsync());
                            break;
                        case "SpecialitiesName":
                            if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Speciality.Name.Contains(search)).ToListAsync());
                            else
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Speciality.Name.Contains(search)).ToListAsync());
                            break;
                    }

                    count = applicationDbContext.Count();
                    applicationDbContext = applicationDbContext.Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToList();

                    ViewData["Search"] = search;
                    ViewData["Filter"] = filter;
                }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.DoctorSpecialities.Count() : count;
                #endregion
            #endregion
            
            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new DoctorSpecialitiesViewModel { DoctorSpecialities = id != 0 ? applicationDbContext.Where(s => s.DoctorID == id) : applicationDbContext, Pagination = paging });
        }

        // GET: DoctorSpecialities/Details/5
        public async Task<IActionResult> Details(int? id, string filter = "", string search = "", string order = "", string asc = "", int page = 1, int lazy = 1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorsSpeciality = await _context.DoctorSpecialities
                .Include(d => d.Doctor)
                .Include(d => d.Speciality).Include(d => d.Doctor).ToListAsync();

            if (doctorsSpeciality == null || doctorsSpeciality.Where(doc => doc.DoctorID == id).Count() == 0)
            {
                return RedirectToAction("NewNotFound");
            }

            #region Search, Sort & Pagination Related Region
                int count = 0;
                var applicationDbContext = await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Skip(paging.PageSize * (page - 1))
                    .Take(paging.PageSize).ToListAsync();
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                    if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                        applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                    else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                        applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToListAsync());
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                    if (!string.IsNullOrEmpty(search))
                    {
                        switch (filter)
                        {
                            default:
                            case "All":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search) || ds.Speciality.Name.Contains(search)).ToListAsync());
                                else
                                    applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search) || ds.Speciality.Name.Contains(search)).ToListAsync());
                                break;
                            case "DoctorsName":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search)).ToListAsync());
                                else
                                    applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search)).ToListAsync());
                                break;
                            case "SpecialitiesName":
                                if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                    applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Speciality.Name.Contains(search)).ToListAsync());
                                else
                                    applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Speciality.Name.Contains(search)).ToListAsync());
                                break;
                        }

                        count = applicationDbContext.Count();
                        applicationDbContext = applicationDbContext.Skip(paging.PageSize * (page - 1))
                                .Take(paging.PageSize).ToList();

                        ViewData["Search"] = search;
                        ViewData["Filter"] = filter;
                    }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.DoctorSpecialities.Count() : count;
                #endregion
            #endregion

            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new DoctorSpecialitiesViewModel { DoctorSpecialities = id != 0 ? applicationDbContext.Where(s => s.DoctorID == id) : applicationDbContext, Pagination = paging });
        }

        // Post: DoctorSpecialities/Details/5
        [HttpPost]
        public async Task<IActionResult> Details(int? id, string filter = "", string search = "", string order = "", string asc = "", int page = 1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorsSpeciality = await _context.DoctorSpecialities
                .Include(d => d.Doctor)
                .Include(d => d.Speciality).Include(d => d.Doctor).ToListAsync();
               
            if (doctorsSpeciality == null || doctorsSpeciality.Where(doc=> doc.DoctorID==id).Count() == 0 )
            {
                return RedirectToAction("NewNotFound");
            }

            #region Search, Sort & Pagination Related Region
                int count = 0;
                var applicationDbContext = await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Skip(paging.PageSize * (page - 1))
                    .Take(paging.PageSize).ToListAsync();
                #region Variable to obtain doctors including thier specialities that skips 5 * number of items per page
                if (!string.IsNullOrEmpty(asc) && asc.Equals("Asc"))
                    applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync());
                else if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                    applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToListAsync());
                #endregion

                #region If searching gets same list as the one above and filters by fields after ds. and then obtains the pages 5 items if search contains more than 5 items
                if (!string.IsNullOrEmpty(search))
                {
                    switch (filter)
                    {
                        default:
                        case "All":
                            if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search) || ds.Speciality.Name.Contains(search)).ToListAsync());
                            else
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search) || ds.Speciality.Name.Contains(search)).ToListAsync());
                            break;
                        case "DoctorsName":
                            if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search)).ToListAsync());
                            else
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Doctor.Name.Contains(search)).ToListAsync());
                            break;
                        case "SpecialitiesName":
                            if (!string.IsNullOrEmpty(asc) && asc.Equals("Desc"))
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderByDescending(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Speciality.Name.Contains(search)).ToListAsync());
                            else
                                applicationDbContext = (await _context.DoctorSpecialities.Include(d => d.Speciality).Include(d => d.Doctor).OrderBy(d => order.Equals("DoctorsName") ? d.Doctor.Name : order.Equals("SpecialitiesName") ? d.Speciality.Name : order.Equals("Date") ? d.Date.ToString() : order.Equals("Type") ? d.Type : d.Doctor.Name).Where(ds => ds.Speciality.Name.Contains(search)).ToListAsync());
                            break;
                    }

                    count = applicationDbContext.Count();
                    applicationDbContext = applicationDbContext.Skip(paging.PageSize * (page - 1))
                            .Take(paging.PageSize).ToList();

                    ViewData["Search"] = search;
                    ViewData["Filter"] = filter;
                }
                #endregion

                #region Pagination Data initialized
                    paging.CurrentPage = page;
                    paging.TotalItems = (string.IsNullOrEmpty(search)) ? _context.DoctorSpecialities.Count() : count;
                #endregion
            #endregion
            
            ViewData["Order"] = string.IsNullOrEmpty(order) ? ViewData["Order"] : order;
            ViewData["Asc"] = !string.IsNullOrEmpty(asc) ? asc.Equals("Asc") ? "Asc" : "Desc" : "Asc";

            return View(new DoctorSpecialitiesViewModel { DoctorSpecialities = id != 0 ? applicationDbContext.Where(s => s.DoctorID == id) : applicationDbContext, Pagination = paging });
        }

        private bool DoctorsSpecialityExists(int id)
        {
            return _context.DoctorSpecialities.Any(e => e.DoctorSpecialityID == id);
        }
    }
}
