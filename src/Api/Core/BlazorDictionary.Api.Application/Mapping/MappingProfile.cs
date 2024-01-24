using AutoMapper;
using BlazorDictionary.Api.Domain.Models;
using BlazorDictionary.Common.ViewModels.Queries;
using BlazorDictionary.Common.ViewModels.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Api.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, LoginUserViewModel>()
                .ReverseMap();

            CreateMap<CreateUserCommand, User>();

            CreateMap<UpdateUserCommand, User>();

            CreateMap<UserDetailViewModel, User>()
                .ReverseMap();

            CreateMap<CreateEntryCommand, Entry>()
                .ReverseMap();

            CreateMap<Entry, GetEntriesViewModel>()
                .ForMember(x=>x.CommentCount, y=>y.MapFrom(z=>z.EntryComments.Count));

            CreateMap<CreateEntryCommentCommand, EntryComment>()
                .ReverseMap();

        }
    }
}
