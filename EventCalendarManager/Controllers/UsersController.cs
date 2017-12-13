namespace EventCalendarManager.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using EventCalendarManager.Models;
    using EventCalendarManager.Models.Context;

    public class UsersController : Controller
    {
        private readonly EventCalendarDb db;

        public UsersController()
        {
            db = new EventCalendarDb();
        }

        // GET: HomePage
        public IActionResult Index()
        {
            CheckForLoggedUsers();

            return View();
        }

        // GET: Users/Register
        public IActionResult Create()
        {
            CheckForLoggedUsers();

            return View();
        }

        // POST: Users/CreatePost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.IsLogged = true;
                db.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction($"CreateEvent/{user.Id}");
            }

            return View(user);
        }

        public IActionResult Login()
        {
            CheckForLoggedUsers();

            return View();
        }

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

            return RedirectToAction($"CreateEvent/{loggedUser.Id}");
        }

        public IActionResult Calendar()
        {
            return View();
        }

        public IActionResult CreateEvent(int id)
        {
            if (db
                .Users
                .Where(u => u.Id == id)
                .FirstOrDefault()
                .IsLogged == false)
            {
                return RedirectToAction("Login");
            }

            if (!db.Users.Any(u => u.Id == id))
            {
                return View("Error");
            }

            var user = db
                .Users
                .Where(u => u.Id == id)
                .Include(u => u.Events)
                .FirstOrDefault();

            return View(user);
        }

        public IActionResult Delete(int? id)
        {
            var eventToRemove = db
                                    .Events
                                    .Where(e => e.Id == id)
                                    .FirstOrDefault();

            var user = db
                .Users
                .Where(u => u.Events.Any(e => e.Id == id))
                .FirstOrDefault();

            db
                .Events
                .Remove(eventToRemove);

            db.SaveChanges();

            return RedirectToAction($"CreateEvent/{user.Id}");
        }

        public IActionResult CreateNewEvent(int? id)
        {
            var user = db
                .Users
                .Where(u => u.Id == id)
                .FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewEvent([Bind("Title,Date,UserId")] Event newEvent)
        {
            var user = db
                .Users
                .Where(u => u.Id == newEvent.UserId)
                .FirstOrDefault();

            if (user.IsLogged == false)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                db.Add(newEvent);
                await db.SaveChangesAsync();
                return RedirectToAction($"CreateEvent/{user.Id}");
            }

            return View(user);
        }

        private bool UserExists(int id)
        {
            return db
                .Users
                .Any(e => e.Id == id);
        }

        private void CheckForLoggedUsers()
        {
            if (db.Users.Where(u => u.IsLogged == true).Count() > 0)
            {
                db
               .Users
               .Where(u => u.IsLogged == true)
               .ToList()
               .ForEach(u => u.IsLogged = false);

                db.SaveChanges();
            }
        }
    }
}
