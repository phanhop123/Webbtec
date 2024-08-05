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

        public async Task<IActionResult> Index()
        {
            var connectDB = _context.Course.Include(c => c.Category).Include(c => c.User);
            return View(await connectDB.ToListAsync());
        }

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

        public ActionResult Edit(int id)
        {
            // Find the course by id
            var course = _context.Course.Find(id);
          

            var editCourse = new Web_bao.DTO.EditCourse
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Url = course.Image,
                Category_Id = course.Category_Id,
                User_id = course.User_id
            };
            ViewData["Category_Id"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["User_id"] = new SelectList(
                           _context.Users
                            .Where(user => user.Role == 1)
                            .Select(user => new {
                                Id = user.Id,
                                FullName = user.LastName + " " + user.FistName
                            }),
                           "Id",
                           "FullName");
            return View(editCourse);
        }
        [HttpPost]
        public ActionResult Edit(EditCourse model)
        {
            var course = _context.Course.FirstOrDefault(u => u.Id == model.Id);
            if (course != null)
            {
                course.Id = model.Id;
                course.Image = Myunti.UploadHinh(model.Image, "Filenopbt");
                course.Name = model.Name;
                course.Description = model.Description;
                course.Category_Id = model.Category_Id;
                course.User_id = model.User_id;

                _context.Entry(course).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return View();
        }

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
