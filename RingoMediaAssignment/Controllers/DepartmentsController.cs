using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RingoMediaAssignment.DAL;
using RingoMediaAssignment.Models;

namespace RingoMediaAssignment.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string _notDeleteMassage = "You can't this department unit its used.";

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Departments.Include(d => d.ParentDepartment);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var department = await _context.Departments
             .Include(d => d.SubDepartments)
              .Include(d => d.ParentDepartment)
             .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            var parentDepartments = new List<Department>();
            var currentDepartment = department;
            while (currentDepartment.ParentDepartment != null)
            {
                parentDepartments.Add(currentDepartment.ParentDepartment);
                currentDepartment = await _context.Departments
                                    .Include(d => d.ParentDepartment)
                                    .FirstOrDefaultAsync(d => d.Id == currentDepartment.ParentDepartment.Id);
            }

            ViewBag.ParentDepartments = parentDepartments;
            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewData["ParentDepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Logo,ParentDepartmentId")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentDepartmentId"] = new SelectList(_context.Departments, "Id", "Name", department.ParentDepartmentId);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            var allDepartments = _context.Departments.ToList();
            ViewData["ParentDepartmentId"] = new SelectList(_context.Departments.Where(x => x.Id != department.Id), "Id", "Name", department.ParentDepartmentId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Logo,ParentDepartmentId")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
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
            ViewData["ParentDepartmentId"] = new SelectList(_context.Departments, "Id", "Name", department.ParentDepartmentId);
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.ParentDepartment)
                .Include(d => d.SubDepartments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            ViewBag.NotDeleteMassage = string.Empty;

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                if (department != null)
                {
                    _context.Departments.Remove(department);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var department = await _context.Departments
                .Include(d => d.ParentDepartment)
                .Include(d => d.SubDepartments)
                .FirstOrDefaultAsync(m => m.Id == id);

                if (department == null)
                {
                    return NotFound();
                }

                ViewBag.NotDeleteMassage = _notDeleteMassage;

                return View(department);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> SelectDepartment(int id)
        {
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
