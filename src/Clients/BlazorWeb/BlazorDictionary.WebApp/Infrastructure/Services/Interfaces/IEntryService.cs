using BlazorDictionary.Common.ViewModels.Page;
using BlazorDictionary.Common.ViewModels.Queries;
using BlazorDictionary.Common.ViewModels.RequestModels;

namespace BlazorDictionary.WebApp.Infrastructure.Services.Interfaces
{
    public interface IEntryService
    {
        Task<Guid> CreateEntry(CreateEntryCommand command);
        Task<Guid> CreateEntryComments(CreateEntryCommentCommand command);
        Task<List<GetEntriesViewModel>> GetEntries();
        Task<PagedViewModel<GetEntryCommentsViewModel>> GetEntryComments(Guid entryId, int page, int pageSize);
        Task<PagedViewModel<GetEntryDetailViewModel>> GetProfilePageEntries(int page, int pageSize, string userName);
        Task<GetEntryDetailViewModel> GetEntryDetail(Guid entryId);
        Task<PagedViewModel<GetEntryDetailViewModel>> GetMainPageEntries(int page, int pageSize);
        Task<PagedViewModel<GetEntryDetailViewModel>> GetUserEntries(string userName, Guid userId, int page, int pageSize);
        Task<List<SearchEntryViewModel>> SearchBySubject(string searchText);
    }
}