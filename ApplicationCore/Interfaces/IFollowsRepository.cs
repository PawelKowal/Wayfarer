using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IFollowsRepository
    {
        Task<bool> CheckIfFollowExistAsync(int followerId, int followedId);
        Task<bool> FollowUserAsync(int followerId, int followedId);
        Task<bool> UnfollowUserAsync(int followerId, int followedId);
    }
}
