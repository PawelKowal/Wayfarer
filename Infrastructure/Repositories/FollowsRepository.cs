using ApplicationCore.Interfaces;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FollowsRepository : IFollowsRepository
    {
        private WayfarerDbContext _context;

        public FollowsRepository(WayfarerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfFollowExistAsync(Guid followerId, Guid followedId)
        {
            return await _context.Follows.AnyAsync(f => f.FollowerId == followerId && f.FollowedId == followedId);
        }

        public async Task<bool> FollowUserAsync(Guid followerId, Guid followedId)
        {
            try
            {
                await _context.Follows.AddAsync(new Follow() { FollowedId = followedId, FollowerId = followerId });
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UnfollowUserAsync(Guid followerId, Guid followedId)
        {
            var follow = await _context.Follows.FirstOrDefaultAsync(f => f.FollowedId == followedId && f.FollowerId == followerId);
            if (follow is null)
            {
                return false;
            }

            try
            {
                _context.Follows.Remove(follow);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
