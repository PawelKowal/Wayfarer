using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Infrastructure.Entities;
using ApplicationCore.Interfaces;

namespace Infrastructure.Repositories
{
    public class ReactionsRepository : IReactionsRepository
    {
        private readonly WayfarerDbContext _context;

        public ReactionsRepository(WayfarerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddPostReactionAsync(int userId, int postId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(post => post.PostId == postId);
            if (post is null)
            {
                return false;
            }

            var reaction = await _context.PostsReactions.FirstOrDefaultAsync(reaction => reaction.PostId == postId && reaction.UserId == userId);
            if (reaction is not null)
            {
                post.ReactionsCounter--;
                _context.PostsReactions.Remove(reaction);
            }
            else
            {
                var newReaction = new PostReaction
                {
                    UserId = userId,
                    PostId = postId
                };
                post.ReactionsCounter++;
                await _context.PostsReactions.AddAsync(newReaction);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddCommentReactionAsync(int userId, int commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(comment => comment.CommentId == commentId);
            if (comment is null)
            {
                return false;
            }

            var reaction = await _context.CommentsReactions.FirstOrDefaultAsync(reaction => reaction.CommentId == commentId && reaction.UserId == userId);
            if (reaction is not null)
            {
                comment.ReactionsCounter--;
                _context.CommentsReactions.Remove(reaction);
            }
            else
            {
                var newReaction = new CommentReaction
                {
                    UserId = userId,
                    CommentId = commentId
                };
                comment.ReactionsCounter++;
                await _context.CommentsReactions.AddAsync(newReaction);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeletePostReactionAsync(int userId, int postId)
        {
            var reaction = await _context.PostsReactions.FirstOrDefaultAsync(reaction => reaction.PostId == postId && reaction.UserId == userId);
            if (reaction is null)
            {
                return false;
            }

            var post = await _context.Posts.FirstOrDefaultAsync(e => e.PostId == postId);
            if (post is not null)
            {
                post.ReactionsCounter--;
            }
            try
            {
                _context.PostsReactions.Remove(reaction);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteCommentReactionAsync(int userId, int commentId)
        {
            var reaction = await _context.CommentsReactions.FirstOrDefaultAsync(reaction => reaction.CommentId == commentId && reaction.UserId == userId);
            if (reaction is null)
            {
                return false;
            }

            var comment = await _context.Comments.FirstOrDefaultAsync(e => e.CommentId == commentId);
            if (comment is not null)
            {
                comment.ReactionsCounter--;
            }
            try
            {
                _context.CommentsReactions.Remove(reaction);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public Task<bool> DoesPostReactionExistAsync(int userId, int postId)
        {
            return _context.PostsReactions.AnyAsync(e => e.PostId == postId && e.UserId == userId);
        }

        public Task<bool> DoesCommentReactionExistAsync(int userId, int commentId)
        {
            return _context.CommentsReactions.AnyAsync(e => e.CommentId == commentId && e.UserId == userId);
        }
    }
}
