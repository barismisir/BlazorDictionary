using AutoMapper;
using BlazorDictionary.Api.Application.Interfaces.Repositories;
using BlazorDictionary.Common.Events.User;
using BlazorDictionary.Common.Infrastructure;
using BlazorDictionary.Common;
using BlazorDictionary.Common.Infrastructure.Exceptions;
using BlazorDictionary.Common.ViewModels.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Api.Application.Features.Commands.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await _userRepository.GetByIdAsync(request.Id);

            if (dbUser == null)
                throw new DatabaseValidationException("User not found!");

            var dbEmailAddress = dbUser.EmailAddress;
            var emailChanged = string.CompareOrdinal(dbEmailAddress,request.EmailAddress) != 0;

            _mapper.Map(request, dbUser);

            var rows = await _userRepository.UpdateAsync(dbUser);

            //Check if email changed

            if (emailChanged && rows > 0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = null,
                    NewEmailAddress = dbUser.EmailAddress
                };

                QueueFactory.SendMessageToExchange(exchangeName: DictionaryConstants.UserExchangeName, exchangeType: DictionaryConstants.DefaultExchangeType, queueName: DictionaryConstants.UserEmailExchangeQueueName, obj: @event);

                dbUser.EmailConfirmed = false;
                await _userRepository.UpdateAsync(dbUser);
            }

            return dbUser.Id;
        }
    }
}
