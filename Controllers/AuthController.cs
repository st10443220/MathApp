using Firebase.Auth;
using MathApp.Data;
using MathApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MathApp
{
    public class AuthController : Controller
    {
        FirebaseAuthProvider auth;
        private readonly MathAppContext _context;

        public AuthController(MathAppContext context)
        {
            auth = new FirebaseAuthProvider(
                new FirebaseConfig(Environment.GetEnvironmentVariable("FirebaseMathApp"))
            );
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("currentUser") != null)
            {
                return RedirectToAction("Calculate", "Math");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginModel login)
        {
            try
            {
                await auth.CreateUserWithEmailAndPasswordAsync(login.Email, login.Password);

                var fbAuthLink = await auth.SignInWithEmailAndPasswordAsync(
                    login.Email,
                    login.Password
                );
                string currentUserId = fbAuthLink.User.LocalId;
                string currentUserEmail = fbAuthLink.User.Email;
                string currentUserName = fbAuthLink.User.Email.Trim().Split('@')[0];

                if (currentUserId != null)
                {
                    HttpContext.Session.SetString("currentUser", currentUserId);
                    HttpContext.Session.SetString("currentUserName", currentUserName);
                    HttpContext.Session.SetString("currentUserEmail", currentUserEmail);

                    var existingProfile = _context.UserProfiles.FirstOrDefault(u =>
                        u.FirebaseUuid == currentUserId
                    );

                    if (existingProfile == null)
                    {
                        var newProfile = new UserProfile
                        {
                            FirebaseUuid = currentUserId,
                            Username = currentUserName,
                        };
                        _context.UserProfiles.Add(newProfile);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction("Calculate", "Math");
                }
            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseErrorModel>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(login);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("currentUser") != null)
            {
                return RedirectToAction("Calculate", "Math");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            try
            {
                var fbAuthLink = await auth.SignInWithEmailAndPasswordAsync(
                    login.Email,
                    login.Password
                );
                string currentUserId = fbAuthLink.User.LocalId;
                string currentUserEmail = fbAuthLink.User.Email;
                string fallbackUserName = fbAuthLink.User.Email.Trim().Split('@')[0];

                if (currentUserId != null)
                {
                    HttpContext.Session.SetString("currentUser", currentUserId);
                    HttpContext.Session.SetString("currentUserName", fallbackUserName);
                    HttpContext.Session.SetString("currentUserEmail", currentUserEmail);

                    var userProfile = _context.UserProfiles.FirstOrDefault(u =>
                        u.FirebaseUuid == currentUserId
                    );

                    if (userProfile == null)
                    {
                        userProfile = new UserProfile
                        {
                            FirebaseUuid = currentUserId,
                            Username = fallbackUserName,
                        };
                        _context.UserProfiles.Add(userProfile);
                        await _context.SaveChangesAsync();
                    }

                    HttpContext.Session.SetString(
                        "currentUserName",
                        userProfile.Username != null ? userProfile.Username : fallbackUserName
                    );

                    return RedirectToAction("Calculate", "Math");
                }
            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseErrorModel>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);

                Utils.AuthLogger.Instance.LogError(
                    firebaseEx.error.message
                        + " - User: "
                        + login.Email
                        + " - IP: "
                        + HttpContext.Connection.RemoteIpAddress
                        + " - Browser: "
                        + Request.Headers.UserAgent
                );

                return View(login);
            }

            return View();
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("currentUser");
            HttpContext.Session.Remove("currentUserName");
            HttpContext.Session.Remove("currentUserEmail");

            return RedirectToAction("Login");
        }
    }
}
