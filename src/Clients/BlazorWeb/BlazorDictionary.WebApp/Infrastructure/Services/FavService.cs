using BlazorDictionary.WebApp.Infrastructure.Services.Interfaces;

namespace BlazorDictionary.WebApp.Infrastructure.Services
{
    public class FavService : IFavService
    {
        private readonly HttpClient _client;

        public FavService(HttpClient client)
        {
            _client = client;
        }

        public async Task CreateEntryFav(Guid entryId)
        {
            await _client.PostAsync($"/api/favorite/entry/{entryId}", null);
        }

        public async Task CreateEntryCommentFav(Guid entryCommentId)
        {
            await _client.PostAsync($"/api/favorite/entrycomment/{entryCommentId}", null);
        }

        public async Task DeleteEntryFav(Guid entryId)
        {
            await _client.PostAsync($"/api/favorite/deleteentry/{entryId}", null);
        }

        public async Task DeleteEntryCommentFav(Guid entryCommentId)
        {
            await _client.PostAsync($"/api/favorite/deleteentrycomment/{entryCommentId}", null);
        }
    }
}
