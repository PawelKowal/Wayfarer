using Controller.Dtos.Chat;
using System.Threading.Tasks;

namespace Controller.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(ChatMessageResponse message);
    }
}
