using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Entities;
using ApplicationCore.Dtos;
using ApplicationCore.Interfaces;
using AutoMapper;
using System;
using NetTopologySuite.Geometries;

namespace Infrastructure.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private WayfarerDbContext _context;
        private readonly IMapper _mapper;

        public PostsRepository(WayfarerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync(Guid authorizedUserId)
        {
            return await _context.Posts
                .Include(post => post.Comments)
                .ThenInclude(comment => comment.User)
                .Include(post => post.User)
                .Select(post => _mapper.Map<PostDto>(post))
                .Select(post => CheckIfReacted(authorizedUserId, post))
                .ToListAsync();
        }

        public async Task<PostDto> GetPostByIdAsync(int postId, Guid authorizedUserId)
        {
            var post = await _context.Posts
                .Include(post => post.Comments)
                .ThenInclude(comment => comment.User)
                .Include(post => post.User)
                .FirstOrDefaultAsync(post => post.PostId == postId);

            return CheckIfReacted(authorizedUserId, _mapper.Map<PostDto>(post));
        }

        public async Task<PostDto> AddPostAsync(PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);

            var newPost = await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return _mapper.Map<PostDto>(newPost.Entity);
        }

        public async Task<PostDto> UpdatePostAsync(PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);

            var newPost = _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return _mapper.Map<PostDto>(newPost.Entity);
        }

        public async Task DeletePostAsync(int postId)
        {
            _context.Posts.RemoveRange(_context.Posts.Where(post => post.PostId == postId));
            await _context.SaveChangesAsync();
        }

        private PostDto CheckIfReacted(Guid userId, PostDto post)
        {
            post.Reacted = _context.PostsReactions.Any(reaction => reaction.UserId == userId && reaction.PostId == post.PostId);
            post.Comments = post.Comments.ConvertAll(comment => CheckIfReactedToComment(userId, comment));
            return post;
        }

        private CommentDto CheckIfReactedToComment(Guid userId, CommentDto comment)
        {
            comment.Reacted = _context.CommentsReactions.Any(reaction => reaction.UserId == userId && reaction.CommentId == comment.CommentId);
            return comment;
        }

        public async Task<IEnumerable<PostDto>> GetPostsFromAreaAsync(AreaDto areaDto, Guid guid)
        {
            var area = new Polygon(new LinearRing(new Coordinate[4] {
                new Coordinate(areaDto.Latitude1, areaDto.Longitude1),
                new Coordinate(areaDto.Latitude2, areaDto.Longitude2),
                new Coordinate(areaDto.Latitude3, areaDto.Longitude3),
                new Coordinate(areaDto.Latitude4, areaDto.Longitude4)
            }));

            return await _context.Posts
                .Include(post => post.Comments)
                .ThenInclude(comment => comment.User)
                .Include(post => post.User)
                .Where(post => post.Location.Within(area))
                .Select(post => _mapper.Map<PostDto>(post))
                .ToListAsync();
        }
    }
}
