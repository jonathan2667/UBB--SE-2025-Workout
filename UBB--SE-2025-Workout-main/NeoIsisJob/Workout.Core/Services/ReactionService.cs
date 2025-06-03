namespace ServerLibraryProject.Services
{
    using System;
    using System.Collections.Generic;
    using ServerLibraryProject.Models;
    using ServerLibraryProject.Enums;
    using ServerLibraryProject.Interfaces;

    public class ReactionService : IReactionService
    {
        private readonly IReactionRepository reactionRepository;

        public ReactionService(IReactionRepository reactionRepository)
        {
            this.reactionRepository = reactionRepository;
        }

        public void AddReaction(Reaction reaction)
        {
            try
            {
                Reaction oldReaction = reactionRepository.GetReaction(reaction.UserId, reaction.PostId);
                if (oldReaction.Type == reaction.Type)
                {
                    reactionRepository.Delete(reaction.UserId, reaction.PostId);
                }
                else
                {
                    reactionRepository.Update(reaction.UserId, reaction.PostId, reaction.Type);
                }

            }
            catch (Exception ex) {
                this.reactionRepository.Add(reaction);
            }
            
        }



        //public void DeleteReaction(long userId, long postId)
        //{
        //    Reaction reaction = reactionRepository.GetReaction(userId, postId);
            
        //    reactionRepository.Delete(userId, postId);
        //}

        //public List<Reaction> GetAllReactions()
        //{
        //    return reactionRepository.GetAllReactions();
        //}

        public List<Reaction> GetReactionsByPostId(long postId)
        {
            return reactionRepository.GetReactionsByPostId(postId);
        }

        public Reaction GetReaction(long userId, long postId)
        {
            return this.reactionRepository.GetReaction(userId, postId);
        }

        //public void UpdateReaction(Reaction reaction)
        //{
        //    reactionRepository.GetReaction(reaction.UserId, reaction.PostId);

        //    this.reactionRepository.Update(reaction.UserId, reaction.PostId, reaction.Type);
        //}
    }
}
