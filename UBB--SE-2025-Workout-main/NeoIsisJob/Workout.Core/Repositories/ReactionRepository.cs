namespace ServerLibraryProject.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using ServerLibraryProject.Data;
    using ServerLibraryProject.Enums;
    using ServerLibraryProject.Interfaces;
    using ServerLibraryProject.Models;


    /// <summary>
    /// Repository for managing reactions.
    /// </summary>
    public class ReactionRepository : IReactionRepository
    {
        private readonly SocialAppDbContext dbContext;

        public ReactionRepository(SocialAppDbContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// Deletes a reaction by a specific user for a specific post.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="postId">The ID of the post.</param>
        public void Delete(int userId, long postId)
        {
            try
            {
                var reactionDeleted = (from reaction in dbContext.Reactions
                                       where reaction.PostId == postId && reaction.UserId == userId
                                       select reaction).FirstOrDefault();
                if (reactionDeleted != null)
                {
                    dbContext.Remove(reactionDeleted);
                    dbContext.SaveChanges();
                }
            }
            catch
            {
                throw new Exception("Error deleting the reaction");
            }

        }

        /// <summary>
        /// Retrieves all reactions.
        /// </summary>
        /// <returns>A list of all reactions.</returns>
        //public List<Reaction> GetAllReactions()
        //{
        //    return this.dbContext.Reactions.ToList();
        //}

        /// <summary>
        /// Retrieves all reactions for a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>A list of reactions for the specified post.</returns>
        public List<Reaction> GetReactionsByPostId(long postId)
        {
            var reactionsQuery = from reaction in dbContext.Reactions
                                 where reaction.PostId == postId
                                 select reaction;

            return reactionsQuery.ToList();
        }

        /// <summary>
        /// Retrieves a reaction by a specific user for a specific post.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>The reaction for the specified user and post.</returns>
        public Reaction GetReaction(int userId, long postId)
        {
            try
            {
                var reactionReturned = (from reaction in dbContext.Reactions
                                        where reaction.PostId == postId && reaction.UserId == userId
                                        select reaction).FirstOrDefault();
                return reactionReturned;
            }
            catch
            {
                throw new Exception("Error retrieving the reaction by user and post");
            }

        }

        /// <summary>
        /// Saves a new reaction to the repository.
        /// </summary>
        /// <param name="entity">The reaction entity to save.</param>
        public void Add(Reaction entity)
        {
            try
            {
                dbContext.Reactions.Add(entity);
                dbContext.SaveChanges();
            }catch
            {
                throw new Exception("Error saving the reaction");
            }
        }

        /// <summary>
        /// Updates the reaction type for a specific user and post.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="postId">The ID of the post.</param>
        /// <param name="type">The new reaction type.</param>
        public void Update(int userId, long postId, ReactionType type)
        {
            try
            {
                var reaction = dbContext.Reactions.FirstOrDefault(r => r.PostId == postId && r.UserId == userId);

                if (reaction != null)
                {
                    reaction.Type = type;
                    dbContext.SaveChanges();
                }
            }
            catch
            {
                throw new Exception("Error updating the reaction type");
            }

        }
    }
}