using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorDictionary.Api.Application.Interfaces.Repositories;
using BlazorDictionary.Common.ViewModels.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Api.Application.Features.Queries.GetEntries
{
    public class GetEntriesQueryHandler : IRequestHandler<GetEntriesQuery, List<GetEntriesViewModel>>
    {
        private readonly IEntryRepository _entryRepository;
        private readonly IMapper _mapper;

        public GetEntriesQueryHandler(IEntryRepository entryRepository, IMapper mapper)
        {
            _entryRepository = entryRepository;
            _mapper = mapper;
        }

        public async Task<List<GetEntriesViewModel>> Handle(GetEntriesQuery request, CancellationToken cancellationToken)
        {
            var query = _entryRepository.AsQueryable();

            if (request.TodaysEntries)
                query = query
                    .Where(w => w.CreateDate >= DateTime.Now.Date)
                    .Where(w => w.CreateDate <= DateTime.Now.AddDays(1).Date);

            query = query.Include(i => i.EntryComments)
                .OrderBy(o => Guid.NewGuid())
                .Take(request.Count);

            return await query.ProjectTo<GetEntriesViewModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}
