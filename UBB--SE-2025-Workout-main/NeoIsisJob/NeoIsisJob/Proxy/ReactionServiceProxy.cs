namespace NeoIsisJob.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using ServerLibraryProject.Enums;
    using ServerLibraryProject.Interfaces;
    using ServerLibraryProject.Models;
    using ServerLibraryProject.Interfaces;

    public class ReactionServiceProxy : IReactionService
    {
        private readonly HttpClient httpClient;

        public ReactionServiceProxy()
        {
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri("https://localhost:7106/api/reactions/");
        }

        public List<Reaction> GetReactionsByPostId(long postId)
        {
            var client = new HttpClient();
            var response = client.GetAsync($"https://localhost:7106/api/posts/{postId}/reactions").Result!;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<List<Reaction>>().Result ?? new List<Reaction>();
            }

            return new List<Reaction>();
        }

        public Reaction? GetReaction(long userId, long postId)
        {
            var response = this.httpClient.GetAsync($"{userId}/{postId}").Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var reaction = response.Content.ReadFromJsonAsync<Reaction>().Result;
                    return reaction;
                }
            }

            return null;
        }

        public void AddReaction(Reaction reaction)
        {
            var response = this.httpClient.PostAsJsonAsync(string.Empty, reaction).Result;
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new Exception($"Failed to add reaction: {response.StatusCode}");
        }
    }
}
