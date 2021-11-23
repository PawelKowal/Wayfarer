using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Dtos;
using ApplicationCore.Interfaces;
using AutoMapper;
using Controller.Dtos.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Controller.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IMapper _mapper;
        private readonly IChatsRepository _chatsRepository;

        public ChatHub(IMapper mapper, IChatsRepository chatsRepository)
        {
            _mapper = mapper;
            _chatsRepository = chatsRepository;
        }

        [Authorize]
        public async Task SendMessage(ChatMessageRequest message)
        {
            var claim = Context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim is not null)
            {
                var authorId = int.Parse(claim.Value);

                var messageDto = _mapper.Map<ChatMessageDto>(message);
                messageDto.AuthorId = authorId;
                messageDto.SendAt = DateTimeOffset.Now;

                await _chatsRepository.SaveMessageAsync(messageDto);

                var receiverConnectionId = _chatsRepository.GetConnectionId(message.ReceiverId);

                if (receiverConnectionId is not null)
                {
                    await Clients.Client(receiverConnectionId).ReceiveMessage(_mapper.Map<ChatMessageResponse>(messageDto));
                }
            }
        }

        [Authorize]
        public override async Task OnConnectedAsync()
        {
            var claim = Context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim is not null)
            {
                var authorId = int.Parse(claim.Value);

                await _chatsRepository.AddConnectionIdAsync(authorId, Context.ConnectionId);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var claim = Context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim is not null)
            {
                var authorId = int.Parse(claim.Value);

                await _chatsRepository.DeleteConnectionIdAsync(authorId);
            }
        }
    }
}
