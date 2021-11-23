using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Dtos;
using ApplicationCore.Interfaces;
using AutoMapper;
using Controller.Dtos.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IPostsService _postsService;
        private readonly IMapper _mapper;

        public PostsController(IPostsRepository postsRepository, IPostsService postsService, IMapper mapper)
        {
            _postsRepository = postsRepository;
            _postsService = postsService;
            _mapper = mapper;
        }

        // GET: api/Posts
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostResponse>>> GetAllPostsAsync()
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var posts = await _postsRepository.GetAllPostsAsync(int.Parse(authorizedUserId.Value));

            return Ok(posts.Select(post => _mapper.Map<PostResponse>(post)));
        }

        // GET: api/Posts/{id}
        [Authorize]
        [HttpGet("{id}", Name = "GetPostById")]
        public async Task<ActionResult<PostResponse>> GetPostByIdAsync(int id)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var post = await _postsRepository.GetPostByIdAsync(id, int.Parse(authorizedUserId.Value));

            if (post == null)
            {
                ModelState.AddModelError(string.Empty, "Post not found.");
                return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
            }

            return Ok(_mapper.Map<PostResponse>(post));
        }

        // POST: api/Posts
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddPostAsync([FromBody] PostRequest post)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var postDto = _mapper.Map<PostDto>(post);
            postDto.UserId = int.Parse(authorizedUserId);

            var newPost = await _postsRepository.AddPostAsync(postDto);

            if (newPost is null)
            {
                ModelState.AddModelError(string.Empty, "An error occured.");
                return ValidationProblem(statusCode: StatusCodes.Status422UnprocessableEntity);
            }

            return Ok(_mapper.Map<PostResponse>(newPost));
        }

        // PUT: api/Posts/{id}
        [Authorize]
        [HttpPost("{id}")]
        public async Task<ActionResult<PostResponse>> UpdatePostAsync(int id, [FromBody] PostRequest post)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var oldPost = await _postsRepository.GetPostByIdAsync(id, int.Parse(authorizedUserId.Value));

            if (oldPost.UserId.ToString() != authorizedUserId.Value)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var postDto = _postsService.UpdatePostData(oldPost, _mapper.Map<PostDto>(post));

            var updatedPost = await _postsRepository.UpdatePostAsync(postDto);

            if (updatedPost is null)
            {
                ModelState.AddModelError(string.Empty, "An error occured.");
                return ValidationProblem(statusCode: StatusCodes.Status422UnprocessableEntity);
            }

            return Ok(_mapper.Map<PostResponse>(updatedPost));
        }

        // DELETE: api/Posts/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var post = await _postsRepository.GetPostByIdAsync(id, int.Parse(authorizedUserId.Value));
            if (post is null)
            {
                ModelState.AddModelError(string.Empty, "Post not found.");
                return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
            }

            if (post.UserId.ToString() != authorizedUserId.Value)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            await _postsRepository.DeletePostAsync(id);

            return NoContent();
        }

        // GET: api/Posts/{from-area}
        [Authorize]
        [HttpGet("{from-area}")]
        public async Task<ActionResult<IEnumerable<PostResponse>>> GetPostsFromAreaAsync([FromBody] AreaRequest area)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var posts = await _postsRepository.GetPostsFromAreaAsync(_mapper.Map<AreaDto>(area), int.Parse(authorizedUserId.Value));

            return Ok(_mapper.Map<PostResponse>(posts));
        }
    }
}
