using BlazorDictionary.Api.Application.Interfaces.Repositories;
using BlazorDictionary.Common.ViewModels.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Api.Application.Features.Queries.SearchBySubject
{
    public class SearchEntryQueryHandler : IRequestHandler<SearchEntryQuery, List<SearchEntryViewModel>>
    {
        private readonly IEntryRepository _entryRepository;

        public SearchEntryQueryHandler(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public async Task<List<SearchEntryViewModel>> Handle(SearchEntryQuery request, CancellationToken cancellationToken)
        {
            var result = _entryRepository.Get(i => EF.Functions.Like(i.Subject, $"{request.SearchText}%"))
                                         .Select(s => new SearchEntryViewModel()
                                         {
                                             Id = s.Id,
                                             Subject = s.Subject
                                         });

            return await result.ToListAsync(cancellationToken);
        }
    }
}
