using System;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IFollowsRepository
    {
        Task<bool> CheckIfFollowExistAsync(Guid followerId, Guid followedId);
        Task<bool> FollowUserAsync(Guid followerId, Guid followedId);
        Task<bool> UnfollowUserAsync(Guid followerId, Guid followedId);
    }
}
