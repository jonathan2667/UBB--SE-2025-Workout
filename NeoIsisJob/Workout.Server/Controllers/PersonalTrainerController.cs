using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalTrainerController : ControllerBase
    {
        private readonly IPersonalTrainerService personalTrainerService;

        public PersonalTrainerController(IPersonalTrainerService personalTrainerService)
        {
            this.personalTrainerService = personalTrainerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersonalTrainers()
        {
            try
            {
                var personalTrainers = await personalTrainerService.GetAllPersonalTrainersAsync();
                return Ok(personalTrainers);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching personal trainers: {ex.Message}");
            }
        }

        [HttpGet("{personalTrainerId}")]
        public async Task<IActionResult> GetPersonalTrainerById(int personalTrainerId)
        {
            try
            {
                var personalTrainer = await personalTrainerService.GetPersonalTrainerByIdAsync(personalTrainerId);
                return Ok(personalTrainer);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching personal trainer: {ex.Message}");
            }
        }

        [HttpPost("api/personaltrainer")]
        public async Task<IActionResult> AddPersonalTrainer([FromBody] PersonalTrainerModel personalTrainerModel)
        {
            try
            {
                await personalTrainerService.AddPersonalTrainerAsync(personalTrainerModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding personal trainer: {ex.Message}");
            }
        }

        [HttpDelete("api/personaltrainer/{personalTrainerId}")]
        public async Task<IActionResult> DeletePersonalTrainer(int personalTrainerId)
        {
            try
            {
                await personalTrainerService.DeletePersonalTrainerAsync(personalTrainerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting personal trainer: {ex.Message}");
            }
        }
    }
}
