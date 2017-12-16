namespace EventCalendarManager.Controllers
{
    using EventCalendarManager.Models;
    using EventCalendarManager.Models.Context;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountController : Controller
    {
        private readonly EventCalendarDb db;

        public AccountController()
        {
            db = new EventCalendarDb();
        }

        // Logged User Main Page
        public IActionResult Manager(int id)
        {
            if (!IsValidUser(id) || IsLoggedUser(id))
            {
                return RedirectToAction("../../User/Login");
            }

            var user = db
                .Users
                .Where(u => u.Id == id)
                .Include(u => u.Events)
                .FirstOrDefault();

            return View(user);
        }

        // Delete an event
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

            return RedirectToAction($"Manager/{user.Id}");
        }

        // GET Create new event
        public IActionResult Event(int id)
        {
            if (!IsValidUser(id) || IsLoggedUser(id))
            {
                return RedirectToAction("../../User/Login");
            }

            var user = db
                .Users
                .Where(u => u.Id == id)
                .FirstOrDefault();

            return View(user);
        }

        // POST Create new event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Event([Bind("Title,Date,UserId")] Event newEvent)
        {
            var user = db
                .Users
                .Where(u => u.Id == newEvent.UserId)
                .FirstOrDefault();

            if (ModelState.IsValid)
            {
                db.Add(newEvent);
                await db.SaveChangesAsync();
                return RedirectToAction($"Manager/{user.Id}");
            }

            return View(user);
        }

        private bool IsValidUser(int id)
        {
            return db.Users.Any(u => u.Id == id);
        }

        private bool IsLoggedUser(int id)
        {
            return db
                            .Users
                            .Where(u => u.Id == id)
                            .FirstOrDefault()
                            .IsLogged == false;
        }
    }
}