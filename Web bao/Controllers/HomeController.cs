using BusinessObject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;
using Web_bao.Data;
using Web_bao.Models;

namespace Web_bao.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly ConnectDB db;

		public HomeController(ConnectDB context, ILogger<HomeController> logger)
		{
			db = context;
			_logger = logger;
		}
      

        public IActionResult Login()
        {
            return View();
        }
		[HttpPost]
		public IActionResult Login(User model)
		{
			var user = db.Users.SingleOrDefault(kh => kh.Email == model.Email && kh.Password == model.Password);

			if (user?.Email != model.Email || user?.Password != model.Password)
			{
				ModelState.AddModelError("", "Invalid email or password.");
				return View();
			}
			if (user == null)
			{
				return View();
			}

			HttpContext.Session.SetString("Email", user.Email);
			HttpContext.Session.SetString("LastName", user.LastName);
			HttpContext.Session.SetString("FistName", user.FistName);
			HttpContext.Session.SetString("Avata", user.Image);
			if (user.Role == 1)
			{
                return RedirectToAction("index");

            }

            return RedirectToAction("Login");


		}
		public IActionResult index()
        {
            var studentCount =  db.Users
                                         .Where(user => user.Role == 2)
                                         .Count();
            var teacherCount = db.Users
                                        .Where(user => user.Role == 1)
                                        .Count();
            var classadmin = db.Class_Admins
                                        .Count();
            // Lưu trữ kết quả vào ViewData
            ViewData["StudentCount"] = studentCount;
            ViewData["TeacherCount"] = teacherCount;
            ViewData["Classadmin"] = classadmin;
            return View();
        }

		public IActionResult Register()
		{
			var major = db.Majors.ToList();
            ViewBag.Majroselect = new SelectList(major, "Id", "Name");

            return View();
		}
        [HttpPost]
        public IActionResult Register(User model)
        {
			var user = new User
			{
				FistName = model.FistName,
				LastName = model.LastName,
				Email = model.Email,
				Password = model.Password,
				Age = model.Age,
				Major = model.Major,
				Major_id = model.Major_id,
				Mobile = model.Mobile,
				Status = model.Status,
				Birthday = model.Birthday,
				City = model.City,
				Role = 2
			};
			db.Add(user);
			db.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult RegisterTeacher()
        {
            var major = db.Majors.ToList();
            ViewBag.Majroselect = new SelectList(major, "Id", "Name");

            return View();
        }
        [HttpPost]
        public IActionResult RegisterTeacher(User model)
        {
            var user = new User
            {
                FistName = model.FistName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                Age = model.Age,
                Major = model.Major,
                Major_id = model.Major_id,
                Mobile = model.Mobile,
                Status = model.Status,
                Birthday = model.Birthday,
                City = model.City,
                Role = 1
            };
            db.Add(user);
            db.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login");
		}
       
		public IActionResult Student()
        {
			var student = db.Users.Include(kh =>kh.Major).
				Where(kh => kh.Role == 2).ToList();
            return View(student);
        }
        public IActionResult Editstudent(int id )
        {

            var student = db.Users
                .Find(id);
            var major = db.Majors.ToList();
            ViewBag.Majroselect = new SelectList(major, "Id", "Name");

            return View(student);
        }
        [HttpPost]
        public IActionResult Editstudent(User model)
        {
            var editstudent = new User
            {
                Id = model.Id,
                FistName = model.FistName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                Age = model.Age,
                Major = model.Major,
                Major_id = model.Major_id,
                Mobile = model.Mobile,
                Status = model.Status,
                Birthday = model.Birthday,
                City = model.City,
                Role = 2

            };
            db.Users.Add(editstudent);
            db.Entry(editstudent).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Student");
        }
        public IActionResult Editteacher(int id)
        {

            var student = db.Users
                .Find(id);
            var major = db.Majors.ToList();
            ViewBag.Majroselect = new SelectList(major, "Id", "Name");

            return View(student);
        }
        [HttpPost]
        public IActionResult Editteacher(User model)
        {
            var editstudent = new User
            {
                Id = model.Id,
                FistName = model.FistName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                Age = model.Age,
                Major = model.Major,
                Major_id = model.Major_id,
                Mobile = model.Mobile,
                Status = model.Status,
                Birthday = model.Birthday,
                City = model.City,
                Role = 1

            };
            db.Users.Add(editstudent);
            db.Entry(editstudent).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Teacher");
        }
        public IActionResult Teacher()
        {
            var teacher = db.Users.Include(kh => kh.Major).
                Where(kh => kh.Role == 1).ToList();
            return View(teacher);
        }

        public async Task<IActionResult> DeleteStudent(int? id)
        {
            if (id == null || db.Users == null)
            {
                return NotFound();
            }

            var major = await db.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (major == null)
            {
                return NotFound();
            }

            return View(major);
        }

        // POST: Majors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Users == null)
            {
                return Problem("Entity set 'ConnectDB.Majors'  is null.");
            }
            var major = await db.Users.FindAsync(id);
            if (major != null)
            {
                db.Users.Remove(major);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MajorExists(int id)
        {
            return (db.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
