using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace ServerAPIProject.Controllers
{
    [ApiController]
    [Route("api/groups")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService groupService;

        public GroupController(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        [HttpGet]
        public ActionResult<List<Group>> GetAllGroups()
        {
            return Ok(groupService.GetAllGroups());
        }

        [HttpGet("{id}")]
        public ActionResult<Group> GetGroupById(int id)
        {
            try
            {
                return Ok(groupService.GetGroupById(id));

            }
            catch (Exception ex)
            {
                return NotFound($"Group with ID {id} not found. Error: {ex.Message}");
            }
        }

        [HttpGet("{id}/users")]
        public ActionResult<List<UserModel>> GetUsersFromGroup(int id)
        {
            return Ok(groupService.GetUsersFromGroup(id));
        }


        [HttpPost]
        public IActionResult SaveGroup([FromBody] Group group)
        {
            try
            {
                var newGroup = groupService.AddGroup(group.Name, group.Description);
                return Ok(newGroup);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}/groups")]
        public ActionResult<List<Group>> GetGroupsForUser(int userId)
        {
            return Ok(groupService.GetUserGroups(userId));
        }


    }
}
