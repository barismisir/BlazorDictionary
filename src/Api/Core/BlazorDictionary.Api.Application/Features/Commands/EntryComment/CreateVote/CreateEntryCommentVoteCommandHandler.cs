using BlazorDictionary.Common.Events.Entry;
using BlazorDictionary.Common.Infrastructure;
using BlazorDictionary.Common;
using BlazorDictionary.Common.ViewModels.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorDictionary.Common.Events.EntryComment;

namespace BlazorDictionary.Api.Application.Features.Commands.EntryComment.CreateVote
{
    public class CreateEntryCommentVoteCommandHandler : IRequestHandler<CreateEntryCommentVoteCommand, bool>
    {
        public Task<bool> Handle(CreateEntryCommentVoteCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: DictionaryConstants.VoteExchangeName,
                exchangeType: DictionaryConstants.DefaultExchangeType,
                queueName: DictionaryConstants.CreateEntryCommentVoteQueueName,
                obj: new CreateEntryCommentVoteEvent()
                {
                    EntryCommentId = request.EntryCommentId,
                    VoteType = request.VoteType,
                    CreatedBy = request.CreatedBy
                });

            return Task.FromResult(true);
        }
    }
}
