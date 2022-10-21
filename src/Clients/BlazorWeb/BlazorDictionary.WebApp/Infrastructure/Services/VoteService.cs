using BlazorDictionary.Common.ViewModels;
using BlazorDictionary.WebApp.Infrastructure.Services.Interfaces;

namespace BlazorDictionary.WebApp.Infrastructure.Services
{
    public class VoteService : IVoteService
    {
        private readonly HttpClient _client;

        public VoteService(HttpClient client)
        {
            _client = client;
        }

        public async Task DeleteEntryVote(Guid entryId)
        {
            var response = await _client.PostAsync($"/api/vote/DeleteEntryVote/{entryId}", null);

            if (!response.IsSuccessStatusCode)
                throw new Exception("DeleteEntryVote error");
        }

        public async Task DeleteEntryCommentVote(Guid entryCommentId)
        {
            var response = await _client.PostAsync($"/api/vote/DeleteEntryCommentVote/{entryCommentId}", null);

            if (!response.IsSuccessStatusCode)
                throw new Exception("DeleteEntryCommentVote error");
        }

        public async Task CreateEntryUpVote(Guid entryId)
        {
            await CreateEntryVote(entryId, VoteType.Up);
        }

        public async Task CreateEntryDownVote(Guid entryId)
        {
            await CreateEntryVote(entryId, VoteType.Down);
        }

        private async Task<HttpResponseMessage> CreateEntryVote(Guid entryId, VoteType voteType = VoteType.Up)
        {
            var response = await _client.PostAsync($"/api/vote/entry/{entryId}?voteType={voteType}", null);

            return response;
        }

        public async Task CreateEntryCommentUpVote(Guid entryCommentId)
        {
            await CreateEntryCommentVote(entryCommentId, VoteType.Up);
        }

        public async Task CreateEntryCommentDownVote(Guid entryCommentId)
        {
            await CreateEntryCommentVote(entryCommentId, VoteType.Down);
        }

        private async Task<HttpResponseMessage> CreateEntryCommentVote(Guid entryCommentId, VoteType voteType = VoteType.Up)
        {
            var response = await _client.PostAsync($"/api/vote/entrycomment/{entryCommentId}?voteType={voteType}", null);

            return response;
        }
    }
}
