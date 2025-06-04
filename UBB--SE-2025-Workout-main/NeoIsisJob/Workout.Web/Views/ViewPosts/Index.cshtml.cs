using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServerLibraryProject.Interfaces;
using ServerLibraryProject.Enums;
using ServerLibraryProject.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;

public class IndexModel : PageModel
{
    private readonly IReactionRepository _reactionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IndexModel(IReactionRepository reactionRepository, IHttpContextAccessor httpContextAccessor)
    {
        _reactionRepository = reactionRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public void OnGet()
    {
        // Hardcode user ID in session for testing
        // _httpContextAccessor.HttpContext.Session.SetString("UserId", "1");
        // Load posts if needed
    }

    // AJAX handler for reactions (hardcoded user)
    public JsonResult OnPostReactAjax(long postId, string type)
    {
        try
        {
            string userIdStr = HttpContext.Session.GetString("UserId");

            int userId = int.Parse(userIdStr);

            if (!Enum.TryParse<ReactionType>(type, out var reactionType))
                return new JsonResult(new { success = false, error = "Invalid reaction type" });

            var existing = _reactionRepository.GetReaction(userId, postId);

            if (existing == null)
            {
                _reactionRepository.Add(new Reaction { UserId = userId, PostId = postId, Type = reactionType });
            }
            else if (existing.Type == reactionType)
            {
                _reactionRepository.Delete(userId, postId);
            }
            else
            {
                _reactionRepository.Update(userId, postId, reactionType);
            }

            var reactions = _reactionRepository.GetReactionsByPostId(postId);
            return new JsonResult(new
            {
                success = true,
                like = reactions.Count(r => r.Type == ReactionType.Like),
                love = reactions.Count(r => r.Type == ReactionType.Love),
                laugh = reactions.Count(r => r.Type == ReactionType.Laugh),
                anger = reactions.Count(r => r.Type == ReactionType.Anger)
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, error = ex.Message });
        }
    }

    // Fallback for non-AJAX (form post, hardcoded user)
    public IActionResult OnPostToggleReaction(long postId, string type)
    {
        // Return JSON if this is an AJAX request
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return new JsonResult(new { success = false, error = "Invalid AJAX request: handler not matched." });
        }

        int userId = 1; // Hardcoded user ID for testing

        var reactionType = Enum.Parse<ReactionType>(type);
        var existing = _reactionRepository.GetReaction(userId, postId);

        if (existing == null)
            _reactionRepository.Add(new Reaction { UserId = userId, PostId = postId, Type = reactionType });
        else if (existing.Type == reactionType)
            _reactionRepository.Delete(userId, postId);
        else
            _reactionRepository.Update(userId, postId, reactionType);

        return RedirectToPage();
    }
}