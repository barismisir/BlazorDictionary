using BlazorDictionary.Common.Events.EntryComment;
using BlazorDictionary.Common.Infrastructure;
using BlazorDictionary.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorDictionary.Common.Events.Entry;

namespace BlazorDictionary.Api.Application.Features.Commands.Entry.CreateFav
{
    public class CreateEntryFavCommandHandler : IRequestHandler<CreateEntryFavCommand, bool>
    {
        public Task<bool> Handle(CreateEntryFavCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: DictionaryConstants.FavExchangeName,
                exchangeType: DictionaryConstants.DefaultExchangeType,
                queueName: DictionaryConstants.CreateEntryCommentFavQueueName,
                obj: new CreateEntryFavEvent()
                {
                    EntryId = request.EntryId.Value,
                    CreatedBy = request.UserId.Value
                });

            return Task.FromResult(true);
        }
    }
}
