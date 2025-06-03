// <copyright file="WaterTrackingController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    /// <summary>
    /// API controller for managing water tracking operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WaterTrackingController : ControllerBase
    {
        private readonly IWaterTrackingService waterTrackingService;
        private readonly ILogger<WaterTrackingController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaterTrackingController"/> class.
        /// </summary>
        /// <param name="waterTrackingService">The service for managing water tracking.</param>
        /// <param name="logger">The logger for logging errors.</param>
        public WaterTrackingController(IWaterTrackingService waterTrackingService, ILogger<WaterTrackingController> logger)
        {
            this.waterTrackingService = waterTrackingService;
            this.logger = logger;
        }

        /// <summary>
        /// Gets daily water intake for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date (format: yyyy-MM-dd).</param>
        /// <returns>The daily water intake amount in milliliters.</returns>
        [HttpGet("{userId}/daily/{date}")]
        public async Task<ActionResult<int>> GetDailyWaterIntake(int userId, string date)
        {
            try
            {
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var dateValue))
                {
                    return this.BadRequest("Invalid date format. Use yyyy-MM-dd.");
                }

                var waterIntake = await this.waterTrackingService.GetDailyWaterIntakeAsync(userId, dateValue);
                return this.Ok(waterIntake);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving daily water intake for user {UserId} on {Date}", userId, date);
                return this.StatusCode(500, "An error occurred while retrieving daily water intake");
            }
        }

        /// <summary>
        /// Adds water intake for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The water intake request.</param>
        /// <returns>The created water intake entry.</returns>
        [HttpPost("{userId}/add")]
        public async Task<ActionResult<UserWaterIntakeModel>> AddWaterIntake(int userId, [FromBody] AddWaterIntakeRequest request)
        {
            try
            {
                var waterEntry = await this.waterTrackingService.AddWaterIntakeAsync(userId, request.AmountMl, request.Notes);
                return this.Ok(waterEntry);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error adding water intake for user {UserId}", userId);
                return this.StatusCode(500, "An error occurred while adding water intake");
            }
        }

        /// <summary>
        /// Gets the water goal for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The water goal in milliliters.</returns>
        [HttpGet("{userId}/goal")]
        public async Task<ActionResult<int>> GetWaterGoal(int userId)
        {
            try
            {
                var goal = await this.waterTrackingService.GetWaterGoalAsync(userId);
                return this.Ok(goal);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving water goal for user {UserId}", userId);
                return this.StatusCode(500, "An error occurred while retrieving water goal");
            }
        }

        /// <summary>
        /// Sets the water goal for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The water goal request.</param>
        /// <returns>No content if successful.</returns>
        [HttpPost("{userId}/goal")]
        public async Task<IActionResult> SetWaterGoal(int userId, [FromBody] SetWaterGoalRequest request)
        {
            try
            {
                await this.waterTrackingService.SetWaterGoalAsync(userId, request.GoalMl);
                return this.NoContent();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error setting water goal for user {UserId}", userId);
                return this.StatusCode(500, "An error occurred while setting water goal");
            }
        }

        /// <summary>
        /// Gets water intake progress for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date (format: yyyy-MM-dd).</param>
        /// <returns>The water intake progress as a percentage.</returns>
        [HttpGet("{userId}/progress/{date}")]
        public async Task<ActionResult<double>> GetWaterIntakeProgress(int userId, string date)
        {
            try
            {
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var dateValue))
                {
                    return this.BadRequest("Invalid date format. Use yyyy-MM-dd.");
                }

                var progress = await this.waterTrackingService.GetWaterIntakeProgressAsync(userId, dateValue);
                return this.Ok(progress);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving water intake progress for user {UserId} on {Date}", userId, date);
                return this.StatusCode(500, "An error occurred while retrieving water intake progress");
            }
        }

        /// <summary>
        /// Gets water intake history for a user over a specified number of days.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="days">The number of days to retrieve history for.</param>
        /// <returns>A dictionary of dates and water intake amounts.</returns>
        [HttpGet("{userId}/history/{days}")]
        public async Task<ActionResult<Dictionary<DateTime, int>>> GetWaterIntakeHistory(int userId, int days)
        {
            try
            {
                var history = await this.waterTrackingService.GetWaterIntakeHistoryAsync(userId, days);
                return this.Ok(history);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving water intake history for user {UserId} for {Days} days", userId, days);
                return this.StatusCode(500, "An error occurred while retrieving water intake history");
            }
        }

        /// <summary>
        /// Gets water intake entries for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date (format: yyyy-MM-dd).</param>
        /// <returns>A collection of water intake entries.</returns>
        [HttpGet("{userId}/entries/{date}")]
        public async Task<ActionResult<IEnumerable<UserWaterIntakeModel>>> GetWaterIntakeEntries(int userId, string date)
        {
            try
            {
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var dateValue))
                {
                    return this.BadRequest("Invalid date format. Use yyyy-MM-dd.");
                }

                var entries = await this.waterTrackingService.GetWaterIntakeEntriesAsync(userId, dateValue);
                return this.Ok(entries);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving water intake entries for user {UserId} on {Date}", userId, date);
                return this.StatusCode(500, "An error occurred while retrieving water intake entries");
            }
        }

        /// <summary>
        /// Gets today's water intake for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Today's water intake amount in milliliters.</returns>
        [HttpGet("{userId}/today")]
        public async Task<ActionResult<int>> GetTodayWaterIntake(int userId)
        {
            try
            {
                var todayIntake = await this.waterTrackingService.GetTodayWaterIntakeAsync(userId);
                return this.Ok(todayIntake);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving today's water intake for user {UserId}", userId);
                return this.StatusCode(500, "An error occurred while retrieving today's water intake");
            }
        }
    }

    /// <summary>
    /// Request model for adding water intake.
    /// </summary>
    public class AddWaterIntakeRequest
    {
        /// <summary>
        /// Gets or sets the amount of water in milliliters.
        /// </summary>
        public int AmountMl { get; set; }

        /// <summary>
        /// Gets or sets optional notes about the water intake.
        /// </summary>
        public string Notes { get; set; }
    }

    /// <summary>
    /// Request model for setting water goal.
    /// </summary>
    public class SetWaterGoalRequest
    {
        /// <summary>
        /// Gets or sets the water goal in milliliters.
        /// </summary>
        public int GoalMl { get; set; }
    }
} 