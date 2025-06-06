using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Workout.Web.Models;
using Workout.Web.Filters;

namespace Workout.Web.Controllers
{
    public class WorkoutTypeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<WorkoutTypeController> _logger;
        private readonly IConfiguration _configuration;
        private string apiUrl;

        public WorkoutTypeController(IHttpClientFactory clientFactory, ILogger<WorkoutTypeController> logger, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _configuration = configuration;
            
            var baseUrl = _configuration["ApiSettings:BaseUrl"] ?? "http://172.30.241.79:5261";
            apiUrl = $"{baseUrl}/api/WorkoutType";
        }

        // GET: WorkoutType
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var content = await response.Content.ReadAsStringAsync();
                var workoutTypes = JsonSerializer.Deserialize<List<WorkoutTypeModel>>(content, options);
                return View(workoutTypes);
            }
            else
            {
                _logger.LogError($"Error fetching workout types: {response.StatusCode}");
                return View(new List<WorkoutTypeModel>());
            }
        }

        // GET: WorkoutType/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{apiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var content = await response.Content.ReadAsStringAsync();
                var workoutType = JsonSerializer.Deserialize<WorkoutTypeModel>(content, options);
                return View(workoutType);
            }
            else
            {
                _logger.LogError($"Error fetching workout type details: {response.StatusCode}");
                return NotFound();
            }
        }

        // GET: WorkoutType/Create
        [AuthorizeUser]
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkoutType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public async Task<IActionResult> Create(WorkoutTypeModel workoutType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _clientFactory.CreateClient();
                    var jsonContent = JsonSerializer.Serialize(workoutType);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    
                    // Log the request payload for debugging
                    _logger.LogInformation($"Sending to API: {jsonContent}");
                    
                    var response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Error creating workout type: {response.StatusCode}, Details: {responseContent}");
                        ViewBag.ErrorMessage = $"Error creating workout type: {response.StatusCode}. {responseContent}";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception creating workout type: {ex.Message}");
                    ViewBag.ErrorMessage = $"Exception: {ex.Message}";
                }
            }
            else
            {
                // Log validation errors
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        _logger.LogError($"Validation error for {state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
            }
            return View(workoutType);
        }

        // GET: WorkoutType/Edit/5
        [AuthorizeUser]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{apiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var content = await response.Content.ReadAsStringAsync();
                var workoutType = JsonSerializer.Deserialize<WorkoutTypeModel>(content, options);
                return View(workoutType);
            }
            else
            {
                _logger.LogError($"Error fetching workout type for edit: {response.StatusCode}");
                return NotFound();
            }
        }

        // POST: WorkoutType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public async Task<IActionResult> Edit(int id, WorkoutTypeModel workoutType)
        {
            if (id != workoutType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(workoutType), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{apiUrl}/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogError($"Error updating workout type: {response.StatusCode}");
                }
            }
            return View(workoutType);
        }

        // GET: WorkoutType/Delete/5
        [AuthorizeUser]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{apiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var content = await response.Content.ReadAsStringAsync();
                var workoutType = JsonSerializer.Deserialize<WorkoutTypeModel>(content, options);
                return View(workoutType);
            }
            else
            {
                _logger.LogError($"Error fetching workout type for delete: {response.StatusCode}");
                return NotFound();
            }
        }

        // POST: WorkoutType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.DeleteAsync($"{apiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError($"Error deleting workout type: {response.StatusCode}");
                return RedirectToAction(nameof(Index));
            }
        }
    }
} 