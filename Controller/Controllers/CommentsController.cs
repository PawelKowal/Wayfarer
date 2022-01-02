using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Dtos;
using ApplicationCore.Interfaces;
using AutoMapper;
using Controller.Dtos.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IPostsRepository _postsRepository;
        private readonly IMapper _mapper;

        public CommentsController(ICommentsRepository commentsRepository, IPostsRepository postsRepository, IMapper mapper)
        {
            _commentsRepository = commentsRepository;
            _postsRepository = postsRepository;
            _mapper = mapper;
        }

        // POST: api/Comments
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CommentResponse>> AddCommentAsync([FromBody] CommentRequest comment)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var userId = int.Parse(authorizedUserId.Value);

            var post = await _postsRepository.GetPostByIdAsync(comment.PostId, userId);
            if (post is null)
            {
                ModelState.AddModelError(string.Empty, "Post not found.");
                return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
            }

            var commentDto = _mapper.Map<CommentDto>(comment);
            commentDto.UserId = userId;
            commentDto.PublicationDate = DateTimeOffset.Now;

            var newComment = await _commentsRepository.AddCommentAsync(commentDto);

            if (newComment is null)
            {
                ModelState.AddModelError(string.Empty, "An error occured.");
                return ValidationProblem(statusCode: StatusCodes.Status422UnprocessableEntity);
            }

            return Ok(_mapper.Map<CommentResponse>(newComment));
        }

        // DELETE: api/Comments/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var comment = await _commentsRepository.GetCommentByIdAsync(id);
            if (comment is null)
            {
                ModelState.AddModelError(string.Empty, "Comment not found.");
                return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
            }

            if (comment.UserId.ToString() != authorizedUserId)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            await _commentsRepository.DeleteCommentAsync(id);

            return NoContent();
        }
    }
}
