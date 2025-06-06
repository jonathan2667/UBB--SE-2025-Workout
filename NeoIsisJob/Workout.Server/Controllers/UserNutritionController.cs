// <copyright file="UserNutritionController.cs" company="PlaceholderCompany">
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
    /// API controller for managing user nutrition operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserNutritionController : ControllerBase
    {
        private readonly IUserNutritionService nutritionService;
        private readonly ILogger<UserNutritionController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNutritionController"/> class.
        /// </summary>
        /// <param name="nutritionService">The service for managing user nutrition.</param>
        /// <param name="logger">The logger for logging errors.</param>
        public UserNutritionController(IUserNutritionService nutritionService, ILogger<UserNutritionController> logger)
        {
            this.nutritionService = nutritionService;
            this.logger = logger;
        }

        /// <summary>
        /// Gets daily nutrition for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date (format: yyyy-MM-dd).</param>
        /// <returns>The daily nutrition summary.</returns>
        [HttpGet("{userId}/daily/{date}")]
        public async Task<ActionResult<UserDailyNutritionModel>> GetDailyNutrition(int userId, string date)
        {
            try
            {
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var dateValue))
                {
                    return this.BadRequest("Invalid date format. Use yyyy-MM-dd.");
                }

                var nutrition = await this.nutritionService.GetDailyNutritionAsync(userId, dateValue);
                return this.Ok(nutrition);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving daily nutrition for user {UserId} on {Date}", userId, date);
                return this.StatusCode(500, "An error occurred while retrieving daily nutrition");
            }
        }

        /// <summary>
        /// Gets nutrition data for a user over a specified number of days.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="days">The number of days to retrieve data for.</param>
        /// <returns>A collection of daily nutrition summaries.</returns>
        [HttpGet("{userId}/data/{days}")]
        public async Task<ActionResult<IEnumerable<UserDailyNutritionModel>>> GetNutritionData(int userId, int days)
        {
            try
            {
                var nutritionData = await this.nutritionService.GetNutritionDataAsync(userId, days);
                return this.Ok(nutritionData);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving nutrition data for user {UserId} for {Days} days", userId, days);
                return this.StatusCode(500, "An error occurred while retrieving nutrition data");
            }
        }

        /// <summary>
        /// Logs a meal as consumed by a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="mealId">The meal identifier.</param>
        /// <param name="portionMultiplier">The portion size multiplier.</param>
        /// <param name="notes">Optional notes about the meal consumption.</param>
        /// <returns>The created meal log entry.</returns>
        [HttpPost("{userId}/logmeal")]
        public async Task<ActionResult<UserMealLogModel>> LogMeal(int userId, [FromBody] LogMealRequest request)
        {
            try
            {
                var mealLog = await this.nutritionService.LogMealAsync(userId, request.MealId, request.PortionMultiplier, request.Notes);
                return this.Ok(mealLog);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error logging meal for user {UserId}", userId);
                return this.StatusCode(500, "An error occurred while logging meal");
            }
        }

        /// <summary>
        /// Gets all meals logged by a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date (format: yyyy-MM-dd).</param>
        /// <returns>A collection of meal log entries.</returns>
        [HttpGet("{userId}/meallogs/{date}")]
        public async Task<ActionResult<IEnumerable<UserMealLogModel>>> GetMealLogs(int userId, string date)
        {
            try
            {
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var dateValue))
                {
                    return this.BadRequest("Invalid date format. Use yyyy-MM-dd.");
                }

                var mealLogs = await this.nutritionService.GetMealLogsAsync(userId, dateValue);
                return this.Ok(mealLogs);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving meal logs for user {UserId} on {Date}", userId, date);
                return this.StatusCode(500, "An error occurred while retrieving meal logs");
            }
        }

        /// <summary>
        /// Gets weekly nutrition averages for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="weekStartDate">The start date of the week (format: yyyy-MM-dd).</param>
        /// <returns>The average daily nutrition for the week.</returns>
        [HttpGet("{userId}/weekly/{weekStartDate}")]
        public async Task<ActionResult<UserDailyNutritionModel>> GetWeeklyAverage(int userId, string weekStartDate)
        {
            try
            {
                if (!DateTime.TryParseExact(weekStartDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var dateValue))
                {
                    return this.BadRequest("Invalid date format. Use yyyy-MM-dd.");
                }

                var weeklyAverage = await this.nutritionService.GetWeeklyAverageAsync(userId, dateValue);
                return this.Ok(weeklyAverage);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving weekly average for user {UserId} starting {Date}", userId, weekStartDate);
                return this.StatusCode(500, "An error occurred while retrieving weekly average");
            }
        }

        /// <summary>
        /// Gets monthly nutrition averages for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns>The average daily nutrition for the month.</returns>
        [HttpGet("{userId}/monthly/{year}/{month}")]
        public async Task<ActionResult<UserDailyNutritionModel>> GetMonthlyAverage(int userId, int year, int month)
        {
            try
            {
                var monthlyAverage = await this.nutritionService.GetMonthlyAverageAsync(userId, month, year);
                return this.Ok(monthlyAverage);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving monthly average for user {UserId} for {Month}/{Year}", userId, month, year);
                return this.StatusCode(500, "An error occurred while retrieving monthly average");
            }
        }

        /// <summary>
        /// Gets the top meal types consumed by a user in the last specified days.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="days">The number of days to analyze (default is 30).</param>
        /// <returns>A dictionary of meal types and their consumption counts, ordered by frequency.</returns>
        [HttpGet("{userId}/topmealtypes/{days}")]
        public async Task<ActionResult<Dictionary<string, int>>> GetTopMealTypes(int userId, int days = 30)
        {
            try
            {
                var topMealTypes = await this.nutritionService.GetTopMealTypesAsync(userId, days);
                return this.Ok(topMealTypes);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving top meal types for user {UserId} for {Days} days", userId, days);
                return this.StatusCode(500, "An error occurred while retrieving top meal types");
            }
        }

        /// <summary>
        /// Gets today's nutrition for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Today's nutrition summary.</returns>
        [HttpGet("{userId}/today")]
        public async Task<ActionResult<UserDailyNutritionModel>> GetTodayNutrition(int userId)
        {
            try
            {
                var todayNutrition = await this.nutritionService.GetTodayNutritionAsync(userId);
                return this.Ok(todayNutrition);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving today's nutrition for user {UserId}", userId);
                return this.StatusCode(500, "An error occurred while retrieving today's nutrition");
            }
        }
    }

    /// <summary>
    /// Request model for logging a meal.
    /// </summary>
    public class LogMealRequest
    {
        /// <summary>
        /// Gets or sets the meal identifier.
        /// </summary>
        public int MealId { get; set; }

        /// <summary>
        /// Gets or sets the portion size multiplier.
        /// </summary>
        public double PortionMultiplier { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets optional notes about the meal consumption.
        /// </summary>
        public string Notes { get; set; }
    }
} 