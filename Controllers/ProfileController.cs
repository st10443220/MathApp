using MathApp.Data;
using MathApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MathApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly MathAppContext _context;

        public ProfileController(MathAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Profile")]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("currentUser");
            var email = HttpContext.Session.GetString("currentUserEmail");

            if (token == null)
                return RedirectToAction("Login", "Auth");

            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u =>
                u.FirebaseUuid == token
            );

            var viewModel = new ProfileViewModel
            {
                Email = email,
                Username = userProfile?.Username ?? "",
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            var token = HttpContext.Session.GetString("currentUser");
            var email = HttpContext.Session.GetString("currentUserEmail");

            if (token == null)
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u =>
                    u.FirebaseUuid == token
                );

                if (userProfile == null)
                {
                    userProfile = new UserProfile
                    {
                        FirebaseUuid = token,
                        Username = model.Username,
                    };
                    _context.UserProfiles.Add(userProfile);
                }
                else
                {
                    userProfile.Username = model.Username;
                    HttpContext.Session.SetString("currentUserName", userProfile.Username);
                    _context.UserProfiles.Update(userProfile);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            model.Email = email;
            return View("Profile", model);
        }
    }
}
