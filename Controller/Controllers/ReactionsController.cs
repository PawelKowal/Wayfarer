using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionsController : ControllerBase
    {
        private readonly IReactionsRepository _reactionsRepository;
        private readonly IPostsRepository _postsRepository;
        private readonly ICommentsRepository _commentsRepository;

        public ReactionsController(IReactionsRepository reactionsRepository,
            IPostsRepository postsRepository,
            ICommentsRepository commentsRepository)
        {
            _reactionsRepository = reactionsRepository;
            _postsRepository = postsRepository;
            _commentsRepository = commentsRepository;
        }

        // POST: api/reactions/post/{id}
        [Authorize]
        [HttpPost("post/{id}")]
        public async Task<ActionResult> AddPostReactionAsync(int postId)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var userId = int.Parse(authorizedUserId.Value);

            var post = _postsRepository.GetPostByIdAsync(postId, userId);
            if (post is null)
            {
                ModelState.AddModelError(string.Empty, "Post not found.");
                return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
            }

            var result = await _reactionsRepository.AddPostReactionAsync(userId, postId);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "An error occured.");
                return ValidationProblem(statusCode: StatusCodes.Status422UnprocessableEntity);
            }

            return Ok();
        }

        // DELETE: api/reactions/post/{id}
        [Authorize]
        [HttpDelete("post/{id}")]
        public async Task<ActionResult> DeletePostReactionAsync(int reactionId)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var result = await _reactionsRepository.DeletePostReactionAsync(int.Parse(authorizedUserId.Value), reactionId);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Reaction not found.");
                return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
            }

            return Ok();
        }

        // POST: api/reactions/comment/{id}
        [Authorize]
        [HttpPost("comment/{id}")]
        public async Task<ActionResult> AddCommentReactionAsync(int commentId)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var comment = _commentsRepository.GetCommentByIdAsync(commentId);
            if (comment is null)
            {
                ModelState.AddModelError(string.Empty, "Comment not found.");
                return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
            }

            var result = await _reactionsRepository.AddCommentReactionAsync(int.Parse(authorizedUserId.Value), commentId);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "An error occured.");
                return ValidationProblem(statusCode: StatusCodes.Status422UnprocessableEntity);
            }

            return Ok();
        }

        // DELETE: api/reactions/comment/{id}
        [Authorize]
        [HttpDelete("comment/{id}")]
        public async Task<ActionResult> DeleteCommentReactionAsync(int reactionId)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var reaction = await _reactionsRepository.DeleteCommentReactionAsync(int.Parse(authorizedUserId.Value), reactionId);

            if (!reaction)
            {
                ModelState.AddModelError(string.Empty, "Reaction not found.");
                return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
            }

            return Ok();
        }
    }
}
