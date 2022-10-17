using BlazorDictionary.Api.Application.Features.Commands.EntryComment.EntryComment;
using BlazorDictionary.Api.Application.Interfaces.Repositories;
using BlazorDictionary.Common;
using BlazorDictionary.Common.Events.EntryComment;
using BlazorDictionary.Common.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Api.Application.Features.Commands.EntryComment.CreateFav
{
    public class CreateEntryCommentFavCommandHandler : IRequestHandler<CreateEntryCommentFavCommand,bool>
    {
        private readonly IUserRepository _userRepository;

        public CreateEntryCommentFavCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> Handle(CreateEntryCommentFavCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: DictionaryConstants.FavExchangeName,
                exchangeType: DictionaryConstants.DefaultExchangeType,
                queueName: DictionaryConstants.CreateEntryCommentFavQueueName,
                obj:new CreateEntryCommentFavEvent()
                {
                    EntryCommentId = request.EntryCommentId,
                    CreatedBy = request.UserId
                });

            return await Task.FromResult(true);
        }

    }
}
