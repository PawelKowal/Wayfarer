using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Dtos;
using AutoMapper;

namespace Infrastructure.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private WayfarerDbContext _context;
        private readonly IMapper _mapper;

        public CommentsRepository(WayfarerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentDto> AddCommentAsync(CommentDto commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);

            var newComment = await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return _mapper.Map<CommentDto>(newComment.Entity);
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            _context.Comments.RemoveRange(_context.Comments.Where(comment => comment.CommentId == commentId));
            await _context.SaveChangesAsync();
        }

        public async Task<CommentDto> GetCommentByIdAsync(int id)
        {
            var comment = await _context.Comments
                .Include(comment => comment.User)
                .FirstOrDefaultAsync(comment => comment.CommentId == id);

            return _mapper.Map<CommentDto>(comment);
        }
    }
}
