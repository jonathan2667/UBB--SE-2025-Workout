using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Workout.Core.IServices;
using Workout.Core.Models;
using ServerLibraryProject.Models;
using ServerMVCProject.Models;
using System.Diagnostics;
using Workout.Web.Models;

namespace Workout.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        /*
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
        }*/


        // Meal Planner Part
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return this.View(new AuthenticationModel());
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return this.View(new AuthenticationModel());
        }

        [HttpPost("Register")]
        public IActionResult Register([FromForm] AuthenticationModel user)
        {
            try
            {
                if (user.Username != null && user.Password != null)
                {
                    long result = this._userService.AddUser(user.Username, user.Password, "");

                    this.HttpContext.Session.SetString("UserId", result.ToString());

                    // at this point, the register is successful
                    // here you redirect to Body Metrics page (for registering new user)
                    return this.RedirectToAction("Index", "ViewPosts");
                }
                else
                {
                    return this.View("Error", new ErrorViewModel { ErrorMessage = "Username and Password cannot be null" });
                }
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    ErrorMessage = ex.Message,
                };
                return this.View("Error", errorModel);
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromForm] AuthenticationModel user)
        {
            try
            {
                if (user.Username != null && user.Password != null)
                {
                    try
                    {

                        User findUser = this._userService.GetUserByUsername(user.Username);
                        if (findUser.Password.Equals(user.Password))
                        {
                            this.HttpContext.Session.SetString("UserId", findUser.Id.ToString());

                        }
                        else
                        {
                            throw new Exception("Password is incorrect");

                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("User doesn't exist");

                    }




                    // at this point, the register is successful
                    // here you redirect to Main page (Dashboard)
                    return this.RedirectToAction("Index", "ViewPosts");
                }
                else
                {
                    return this.View("Error", new ErrorViewModel { ErrorMessage = "Username and Password cannot be null" });
                }
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    ErrorMessage = ex.Message,
                };

                return this.View("Error", errorModel);
            }
        }

        [HttpGet("Follow")]
        public IActionResult Follow(string search = "")
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            long userId = long.Parse(userIdStr);
            var allUsers = _userService.GetAllUsers()
                .Where(u => u.Id != userId &&
                            u.Username.Contains(search ?? "", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var following = _userService.GetUserFollowing(userId).Select(u => u.Id).ToHashSet();

            ViewData["Following"] = following;
            ViewData["CurrentUserId"] = userId;
            ViewData["Search"] = search;

            return View(allUsers);
        }

        [HttpPost("FollowToggle/{targetId}")]
        public IActionResult FollowToggle(long targetId)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            long userId = long.Parse(userIdStr);
            var following = _userService.GetUserFollowing(userId);

            bool isFollowing = following.Any(u => u.Id == targetId);
            try
            {
                if (isFollowing)
                    _userService.UnfollowUserById(userId, targetId);
                else
                    _userService.FollowUserById(userId, targetId);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = ex.Message
                });
            }
            return RedirectToAction("Follow", new { search = Request.Query["search"].ToString() });
        }

    }


} 