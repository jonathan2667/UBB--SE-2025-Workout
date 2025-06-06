using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ServerMVCProject.Models;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Web.Models;

namespace Workout.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

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
                    long result = this._userService.AddUserAsync(user.Username, user.Password, "").Result;

                    this.HttpContext.Session.SetString("UserId", result.ToString());

                    // at this point, the register is successful
                    // here you redirect to Body Metrics page (for registering new user)
                    return this.RedirectToAction("Index", "Home");
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

                        UserModel findUser = this._userService.GetUserByUsername(user.Username);
                        if (findUser.Password.Equals(user.Password))
                        {
                            this.HttpContext.Session.SetString("UserId", findUser.ID.ToString());

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
                    return this.RedirectToAction("Index", "Home");
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

        [HttpPost]
        [ValidateAntiForgeryToken] // optional but recommended
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // log out user
            HttpContext.Session.Clear();      // clear session
            return RedirectToAction("Index", "Login");
        }

        [HttpGet("Follow")]
        public IActionResult Follow(string search = "")
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            int userId = int.Parse(userIdStr);
            var allUsers = _userService.GetAllUsersAsync().Result
                .Where(u => u.ID != userId &&
                            u.Username.Contains(search ?? "", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var following = _userService.GetUserFollowing(userId).Select(u => u.ID).ToHashSet();

            ViewData["Following"] = following;
            ViewData["CurrentUserId"] = userId;
            ViewData["Search"] = search;

            return View(allUsers);
        }

        [HttpPost("FollowToggle/{targetId}")]
        public IActionResult FollowToggle(int targetId)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            int userId = int.Parse(userIdStr);
            var following = _userService.GetUserFollowing(userId);

            bool isFollowing = following.Any(u => u.ID == targetId);
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