using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pagination.Models;
using PagedList.Core;

namespace Pagination.Controllers
{
    public class PagingSearchController : Controller
    {
        private readonly StudentDbContext _context;

        public PagingSearchController(StudentDbContext context)
        {
            _context = context;
        }

        // GET: PagingSearch
        //public IActionResult Index(int page = 1, int pageSize = 6)
        //{
        //    PagedList<Student> model = new PagedList<Student>(_context.Students, page, pageSize);
        //    return View("Index", model);
        //}

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index(int page = 1, int pageSize = 6)
        {
            var StudSearch = Request.Query["StudSearch"].ToString();
            var students = _context.Students.Where(s => s.Name.Contains(StudSearch) || s.Class.Contains(StudSearch));
            PagedList<Student> model = new PagedList<Student>(students, page, pageSize);
            ViewData["GetStudentDetails"] = StudSearch;
            return View("Index", model); 
        }

        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    var StudSearch = Request.Query["StudSearch"].ToString();

        //    var students = _context.Students.Where(s => s.Name.Contains(StudSearch) || s.Class.Contains(StudSearch));
        //    ViewData["GetStudentDetails"] = StudSearch;

        //    return View(await students.AsNoTracking().ToListAsync());

        //}


        //[HttpGet]
        //public async Task<IActionResult> Index(string StudSearch)
        //{
        //    ViewBag.GetStudentDetails = StudSearch;

        //    var studQuery = from x in _context.Students select x;

        //    if(!String.IsNullOrEmpty(StudSearch))
        //    {
        //        studQuery = studQuery.Where(x => x.Name.Contains(StudSearch) || x.Class.Contains(StudSearch));
        //    }
        //    return View(await studQuery.AsNoTracking().ToListAsync());

        //}

        // GET: PagingSearch/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: PagingSearch/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PagingSearch/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,RollNo,Class")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: PagingSearch/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: PagingSearch/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,RollNo,Class")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }

        // GET: PagingSearch/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: PagingSearch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
