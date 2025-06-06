using Workout.Core.Models;

namespace Workout.Core.IServices
{
    public interface IReactionService
    {
        //List<Reaction> GetAllReactions();

        List<Reaction> GetReactionsByPostId(long postId);

        void AddReaction(Reaction reaction);

        Reaction GetReaction(int userId, long postId);
    }
}