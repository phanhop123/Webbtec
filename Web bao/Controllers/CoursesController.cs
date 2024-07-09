using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Context;
using Web_bao.Data;
using WebQuanLyhs.Helps;
using Web_bao.DTO;

namespace Web_bao.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ConnectDB _context;

        public CoursesController(ConnectDB context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var connectDB = _context.Course.Include(c => c.Category).Include(c => c.User);
            return View(await connectDB.ToListAsync());
        }

        // GET: Courses/Details/5
        

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["Category_Id"] = new SelectList(_context.Category,"Id", "Name" );
            ViewData["User_id"] = new SelectList(
                           _context.Users
                            .Where(user => user.Role == 1)
                            .Select(user => new {
                               Id = user.Id,
                               FullName = user.LastName + " " + user.FistName
                           }),
                           "Id",
                           "FullName"); 

            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Courses course)
        {
            var courses = new Course()
            {
                Name = course.Name,
                Description = course.Description,
                Image = Myunti.UploadHinh(course.Image,"Anhhoc"),
                Category_Id = course.Category_Id,
                User_id = course.User_id
            };
           
            

                _context.Add(courses);
                await _context.SaveChangesAsync();
            return RedirectToAction("Index");
            

        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["Category_Id"] = new SelectList(_context.Category, "Id", "Name", course.Category_Id);
            ViewData["User_id"] = new SelectList(_context.Users, "Id", "LastName", course.User_id);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Image,Category_Id,User_id")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            ViewData["Category_Id"] = new SelectList(_context.Category, "Id", "Id", course.Category_Id);
            ViewData["User_id"] = new SelectList(_context.Users, "Id", "Id", course.User_id);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Category)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'ConnectDB.Course'  is null.");
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
