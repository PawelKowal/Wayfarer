using ApplicationCore.Dtos;
using AutoMapper;
using Infrastructure.Entities;

namespace Infrastructure.Profiles
{
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<ChatMessageDto, Chat>()
                .ForMember(chat => chat.User1Id, chatMessageDto => chatMessageDto.MapFrom(chatMessageDto => chatMessageDto.AuthorId))
                .ForMember(chat => chat.User2Id, chatMessageDto => chatMessageDto.MapFrom(chatMessageDto => chatMessageDto.ReceiverId));
            CreateMap<ChatMessageDto, ChatMessage>();
            CreateMap<ChatMessage, ChatMessageDto>();
        }
    }
}
