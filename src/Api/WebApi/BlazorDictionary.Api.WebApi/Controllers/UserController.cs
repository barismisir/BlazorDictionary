using BlazorDictionary.Api.Application.Features.Commands.User.ConfirmEmail;
using BlazorDictionary.Api.Application.Features.Queries.GetEntryDetail;
using BlazorDictionary.Api.Application.Features.Queries.GetUserDetail;
using BlazorDictionary.Common.Events.User;
using BlazorDictionary.Common.ViewModels.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorDictionary.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : BaseController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new GetUserDetailQuery(id));

            return Ok(result);
        }

        [HttpGet("UserName/{userName}")]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            var result = await _mediator.Send(new GetUserDetailQuery(Guid.Empty,userName));

            return Ok(result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginUserCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]CreateUserCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        [HttpPost]
        [Route("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody]UpdateUserCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(Guid id)
        {
            var res = await _mediator.Send(new ConfirmEmailCommand() { ConfirmationId = id});

            return Ok(res);
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand command)
        {
            if (!command.UserId.HasValue)
                command.UserId = UserId;

            var res = await _mediator.Send(command);

            return Ok(res);
        }

    }
}
