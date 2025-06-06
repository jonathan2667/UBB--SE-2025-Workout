namespace NeoIsisJob.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using ServerLibraryProject.Interfaces;
    using Workout.Core.Enums;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    /// <summary>
    /// Proxy implementation of <see cref="IPostRepository"/> that communicates with a remote Post API.
    /// </summary>
    public class PostServiceProxy : IPostService
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostServiceProxy"/> class.
        /// </summary>
        public PostServiceProxy()
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5261/api/posts/"),
            };
        }

        public void AddPost(string title, string content, int userId, long? groupId, PostVisibility postVisibility, PostTag postTag)
        {
            Post newPost = new Post
            {
                Title = title,
                Content = content ?? string.Empty,
                UserId = userId,
                GroupId = groupId,
                Visibility = postVisibility,
                Tag = postTag,
                CreatedDate = DateTime.UtcNow,
            };
            var response = this.httpClient.PostAsJsonAsync(string.Empty, newPost).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to add post: {response.StatusCode}");
            }
        }

        /// <summary>
        /// Deletes a post by its unique identifier.
        /// </summary>
        /// <param name="postId">The ID of the post to delete.</param>
        //public void DeletePost(long postId)
        //{
        //    var response = this.httpClient.DeleteAsync($"{postId}").Result;
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception($"Failed to delete post {postId}: {response.StatusCode}");
        //    }
        //}

        /// <summary>
        /// Retrieves all posts.
        /// </summary>
        /// <returns>A list of all posts, or an empty list if none are found.</returns>
        public List<Post> GetAllPosts()
        {
            var response = this.httpClient.GetAsync(string.Empty).Result;

            if (response.IsSuccessStatusCode)
            {
                var posts = response.Content.ReadFromJsonAsync<List<Post>>().Result;
                return posts ?? new List<Post>();
            }

            throw new Exception($"Failed to get all posts: {response.StatusCode}");
        }

        /// <summary>
        /// Retrieves a post by its ID.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>The post if found, or a default post object if not found (404).</returns>
        public Post GetPostById(long postId)
        {
            var response = this.httpClient.GetAsync($"{postId}").Result;

            if (response.IsSuccessStatusCode)
            {
                var post = response.Content.ReadFromJsonAsync<Post>().Result;
            }


            throw new Exception($"Failed to get post {postId}: {response.StatusCode}");
        }

        /// <summary>
        /// Retrieves posts associated with a specific group.
        /// </summary>
        /// <param name="groupId">The group ID.</param>
        /// <returns>A list of posts, or an empty list if none are found.</returns>
        public List<Post> GetPostsByGroupId(long groupId)
        {
            var response = this.httpClient.GetAsync($"group/{groupId}").Result;

            if (response.IsSuccessStatusCode)
            {
                var posts = response.Content.ReadFromJsonAsync<List<Post>>().Result;
                return posts ?? new List<Post>();
            }

            throw new Exception($"Failed to get posts by groupId {groupId}: {response.StatusCode}");
        }

        /// <summary>
        /// Retrieves posts created by a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A list of posts, or an empty list if none are found.</returns>
        public List<Post> GetPostsByUserId(int userId)
        {
            var response = this.httpClient.GetAsync($"user/{userId}").Result;

            if (response.IsSuccessStatusCode)
            {
                var posts = response.Content.ReadFromJsonAsync<List<Post>>().Result;
                return posts ?? new List<Post>();
            }

            throw new Exception($"Failed to get posts by userId {userId}: {response.StatusCode}");
        }


        /// <summary>
        /// Retrieves a group feed for a user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>A list of group feed posts, or an empty list if none are found.</returns>
        //public List<Post> GetPostsGroupsFeed(long userId)
        //{
        //    var response = this.httpClient.GetAsync($"groupfeed/{userId}").Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var posts = response.Content.ReadFromJsonAsync<List<Post>>().Result;
        //        return posts ?? new List<Post>();
        //    }

        //    throw new Exception($"Failed to get group feed for user {userId}: {response.StatusCode}");
        //}

        /// <summary>
        /// Retrieves the home feed for a user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>A list of home feed posts, or an empty list if none are found.</returns>
        public List<Post> GetPostsHomeFeed(int userId)
        {
            var response = this.httpClient.GetAsync($"user/{userId}/homefeed").Result;

            if (response.IsSuccessStatusCode)
            {
                var posts = response.Content.ReadFromJsonAsync<List<Post>>().Result;
                return posts ?? new List<Post>();
            }

            throw new Exception($"Failed to get home feed for user {userId}: {response.StatusCode}");
        }

        /// <summary>
        /// Saves a new post.
        /// </summary>
        /// <param name="post">The post to save.</param>
        //public void SavePost(Post post)
        //{
        //    var request = new
        //    {
        //        title = post.Title,
        //        content = post.Content,
        //        createdDate = post.CreatedDate,
        //        userId = post.UserId,
        //        groupId = post.GroupId,
        //        visibility = post.Visibility,
        //        tag = post.Tag,
        //    };
        //    var response = this.httpClient.PostAsJsonAsync(string.Empty, request).Result;
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception($"Failed to save post: {response.StatusCode}");
        //    }
        //}

        /// <summary>
        /// Updates an existing post by ID with new values.
        /// </summary>
        /// <param name="postId">The ID of the post to update.</param>
        /// <param name="title">The new title.</param>
        /// <param name="content">The new content.</param>
        /// <param name="visibility">The new visibility setting.</param>
        /// <param name="tag">The new tag.</param>
        //public void UpdatePost(long postId, string title, string content, PostVisibility visibility, PostTag tag)
        //{
        //    var post = new PostDTO
        //    {
        //        Title = title,
        //        Content = content,
        //        Visibility = visibility,

        //        Tag = tag,
        //    };

        //    var response = this.httpClient.PutAsJsonAsync($"{postId}", post).Result;
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception($"Failed to update post {postId}: {response.StatusCode}");
        //    }
        //}

        /// <summary>
        /// Generates a default post with placeholder values.
        /// </summary>
        /// <returns>A default <see cref="Post"/> instance.</returns>
        //private Post GetDefaultPost()
        //{
        //    return new Post
        //    {
        //        Id = -1,
        //        Title = "Default Title",
        //        Content = "Default Content",
        //        CreatedDate = DateTime.MinValue,
        //        UserId = -1,
        //        GroupId = -1,
        //        Visibility = PostVisibility.Public,
        //        Tag = PostTag.Misc,
        //    };
        //}
    }
}
