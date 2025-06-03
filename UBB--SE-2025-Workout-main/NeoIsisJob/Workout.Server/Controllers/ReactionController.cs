namespace ServerAPIProject.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ServerLibraryProject.Interfaces;
    using ServerLibraryProject.Models;

    /// <summary>
    /// The controller that manages the reactions.
    /// </summary>
    [ApiController]
    [Route("api/reactions")]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService reactionService;

        public ReactionController(IReactionService reactionService)
        {
            this.reactionService = reactionService;
        }

        //[HttpGet]
        //public ActionResult<List<Reaction>> GetAllReactions()
        //{
        //    return this.reactionService.GetAllReactions();
        //}

        [HttpPost]
        public IActionResult SaveReaction([FromBody] Reaction entity)
        {
            try
            {
                this.reactionService.AddReaction(entity);
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        //[HttpPut("reactions")]
        //public IActionResult UpdateReaction([FromBody] Reaction entity)
        //{
        //    try
        //    {
        //        this.reactionService.UpdateReaction(entity);
        //        return this.Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return this.BadRequest(e.Message);
        //    }
        //}
    }
}