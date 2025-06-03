namespace ServerLibraryProject.Services
{

    using System;
    using System.Collections.Generic;
    using ServerLibraryProject.Enums;
    using ServerLibraryProject.Interfaces;
    using ServerLibraryProject.Models;

    /// <summary>
    /// Service for managing posts.
    /// </summary>
    public class PostService : IPostService
    {
        private IPostRepository postRepository;
        private IUserRepository userService;
        private IGroupRepository groupRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostService"/> class.
        /// </summary>
        /// <param name="postRepository">The post repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="groupRepository">The group repository.</param>
        public PostService(IPostRepository postRepository, IUserRepository userRepository, IGroupRepository groupRepository)
        {
            this.postRepository = postRepository;
            this.userService = userRepository;
            this.groupRepository = groupRepository;
        }

        /// <summary>
        /// Adds a new post.
        /// </summary>
        /// <param name="title">The title of the post.</param>
        /// <param name="content">The content of the post.</param>
        /// <param name="userId">The ID of the user creating the post.</param>
        /// <param name="groupId">The ID of the group where the post is created.</param>
        /// <param name="postVisibility">The visibility of the post.</param>
        /// <param name="postTag">The tag of the post.</param>
        /// <returns>The created post.</returns>
        public void AddPost(string title, string content, long userId, long? groupId, PostVisibility postVisibility, PostTag postTag)
        {
            if (title == null || title.Length == 0)
            {
                throw new Exception("Post title cannot be empty");
            }
            if (this.userService.GetById(userId) == null)
            {
                throw new Exception("User does not exist");
            }
            if (groupId != 0)
            {
                if (groupId != null && this.groupRepository.GetGroupById((long)groupId) == null)
                {
                    throw new Exception("Group does not exist");
                }
            }
            Post post = new Post() { Title = title, Content = content, UserId = userId, GroupId = groupId, Visibility = postVisibility, Tag = postTag, CreatedDate = DateTime.Now };
            this.postRepository.SavePost(post);
        }

        /// <summary>
        /// Deletes a post by ID.
        /// </summary>
        /// <param name="id">The ID of the post to delete.</param>
        //public void DeletePost(long id)
        //{
        //    if (this.postRepository.GetPostById(id) == null)
        //    {
        //        throw new Exception("Post does not exist");
        //    }

        //    this.postRepository.DeletePostById(id);
        //}

        /// <summary>
        /// Updates a post by ID.
        /// </summary>
        /// <param name="id">The ID of the post to update.</param>
        /// <param name="title">The new title of the post.</param>
        /// <param name="description">The new description of the post.</param>
        /// <param name="visibility">The new visibility of the post.</param>
        /// <param name="tag">The new tag of the post.</param>
        //public void UpdatePost(long id, string title, string description, PostVisibility visibility, PostTag tag)
        //{
        //    if (this.postRepository.GetPostById(id) == null)
        //    {
        //        throw new Exception("Post does not exist");
        //    }

        //    this.postRepository.UpdatePostById(id, title, description, visibility, tag);
        //}

        /// <summary>
        /// Gets all posts.
        /// </summary>
        /// <returns>A list of all posts.</returns>
        public List<Post> GetAllPosts()
        {
            return this.postRepository.GetAllPosts();
        }

        /// <summary>
        /// Gets a post by ID.
        /// </summary>
        /// <param name="id">The ID of the post to retrieve.</param>
        /// <returns>The post with the specified ID.</returns>
        public Post GetPostById(long id)
        {
            return this.postRepository.GetPostById(id);
        }

        /// <summary>
        /// Gets posts by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose posts to retrieve.</param>
        /// <returns>A list of posts by the specified user.</returns>
        public List<Post> GetPostsByUserId(long userId)
        {
            return this.postRepository.GetPostsByUserId(userId);
        }

        /// <summary>
        /// Gets posts by group ID.
        /// </summary>
        /// <param name="groupId">The ID of the group whose posts to retrieve.</param>
        /// <returns>A list of posts in the specified group.</returns>
        public List<Post> GetPostsByGroupId(long groupId)
        {
            return this.postRepository.GetPostsByGroupId(groupId);
        }

        /// <summary>
        /// Gets the home feed posts for a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose home feed to retrieve.</param>
        /// <returns>A list of posts for the user's home feed.</returns>
        public List<Post> GetPostsHomeFeed(long userId)
        {
            return this.postRepository.GetPostsHomeFeed(userId);
        }

        /// <summary>
        /// Gets the group feed posts for a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose group feed to retrieve.</param>
        /// <returns>A list of posts for the user's group feed.</returns>
        //public List<Post> GetPostsGroupsFeed(long userId)
        //{
        //    return this.postRepository.GetPostsGroupsFeed(userId);
        //}

        //public void SavePost(Post entity)
        //{
        //    this.postRepository.SavePost(entity);
        //}
    }
}
