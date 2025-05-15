using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workout.Core.Data;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Web.Controllers
{
    public class UserClassController : Controller
    {
        private readonly IUserClassService _userClassService;
        private readonly IUserService _userService;
        private readonly IClassService _classService;

        public UserClassController(
            IUserClassService userClassService,
            IUserService userService,
            IClassService classService)
        {
            _userClassService = userClassService;
            _userService = userService;
            _classService = classService;
        }

        // GET: UserClass
        public async Task<IActionResult> Index()
        {
            var userClasses = await _userClassService.GetAllUserClassesAsync();
            return View(userClasses);
        }

        // GET: UserClass/Details/5/3/2023-05-20
        public async Task<IActionResult> Details(int userId, int classId, DateTime date)
        {
            var userClass = await _userClassService.GetUserClassByIdAsync(userId, classId, date);
            
            if (userClass == null)
            {
                return NotFound();
            }

            return View(userClass);
        }

        // GET: UserClass/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await _userService.GetAllUsersAsync();
            ViewBag.Classes = await _classService.GetAllClassesAsync();
            return View();
        }

        // POST: UserClass/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UID,CID,Date")] UserClassModel userClass)
        {
            if (ModelState.IsValid)
            {
                await _userClassService.AddUserClassAsync(userClass);
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Users = await _userService.GetAllUsersAsync();
            ViewBag.Classes = await _classService.GetAllClassesAsync();
            return View(userClass);
        }

        // GET: UserClass/Edit/5/3/2023-05-20
        public async Task<IActionResult> Edit(int userId, int classId, DateTime date)
        {
            var userClass = await _userClassService.GetUserClassByIdAsync(userId, classId, date);
            
            if (userClass == null)
            {
                return NotFound();
            }
            
            ViewBag.Users = await _userService.GetAllUsersAsync();
            ViewBag.Classes = await _classService.GetAllClassesAsync();
            return View(userClass);
        }

        // POST: UserClass/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UID,CID,Date")] UserClassModel userClass)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Since the interface doesn't have an update method, we'll delete and re-add
                    await _userClassService.DeleteUserClassAsync(userClass.UID, userClass.CID, userClass.Date);
                    await _userClassService.AddUserClassAsync(userClass);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UserClassExists(userClass.UID, userClass.CID, userClass.Date))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Users = await _userService.GetAllUsersAsync();
            ViewBag.Classes = await _classService.GetAllClassesAsync();
            return View(userClass);
        }

        // GET: UserClass/Delete/5/3/2023-05-20
        public async Task<IActionResult> Delete(int userId, int classId, DateTime date)
        {
            var userClass = await _userClassService.GetUserClassByIdAsync(userId, classId, date);
            
            if (userClass == null)
            {
                return NotFound();
            }

            return View(userClass);
        }

        // POST: UserClass/Delete/5/3/2023-05-20
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userId, int classId, DateTime date)
        {
            await _userClassService.DeleteUserClassAsync(userId, classId, date);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UserClassExists(int userId, int classId, DateTime date)
        {
            var userClass = await _userClassService.GetUserClassByIdAsync(userId, classId, date);
            return userClass != null;
        }
    }
} 