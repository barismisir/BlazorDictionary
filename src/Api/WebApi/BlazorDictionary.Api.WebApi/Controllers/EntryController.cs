using BlazorDictionary.Api.Application.Features.Queries.GetEntries;
using BlazorDictionary.Api.Application.Features.Queries.GetEntryComments;
using BlazorDictionary.Api.Application.Features.Queries.GetEntryDetail;
using BlazorDictionary.Api.Application.Features.Queries.GetMainPageEntries;
using BlazorDictionary.Api.Application.Features.Queries.GetUserEntries;
using BlazorDictionary.Common.ViewModels.Queries;
using BlazorDictionary.Common.ViewModels.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorDictionary.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class EntryController : BaseController
    {
        private readonly IMediator _mediator;

        public EntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetEntryDetailQuery(id,UserId));

            return Ok(result);
        }

        [HttpGet("/Comments/{id}")]
        public async Task<IActionResult> GetEntryComments(Guid id,int page,int pageSize)
        {
            var result = await _mediator.Send(new GetEntryCommentsQuery(page,pageSize,id,UserId));

            return Ok(result);
        }

        [HttpGet("UserEntries")]
        [Authorize]
        public async Task<IActionResult> GetUserEntries(string userName, Guid userId, int page,int pageSize)
        {
            if (userId == Guid.Empty && string.IsNullOrEmpty(userName))
                userId = UserId.Value;

            var result = await _mediator.Send(new GetUserEntriesQuery(userId, userName,page,pageSize));

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntries([FromQuery] GetEntriesQuery getEntriesQuery)
        {
            var entries = await _mediator.Send(getEntriesQuery);

            return Ok(entries);
        }

        [HttpGet]
        [Route("MainPageEntries")]
        public async Task<IActionResult> GetMainPageEntries(int page, int pageSize)
        {
            var entries = await _mediator.Send(new GetMainPageEntriesQuery(UserId,page,pageSize));

            return Ok(entries);
        }

        [HttpPost]
        [Route("CreateEntry")]
        [Authorize]
        public async Task<IActionResult> CreateEntry([FromBody] CreateEntryCommand command)
        {
            if (!command.CreatedById.HasValue)
                command.CreatedById = UserId;

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        [Route("CreateEntryComment")]
        [Authorize]
        public async Task<IActionResult> CreateEntryComment([FromBody] CreateEntryCommentCommand command)
        {
            if (!command.CreatedById.HasValue)
                command.CreatedById = UserId;

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpGet]
        [Route("Search/{searchText}")]
        public async Task<IActionResult> Search(string searchText)
        {
            SearchEntryQuery query = new SearchEntryQuery() { SearchText = searchText };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

    }
}
