namespace NeoIsisJob.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using ServerLibraryProject.Models;
    using Workout.Core.IServices;

    public class CommentServiceProxy : ICommentService
    {
        private readonly HttpClient httpClient;

        public CommentServiceProxy()
        {

            httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5261/api/comments/"),
            };
        }

        /// <summary>
        /// Adds a new comment.
        /// </summary>
        public Comment AddComment(string content, int userId, long postId)
        {
            var comment = new Comment
            {
                Content = content,
                UserId = userId,
                PostId = postId,

                CreatedDate = DateTime.UtcNow,
            };

            var response = httpClient.PostAsJsonAsync(string.Empty, comment).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<Comment>().Result;
            }
            throw new Exception($"Failed to add comment: {response.StatusCode}");
        }

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        //public void DeleteComment(long commentId)
        //{

        //    var response = this._httpClient.DeleteAsync($"{commentId}").Result;
        //    response.EnsureSuccessStatusCode();
        //}

        /// <summary>
        /// Retrieves all comments.
        /// </summary>
        public List<Comment> GetAllComments()
        {
            var response = httpClient.GetAsync("").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<List<Comment>>().Result;
            }
            throw new Exception($"Failed to get comments: {response.StatusCode}");
        }

        /// <summary>
        /// Retrieves a comment by its ID.
        /// </summary>
        //public Comment GetCommentById(int commentId)
        //{

        //    return this._httpClient.GetFromJsonAsync<Comment>($"{commentId}").Result!;
        //}

        /// <summary>
        /// Retrieves all comments for a specific post.
        /// </summary>
        public List<Comment> GetCommentsByPostId(long postId)
        {
            var client = new HttpClient();
            var response = client.GetAsync($"https://localhost:7106/api/posts/{postId}/comments").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<List<Comment>>().Result;
            }
            throw new Exception($"Failed to get comments: {response.StatusCode}");

        }

        /// <summary>
        /// Updates the content of an existing comment.
        /// </summary>
        //public void UpdateComment(long commentId, string content)
        //{
        //    var commentDto = new
        //    {

        //        Content = content,
        //    };

        //    var response = this._httpClient.PutAsJsonAsync($"{commentId}", commentDto).Result;
        //    response.EnsureSuccessStatusCode();
        //}
    }
}
