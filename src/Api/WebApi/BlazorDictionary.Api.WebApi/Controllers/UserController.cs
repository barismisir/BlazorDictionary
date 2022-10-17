using BlazorDictionary.Api.Application.Features.Commands.User.ConfirmEmail;
using BlazorDictionary.Common.Events.User;
using BlazorDictionary.Common.ViewModels.RequestModels;
using MediatR;
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

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginUserCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody]CreateUserCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        [HttpPost]
        [Route("Update")]
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
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand command)
        {
            if (!command.UserId.HasValue)
                command.UserId = UserId;

            var res = await _mediator.Send(command);

            return Ok(res);
        }

    }
}
