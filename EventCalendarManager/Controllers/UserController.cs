namespace EventCalendarManager.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using EventCalendarManager.Models;
    using EventCalendarManager.Models.Context;

    public class UserController : Controller
    {
        private readonly EventCalendarDb db;

        public UserController()
        {
            db = new EventCalendarDb();
        }

        // GET: Home Page
        public IActionResult Index()
        {
            return View();
        }

        // GET: Register User
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,FirstName,LastName,Email,Password")] User user)
        {
            if (db.Users.Any(u => u.Email == user.Email))
            {
                user.Email = "Incorrect Email";
                return View(user);
            }

            if (ModelState.IsValid)
            {
                user.IsLogged = true;
                db.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction($"../Account/Manager/{user.Id}");
            }

            return View(user);
        }

        // Get Login User
        public IActionResult Login()
        {
            return View();
        }

        // Post Login User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email,Password")] User user)
        {
            var loggedUser = db
                .Users
                .Where(u => u.Email == user.Email && u.Password == user.Password)
                .Include(u => u.Events)
                .FirstOrDefault();

            if (loggedUser == null)
            {
                return View(user);
            }

            loggedUser.IsLogged = true;

            db
                .Users
                .Where(u => u.Id == loggedUser.Id)
                .FirstOrDefault()
                .IsLogged = true;

            db.SaveChanges();

            return RedirectToAction($"../Account/Manager/{loggedUser.Id}");
        }

        // Get Calendar
        public IActionResult Calendar()
        {
            return View();
        }

        private bool UserExists(int id)
        {
            return db
                .Users
                .Any(e => e.Id == id);
        }

        private void CheckForLoggedUsers()
        {
            if (db.Users.Any(u => u.IsLogged == true))
            {
                db
               .Users
               .Where(u => u.IsLogged == true)
               .FirstOrDefault()
               .IsLogged = false;

                db.SaveChanges();
            }
        }
    }
}
