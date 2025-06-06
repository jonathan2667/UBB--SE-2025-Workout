using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Username and password are required";
                return View();
            }

            try
            {
                var result = await _userService.LoginAsync(username, password);
                if (result > 0)
                {
                    // Store user ID in session
                    HttpContext.Session.SetString("UserId", result.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else if (result == -1)
                {
                    ViewBag.ErrorMessage = "Invalid password";
                }
                else // result == -2
                {
                    ViewBag.ErrorMessage = "User not found";
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                ViewBag.ErrorMessage = "An error occurred during login. Please try again.";
                return View();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Username and password are required";
                return View();
            }

            try
            {
                var userId = await _userService.AddUserAsync(username, email, password);
                
                // Log user in automatically
                HttpContext.Session.SetString("UserId", userId.ToString());
                
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                ViewBag.ErrorMessage = "An error occurred during registration. Please try again.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
} 