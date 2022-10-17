using BlazorDictionary.Common.Events.Entry;
using BlazorDictionary.Common.Infrastructure;
using BlazorDictionary.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Api.Application.Features.Commands.Entry.DeleteFav
{
    public class DeleteEntryFavCommandHandler : IRequestHandler<DeleteEntryFavCommand, bool>
    {
        public Task<bool> Handle(DeleteEntryFavCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: DictionaryConstants.FavExchangeName,
                exchangeType: DictionaryConstants.DefaultExchangeType,
                queueName: DictionaryConstants.DeleteEntryFavQueueName,
                obj: new DeleteEntryFavEvent()
                {
                    EntryId = request.EntryId,
                    CreatedBy = request.UserId
                });

            return Task.FromResult(true);
        }
    }
}
