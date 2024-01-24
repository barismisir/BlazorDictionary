using BlazorDictionary.Common.ViewModels.Page;
using BlazorDictionary.Common.ViewModels.Queries;
using BlazorDictionary.Common.ViewModels.RequestModels;
using BlazorDictionary.WebApp.Infrastructure.Services.Interfaces;
using System.Net.Http.Json;

namespace BlazorDictionary.WebApp.Infrastructure.Services
{
    public class EntryService : IEntryService
    {
        private readonly HttpClient _client;

        public EntryService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<GetEntriesViewModel>> GetEntries()
        {
            var response = await _client.GetFromJsonAsync<List<GetEntriesViewModel>>($"/api/entry?todaysEntites=false&count=30");

            return response;
        }

        public async Task<GetEntryDetailViewModel> GetEntryDetail(Guid entryId)
        {
            var response = await _client.GetFromJsonAsync<GetEntryDetailViewModel>($"/api/entry/{entryId}");

            return response;
        }

        public async Task<PagedViewModel<GetEntryDetailViewModel>> GetMainPageEntries(int page, int pageSize)
        {
            var response = await _client.GetFromJsonAsync<PagedViewModel<GetEntryDetailViewModel>>($"/api/entry/mainpageentries?page={page}&pageSize={pageSize}");

            return response;
        }

        public async Task<PagedViewModel<GetEntryDetailViewModel>> GetProfilePageEntries(int page, int pageSize, string userName)
        {
            var response = await _client.GetFromJsonAsync<PagedViewModel<GetEntryDetailViewModel>>($"/api/entry/userentries?userName={userName}&userId={null}&page={page}&pageSize={pageSize}");

            return response;
        }

        public async Task<PagedViewModel<GetEntryDetailViewModel>> GetUserEntries(string userName, Guid userId, int page, int pageSize)
        {
            var response = await _client.GetFromJsonAsync<PagedViewModel<GetEntryDetailViewModel>>($"/api/entry/userentries?userName={userName}&userId={userId}&page={page}&pageSize={pageSize}");

            return response;
        }

        public async Task<PagedViewModel<GetEntryCommentsViewModel>> GetEntryComments(Guid entryId, int page, int pageSize)
        {
            var response = await _client.GetFromJsonAsync<PagedViewModel<GetEntryCommentsViewModel>>($"/api/entry/comments/{entryId}?page={page}&pageSize={pageSize}");

            return response;
        }


        public async Task<Guid> CreateEntry(CreateEntryCommand command)
        {
            var response = await _client.PostAsJsonAsync($"/api/entry/createentry", command);

            if (!response.IsSuccessStatusCode)
                return Guid.Empty;

            var guidStr = await response.Content.ReadAsStringAsync();

            return new Guid(guidStr.Trim('"'));
        }

        public async Task<Guid> CreateEntryComments(CreateEntryCommentCommand command)
        {
            var response = await _client.PostAsJsonAsync($"/api/entry/createentrycomment", command);

            if (!response.IsSuccessStatusCode)
                return Guid.Empty;

            var guidStr = await response.Content.ReadAsStringAsync();

            return new Guid(guidStr.Trim('"'));
        }

        public async Task<List<SearchEntryViewModel>> SearchBySubject(string searchText)
        {
            var response = await _client.GetFromJsonAsync<List<SearchEntryViewModel>>($"/api/entry/search/{searchText}");

            return response;
        }


    }
}
