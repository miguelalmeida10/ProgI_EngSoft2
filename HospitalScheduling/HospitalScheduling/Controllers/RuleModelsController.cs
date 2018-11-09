using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalScheduling.Data;
using HospitalScheduling.Models;

namespace HospitalScheduling.Controllers
{
    public class RuleModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RuleModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RuleModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.RuleModel.ToListAsync());
        }

        // GET: RuleModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruleModel = await _context.RuleModel
                .FirstOrDefaultAsync(m => m.ValidationID == id);
            if (ruleModel == null)
            {
                return NotFound();
            }

            return View(ruleModel);
        }

        // GET: RuleModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RuleModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ValidationID,Name,category,age,begin,end")] RuleModel ruleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ruleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ruleModel);
        }

        // GET: RuleModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruleModel = await _context.RuleModel.FindAsync(id);
            if (ruleModel == null)
            {
                return NotFound();
            }
            return View(ruleModel);
        }

        // POST: RuleModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ValidationID,Name,category,age,begin,end")] RuleModel ruleModel)
        {
            if (id != ruleModel.ValidationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ruleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RuleModelExists(ruleModel.ValidationID))
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
            return View(ruleModel);
        }

        // GET: RuleModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruleModel = await _context.RuleModel
                .FirstOrDefaultAsync(m => m.ValidationID == id);
            if (ruleModel == null)
            {
                return NotFound();
            }

            return View(ruleModel);
        }

        // POST: RuleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ruleModel = await _context.RuleModel.FindAsync(id);
            _context.RuleModel.Remove(ruleModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RuleModelExists(int id)
        {
            return _context.RuleModel.Any(e => e.ValidationID == id);
        }
    }
}
