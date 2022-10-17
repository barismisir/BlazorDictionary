using BlazorDictionary.Common.Events.Entry;
using BlazorDictionary.Common.Infrastructure;
using BlazorDictionary.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Api.Application.Features.Commands.Entry.DeleteVote
{
    public class DeleteEntryVoteCommandHandler : IRequestHandler<DeleteEntryVoteCommand, bool>
    {
        public Task<bool> Handle(DeleteEntryVoteCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: DictionaryConstants.VoteExchangeName,
                exchangeType: DictionaryConstants.DefaultExchangeType,
                queueName: DictionaryConstants.DeleteEntryVoteQueueName,
                obj: new DeleteEntryVoteEvent()
                {
                    EntryId = request.EntryId,
                    CreatedBy = request.UserId
                });

            return Task.FromResult(true);
        }
    }
}
