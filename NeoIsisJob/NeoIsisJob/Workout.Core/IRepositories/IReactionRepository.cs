namespace Workout.Core.IRepositories
{
    using System.Collections.Generic;
    using Workout.Core.Enums;
    using Workout.Core.Models;

    /// <summary>
    /// Interface for managing reactions in the repository.
    /// </summary>
    public interface IReactionRepository
    {
        /// <summary>
        /// Deletes a reaction by a specific user for a specific post.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="postId">The ID of the post.</param>
        void Delete(int userId, long postId);

        /// <summary>
        /// Retrieves all reactions.
        /// </summary>
        /// <returns>A list of all reactions.</returns>
        //List<Reaction> GetAllReactions();

        /// <summary>
        /// Retrieves all reactions for a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>A list of reactions for the specified post.</returns>
        List<Reaction> GetReactionsByPostId(long postId);

        /// <summary>
        /// Retrieves a reaction by a specific user for a specific post.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>The reaction for the specified user and post.</returns>
        Reaction? GetReaction(int userId, long postId);

        /// <summary>
        /// Saves a new reaction to the repository.
        /// </summary>
        /// <param name="entity">The reaction entity to save.</param>
        void Add(Reaction entity);

        /// <summary>
        /// Updates the reaction type for a specific user and post.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="postId">The ID of the post.</param>
        /// <param name="type">The new reaction type.</param>
        void Update(int userId, long postId, ReactionType type);
    }
}