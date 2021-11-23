using ApplicationCore.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IChatsRepository
    {
        Task SaveMessageAsync(ChatMessageDto chatMessageDto);
        string? GetConnectionId(int userId);
        Task DeleteConnectionIdAsync(int authorId);
        Task AddConnectionIdAsync(int authorId, string connectionId);
        IEnumerable<ChatMessageDto> GetChatMessages(int user1Id, int user2Id);
    }
}
