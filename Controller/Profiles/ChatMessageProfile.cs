using ApplicationCore.Dtos;
using AutoMapper;
using Controller.Dtos.Chat;

namespace Controller.Profiles
{
    public class ChatMessageProfile : Profile
    {
        public ChatMessageProfile()
        {
            CreateMap<ChatMessageRequest, ChatMessageDto>();
            CreateMap<ChatMessageDto, ChatMessageResponse>();
        }
    }
}
