using AutoMapper;
using BlazorDictionary.Api.Application.Interfaces.Repositories;
using BlazorDictionary.Common;
using BlazorDictionary.Common.Events.User;
using BlazorDictionary.Common.Infrastructure;
using BlazorDictionary.Common.Infrastructure.Exceptions;
using BlazorDictionary.Common.ViewModels.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Api.Application.Features.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existUser = await _userRepository.GetSingleAsync(i=>i.EmailAddress == request.EmailAddress);

            if (existUser != null)
                throw new DatabaseValidationException("User already exists!");

            var dbUser = _mapper.Map<Domain.Models.User>(request);

            dbUser.Password = PasswordEncryptor.Encrpt(request.Password);

            var rows = await _userRepository.AddAsync(dbUser);

            //Email Changed/Created
            if (rows > 0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = null,
                    NewEmailAddress = dbUser.EmailAddress
                };

                QueueFactory.SendMessageToExchange(exchangeName:DictionaryConstants.UserExchangeName,exchangeType:DictionaryConstants.DefaultExchangeType,queueName:DictionaryConstants.UserEmailExchangeQueueName,obj:@event);
            }

            return dbUser.Id;
        }
    }
}
