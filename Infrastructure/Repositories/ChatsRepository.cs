using ApplicationCore.Dtos;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ChatsRepository : IChatsRepository
    {
        private WayfarerDbContext _context;
        private readonly IMapper _mapper;

        public ChatsRepository(WayfarerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task SaveMessageAsync(ChatMessageDto chatMessageDto)
        {
            var chat = _context.Chats.FirstOrDefault(chat => (chat.User1Id == chatMessageDto.AuthorId && chat.User2Id == chatMessageDto.ReceiverId)
                || (chat.User1Id == chatMessageDto.ReceiverId && chat.User2Id == chatMessageDto.AuthorId));

            if (chat is null)
            {
                chat = (await _context.Chats.AddAsync(_mapper.Map<Chat>(chatMessageDto))).Entity;
                await _context.SaveChangesAsync();
            }

            var chatMessage = _mapper.Map<ChatMessage>(chatMessageDto);
            chatMessage.ChatId = chat.ChatId;
            chat.LastMessageTime = chatMessage.SendAt;

            await _context.ChatMessages.AddAsync(chatMessage);
            await _context.SaveChangesAsync();
        }

        public string? GetConnectionId(int userId)
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == userId);

            if (user is not null)
            {
                return user.ConnectionId;
            }

            return null;
        }

        public async Task DeleteConnectionIdAsync(int authorId)
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == authorId);

            if (user is not null)
            {
                user.ConnectionId = null;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddConnectionIdAsync(int authorId, string connectionId)
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == authorId);

            if (user is not null)
            {
                user.ConnectionId = connectionId;
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<ChatMessageDto> GetChatMessages(int user1Id, int user2Id)
        {
            var chat = _context.Chats
                .Include(chat => chat.Messages)
                .FirstOrDefault(chat => (chat.User1Id == user1Id && chat.User2Id == user2Id)
                    || (chat.User1Id == user2Id && chat.User2Id == user1Id));

            if (chat is null)
            {
                return null;
            }

            return chat.Messages.Select(message => _mapper.Map<ChatMessageDto>(message));
        }
    }
}
