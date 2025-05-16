using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Workout.Web.Models;

namespace Workout.Web.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<WorkoutController> _logger;
        private string apiUrl = "http://localhost:5261/api/Workout";
        private string workoutTypeApiUrl = "http://localhost:5261/api/WorkoutType";

        public WorkoutController(IHttpClientFactory clientFactory, ILogger<WorkoutController> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        // GET: Workout
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            
            try
            {
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var content = await response.Content.ReadAsStringAsync();
                    var workouts = JsonSerializer.Deserialize<List<WorkoutModel>>(content, options);
                    
                    // Get workout types for display
                    try
                    {
                        var workoutTypesResponse = await client.GetAsync(workoutTypeApiUrl);
                        if (workoutTypesResponse.IsSuccessStatusCode)
                        {
                            var typesContent = await workoutTypesResponse.Content.ReadAsStringAsync();
                            var workoutTypes = JsonSerializer.Deserialize<List<WorkoutTypeModel>>(typesContent, options);
                            
                            // Populate workout type names
                            foreach (var workout in workouts)
                            {
                                var workoutType = workoutTypes.FirstOrDefault(t => t.Id == workout.WorkoutTypeId);
                                if (workoutType != null)
                                {
                                    workout.WorkoutTypeName = workoutType.Name;
                                }
                            }
                            
                            ViewBag.WorkoutTypes = workoutTypes;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error fetching workout types: {ex.Message}");
                    }
                    
                    return View(workouts);
                }
                else
                {
                    _logger.LogError($"Error fetching workouts: {response.StatusCode}");
                    ViewBag.ErrorMessage = $"Error fetching workouts: {response.StatusCode}";
                    return View(new List<WorkoutModel>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Connection Error: {ex.Message}");
                ViewBag.ErrorMessage = "Could not connect to the API server. Please make sure it's running.";
                return View(new List<WorkoutModel>());
            }
        }

        // GET: Workout/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{apiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var content = await response.Content.ReadAsStringAsync();
                var workout = JsonSerializer.Deserialize<WorkoutModel>(content, options);
                
                // Get workout type name
                var workoutTypeResponse = await client.GetAsync($"{workoutTypeApiUrl}/{workout.WorkoutTypeId}");
                if (workoutTypeResponse.IsSuccessStatusCode)
                {
                    var typeContent = await workoutTypeResponse.Content.ReadAsStringAsync();
                    var workoutType = JsonSerializer.Deserialize<WorkoutTypeModel>(typeContent, options);
                    workout.WorkoutTypeName = workoutType.Name;
                }
                
                return View(workout);
            }
            else
            {
                _logger.LogError($"Error fetching workout details: {response.StatusCode}");
                return NotFound();
            }
        }

        // GET: Workout/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync(workoutTypeApiUrl);
                
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var content = await response.Content.ReadAsStringAsync();
                    var workoutTypes = JsonSerializer.Deserialize<List<WorkoutTypeModel>>(content, options);
                    
                    ViewBag.WorkoutTypes = new SelectList(workoutTypes, "Id", "Name");
                }
                else
                {
                    _logger.LogError($"Error fetching workout types: {response.StatusCode}");
                    ViewBag.ErrorMessage = "Could not load workout types. Please make sure the API server is running.";
                }
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Connection Error: {ex.Message}");
                ViewBag.ErrorMessage = "Could not connect to the API server. Please make sure it's running.";
                return View();
            }
        }

        // POST: Workout/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkoutModel workout)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _clientFactory.CreateClient();
                    
                    // First, get available workout types to verify the selected ID is valid
                    var availableWorkoutTypes = new List<WorkoutTypeModel>();
                    try
                    {
                        var typesResponse = await client.GetAsync(workoutTypeApiUrl);
                        if (typesResponse.IsSuccessStatusCode)
                        {
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var typesContent = await typesResponse.Content.ReadAsStringAsync();
                            availableWorkoutTypes = JsonSerializer.Deserialize<List<WorkoutTypeModel>>(typesContent, options);
                            
                            _logger.LogInformation($"Available workout types: {JsonSerializer.Serialize(availableWorkoutTypes.Select(wt => new { wt.Id, wt.Name }))}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error fetching workout types: {ex.Message}");
                    }
                    
                    // Check if the selected workout type exists in available types
                    var selectedWorkoutType = availableWorkoutTypes.FirstOrDefault(wt => wt.Id == workout.WorkoutTypeId);
                    if (selectedWorkoutType == null && availableWorkoutTypes.Any())
                    {
                        // Use the first available workout type if the selected one is invalid
                        _logger.LogWarning($"Selected workout type ID {workout.WorkoutTypeId} not found. Using first available type instead.");
                        selectedWorkoutType = availableWorkoutTypes.First();
                        workout.WorkoutTypeId = selectedWorkoutType.Id;
                        workout.WorkoutTypeName = selectedWorkoutType.Name;
                    }
                    else if (selectedWorkoutType != null)
                    {
                        workout.WorkoutTypeName = selectedWorkoutType.Name;
                    }
                    else
                    {
                        _logger.LogError("No workout types available. Cannot create workout.");
                        ViewBag.ErrorMessage = "No workout types available. Please create a workout type first.";
                        await LoadWorkoutTypes();
                        return View(workout);
                    }
                    
                    if (string.IsNullOrEmpty(workout.Description))
                    {
                        workout.Description = "Default description";
                    }
                    
                    // Approach 1: Try using direct URL method (simplest, no body content)
                    string directUrl = $"{apiUrl}/{Uri.EscapeDataString(workout.Name)}/{workout.WorkoutTypeId}";
                    _logger.LogInformation($"Trying URL endpoint: {directUrl}");
                    
                    var response = await client.PostAsync(directUrl, null);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Successfully created workout using URL endpoint");
                        return RedirectToAction(nameof(Index));
                    }
                    
                    // Approach 2: Try with minimal payload focusing on just essential fields
                    var minimalPayload = new
                    {
                        Name = workout.Name,
                        WTID = workout.WorkoutTypeId
                    };
                    
                    var minimalJson = JsonSerializer.Serialize(minimalPayload);
                    _logger.LogInformation($"Trying minimal JSON: {minimalJson}");
                    
                    var minimalContent = new StringContent(minimalJson, Encoding.UTF8, "application/json");
                    var minimalResponse = await client.PostAsync(apiUrl, minimalContent);
                    
                    if (minimalResponse.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Successfully created workout using minimal JSON");
                        return RedirectToAction(nameof(Index));
                    }
                    
                    // Approach 3: Try with the exact structure from the DB
                    var exactPayload = new
                    {
                        Name = workout.Name,
                        WTID = workout.WorkoutTypeId,
                        Description = workout.Description,
                    };
                    
                    var exactJson = JsonSerializer.Serialize(exactPayload);
                    _logger.LogInformation($"Trying exact DB structure JSON: {exactJson}");
                    
                    var exactContent = new StringContent(exactJson, Encoding.UTF8, "application/json");
                    var exactResponse = await client.PostAsync(apiUrl, exactContent);
                    
                    if (exactResponse.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Successfully created workout using exact DB structure");
                        return RedirectToAction(nameof(Index));
                    }
                    
                    // Approach 4: Try with complete structure including WorkoutType
                    var completePayload = new
                    {
                        Name = workout.Name,
                        WTID = workout.WorkoutTypeId,
                        Description = workout.Description,
                        WorkoutType = new 
                        {
                            WTID = workout.WorkoutTypeId,
                            Name = workout.WorkoutType.Name
                        }
                    };
                    
                    var completeJson = JsonSerializer.Serialize(completePayload);
                    _logger.LogInformation($"Trying complete structure JSON: {completeJson}");
                    
                    var completeContent = new StringContent(completeJson, Encoding.UTF8, "application/json");
                    var completeResponse = await client.PostAsync(apiUrl, completeContent);
                    
                    if (completeResponse.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Successfully created workout using complete structure");
                        return RedirectToAction(nameof(Index));
                    }
                    
                    // Final approach: Try with original Core model structure exactly
                    var coreModelPayload = new Dictionary<string, object>
                    {
                        ["Name"] = workout.Name,
                        ["WTID"] = workout.WorkoutTypeId,
                        ["Description"] = workout.Description,
                        ["WorkoutType"] = new Dictionary<string, object>
                        {
                            ["WTID"] = workout.WorkoutTypeId,
                            ["Name"] = workout.WorkoutType.Name ?? "Default Type"
                        }
                    };
                    
                    var coreModelJson = JsonSerializer.Serialize(coreModelPayload);
                    _logger.LogInformation($"Trying core model structure JSON: {coreModelJson}");
                    
                    var coreModelContent = new StringContent(coreModelJson, Encoding.UTF8, "application/json");
                    var coreModelResponse = await client.PostAsync(apiUrl, coreModelContent);
                    
                    if (coreModelResponse.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Successfully created workout using core model structure");
                        return RedirectToAction(nameof(Index));
                    }
                    
                    // Log all failed attempts
                    var errorResponseContent = await response.Content.ReadAsStringAsync();
                    var minimalErrorContent = await minimalResponse.Content.ReadAsStringAsync();
                    var exactErrorContent = await exactResponse.Content.ReadAsStringAsync();
                    var completeErrorContent = await completeResponse.Content.ReadAsStringAsync();
                    var coreModelErrorContent = await coreModelResponse.Content.ReadAsStringAsync();
                    
                    _logger.LogError($"All approaches failed:\n" +
                        $"URL approach: {response.StatusCode}, {errorResponseContent}\n" +
                        $"Minimal JSON: {minimalResponse.StatusCode}, {minimalErrorContent}\n" +
                        $"Exact JSON: {exactResponse.StatusCode}, {exactErrorContent}\n" +
                        $"Complete JSON: {completeResponse.StatusCode}, {completeErrorContent}\n" +
                        $"Core Model JSON: {coreModelResponse.StatusCode}, {coreModelErrorContent}");
                    
                    ViewBag.ErrorMessage = $"Could not create workout. Status codes: URL={response.StatusCode}, Minimal={minimalResponse.StatusCode}, Exact={exactResponse.StatusCode}, Complete={completeResponse.StatusCode}, CoreModel={coreModelResponse.StatusCode}";
                    ViewBag.RequestContent = $"URL: {directUrl}\nMinimal: {minimalJson}\nExact: {exactJson}\nComplete: {completeJson}\nCoreModel: {coreModelJson}";
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception creating workout: {ex.Message}");
                    ViewBag.ErrorMessage = $"Could not create workout. Error: {ex.Message}";
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
            
            // If we got this far, something failed, reload workout types and redisplay form
            await LoadWorkoutTypes();
            return View(workout);
        }
        
        // Helper method to load workout types into ViewBag
        private async Task LoadWorkoutTypes()
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var typesResponse = await client.GetAsync(workoutTypeApiUrl);
                if (typesResponse.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var typesContent = await typesResponse.Content.ReadAsStringAsync();
                    var workoutTypes = JsonSerializer.Deserialize<List<WorkoutTypeModel>>(typesContent, options);
                    ViewBag.WorkoutTypes = new SelectList(workoutTypes, "Id", "Name");
                }
            }
            catch
            {
                // Ignore errors when loading workout types for redisplay
            }
        }

        // GET: Workout/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{apiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var content = await response.Content.ReadAsStringAsync();
                var workout = JsonSerializer.Deserialize<WorkoutModel>(content, options);
                
                // Get workout types for dropdown
                var workoutTypesResponse = await client.GetAsync(workoutTypeApiUrl);
                if (workoutTypesResponse.IsSuccessStatusCode)
                {
                    var typesContent = await workoutTypesResponse.Content.ReadAsStringAsync();
                    var workoutTypes = JsonSerializer.Deserialize<List<WorkoutTypeModel>>(typesContent, options);
                    ViewBag.WorkoutTypes = new SelectList(workoutTypes, "Id", "Name", workout.WorkoutTypeId);
                }
                
                return View(workout);
            }
            else
            {
                _logger.LogError($"Error fetching workout for edit: {response.StatusCode}");
                return NotFound();
            }
        }

        // POST: Workout/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkoutModel workout)
        {
            if (id != workout.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var client = _clientFactory.CreateClient();
                    
                    // First, get all available workout types
                    var workoutTypes = await GetWorkoutTypesAsync();
                    if (!workoutTypes.Any())
                    {
                        ViewBag.ErrorMessage = "No workout types available in the system";
                        await LoadWorkoutTypes();
                        return View(workout);
                    }
                    
                    // Verify the selected workout type exists
                    int workoutTypeId = workout.WorkoutTypeId;
                    string workoutTypeName = null;
                    var selectedType = workoutTypes.FirstOrDefault(wt => wt.Id == workoutTypeId);
                    
                    if (selectedType != null)
                    {
                        // If found, use it
                        workoutTypeId = selectedType.WTID;
                        workoutTypeName = selectedType.Name;
                        _logger.LogInformation($"Using selected workout type: ID={workoutTypeId}, Name={workoutTypeName}");
                    }
                    else
                    {
                        // If not found, use the first available type
                        var firstType = workoutTypes.First();
                        workoutTypeId = firstType.WTID;
                        workoutTypeName = firstType.Name;
                        _logger.LogWarning($"Selected workout type ID {workout.WorkoutTypeId} not found, using first available: ID={workoutTypeId}, Name={workoutTypeName}");
                    }
                    
                    // Use the delete-recreate approach which is known to work
                    _logger.LogInformation($"Attempting to delete workout with ID {id}");
                    var deleteResponse = await client.DeleteAsync($"{apiUrl}/{id}");
                    
                    if (deleteResponse.IsSuccessStatusCode)
                    {
                        // Then recreate with the new data using the simple URL approach
                        _logger.LogInformation($"Recreating workout with name '{workout.Name}' and verified type ID {workoutTypeId}");
                        string createUrl = $"{apiUrl}/{Uri.EscapeDataString(workout.Name)}/{workoutTypeId}";
                        var createResponse = await client.PostAsync(createUrl, null);
                        
                        if (createResponse.IsSuccessStatusCode)
                        {
                            _logger.LogInformation("Successfully updated workout by recreating it");
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            var createErrorContent = await createResponse.Content.ReadAsStringAsync();
                            _logger.LogError($"Error recreating workout: {createResponse.StatusCode}, {createErrorContent}");
                            ViewBag.ErrorMessage = $"Failed to recreate workout: {createErrorContent}";
                        }
                    }
                    else
                    {
                        var deleteErrorContent = await deleteResponse.Content.ReadAsStringAsync();
                        _logger.LogError($"Error deleting workout: {deleteResponse.StatusCode}, {deleteErrorContent}");
                        ViewBag.ErrorMessage = $"Failed to delete workout: {deleteErrorContent}";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception updating workout: {ex.Message}");
                    ViewBag.ErrorMessage = $"Error updating workout: {ex.Message}";
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
            
            // If we got this far, something failed, redisplay form
            await LoadWorkoutTypes();
            ViewBag.WorkoutTypes = new SelectList(await GetWorkoutTypesAsync(), "Id", "Name", workout.WorkoutTypeId);
            return View(workout);
        }
        
        // Helper method to get workout types
        private async Task<List<WorkoutTypeModel>> GetWorkoutTypesAsync()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync(workoutTypeApiUrl);
            
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<WorkoutTypeModel>>(content, options);
            }
            
            return new List<WorkoutTypeModel>();
        }

        // GET: Workout/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{apiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var content = await response.Content.ReadAsStringAsync();
                var workout = JsonSerializer.Deserialize<WorkoutModel>(content, options);
                
                // Get workout type name
                var workoutTypeResponse = await client.GetAsync($"{workoutTypeApiUrl}/{workout.WorkoutTypeId}");
                if (workoutTypeResponse.IsSuccessStatusCode)
                {
                    var typeContent = await workoutTypeResponse.Content.ReadAsStringAsync();
                    var workoutType = JsonSerializer.Deserialize<WorkoutTypeModel>(typeContent, options);
                    workout.WorkoutTypeName = workoutType.Name;
                }
                
                return View(workout);
            }
            else
            {
                _logger.LogError($"Error fetching workout for delete: {response.StatusCode}");
                return NotFound();
            }
        }

        // POST: Workout/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
                _logger.LogError($"Error deleting workout: {response.StatusCode}");
                return RedirectToAction(nameof(Index));
            }
        }
        
        // GET: Workout/Types
        public async Task<IActionResult> Types()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync(workoutTypeApiUrl);

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

        // POST: Workout/UpdateName
        [HttpPost]
        public async Task<IActionResult> UpdateName(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name cannot be empty");
            }

            try
            {
                var client = _clientFactory.CreateClient();
                
                // First, get all available workout types from the API
                var workoutTypes = await GetWorkoutTypesAsync();
                if (!workoutTypes.Any())
                {
                    _logger.LogError("No workout types available");
                    return StatusCode(500, "No workout types available in the system");
                }
                
                // Get the existing workout data
                var response = await client.GetAsync($"{apiUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    // Get existing workout data
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var content = await response.Content.ReadAsStringAsync();
                    var workout = JsonSerializer.Deserialize<WorkoutModel>(content, options);
                    
                    // Get workout type ID and verify it exists in the database
                    int workoutTypeId = workout.WTID;
                    if (!workoutTypes.Any(wt => wt.WTID == workoutTypeId))
                    {
                        // If the current workout type doesn't exist, use the first available one
                        _logger.LogWarning($"Workout type ID {workoutTypeId} doesn't exist, using the first available type");
                        workoutTypeId = workoutTypes.First().WTID;
                    }
                    
                    // First delete the workout
                    _logger.LogInformation($"Attempting to delete workout with ID {id}");
                    var deleteResponse = await client.DeleteAsync($"{apiUrl}/{id}");
                    
                    if (deleteResponse.IsSuccessStatusCode)
                    {
                        // Then recreate with the new name using the simple URL approach
                        _logger.LogInformation($"Recreating workout with name '{name}' and verified type ID {workoutTypeId}");
                        string createUrl = $"{apiUrl}/{Uri.EscapeDataString(name)}/{workoutTypeId}";
                        var createResponse = await client.PostAsync(createUrl, null);
                        
                        if (createResponse.IsSuccessStatusCode)
                        {
                            _logger.LogInformation("Successfully updated workout name by recreating it");
                            return Ok();
                        }
                        else
                        {
                            var createErrorContent = await createResponse.Content.ReadAsStringAsync();
                            _logger.LogError($"Error recreating workout: {createResponse.StatusCode}, {createErrorContent}");
                            return StatusCode((int)createResponse.StatusCode, $"Failed to recreate workout: {createErrorContent}");
                        }
                    }
                    else
                    {
                        var deleteErrorContent = await deleteResponse.Content.ReadAsStringAsync();
                        _logger.LogError($"Error deleting workout: {deleteResponse.StatusCode}, {deleteErrorContent}");
                        return StatusCode((int)deleteResponse.StatusCode, $"Failed to delete workout: {deleteErrorContent}");
                    }
                }
                else
                {
                    _logger.LogError($"Error fetching workout for name update: {response.StatusCode}");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception updating workout name: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // GET: Workout/GetAll - Returns JSON list of workouts for AJAX calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var client = _clientFactory.CreateClient();
            
            try
            {
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var content = await response.Content.ReadAsStringAsync();
                    var workouts = JsonSerializer.Deserialize<List<WorkoutModel>>(content, options);
                    return Json(workouts);
                }
                else
                {
                    _logger.LogError($"Error fetching workouts: {response.StatusCode}");
                    return Json(new List<WorkoutModel>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Connection Error: {ex.Message}");
                return Json(new List<WorkoutModel>());
            }
        }
    }
} 