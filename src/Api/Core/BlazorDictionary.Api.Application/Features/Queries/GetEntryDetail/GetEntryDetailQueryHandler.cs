using BlazorDictionary.Api.Application.Features.Queries.GetEntryComments;
using BlazorDictionary.Api.Application.Interfaces.Repositories;
using BlazorDictionary.Common.Infrastructure.Extensions;
using BlazorDictionary.Common.ViewModels.Page;
using BlazorDictionary.Common.ViewModels.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Api.Application.Features.Queries.GetEntryDetail
{
    public class GetEntryDetailQueryHandler:IRequestHandler<GetEntryDetailQuery, GetEntryDetailViewModel>
    {
        private readonly IEntryRepository _entryRepository;

        public GetEntryDetailQueryHandler(IEntryRepository entryReposityor)
        {
            _entryRepository = entryReposityor;
        }

        public async Task<GetEntryDetailViewModel> Handle(GetEntryDetailQuery request, CancellationToken cancellationToken)
        {
            var query = _entryRepository.AsQueryable();

            query = query.Include(i => i.EntryFavorites)
                         .Include(i => i.CreatedBy)
                         .Include(i => i.EntryVotes)
                         .Where(w => w.Id == request.EntryId);

            var list = query.Select(i => new GetEntryDetailViewModel()
            {
                Id = i.Id,
                Content = i.Content,
                IsFavorited = request.UserId.HasValue && i.EntryFavorites.Any(a => a.CreatedById == request.UserId),
                FavoritedCount = i.EntryFavorites.Count,
                CreatedDate = i.CreateDate,
                CreatedByUserName = i.CreatedBy.UserName,
                VoteType = request.UserId.HasValue && i.EntryVotes.Any(a => a.CreatedById == request.UserId)
                ? i.EntryVotes.FirstOrDefault(f => f.CreatedById == request.UserId).VoteType
                : Common.ViewModels.VoteType.None,
            });

            return await list.FirstOrDefaultAsync(cancellationToken:cancellationToken);
        }
    }
}
