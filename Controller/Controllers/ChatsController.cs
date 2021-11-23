using ApplicationCore.Dtos;
using ApplicationCore.Interfaces;
using AutoMapper;
using Controller.Dtos.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChatsRepository _chatsRepository;

        public ChatsController(IMapper mapper, IChatsRepository chatsRepository)
        {
            _mapper = mapper;
            _chatsRepository = chatsRepository;
        }

        // GET: api/Posts
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<ChatMessageDto>> GetAllPosts(int userId)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var messages = _chatsRepository.GetChatMessages(int.Parse(authorizedUserId.Value), userId);

            return Ok(messages.Select(messages => _mapper.Map<ChatMessageResponse>(messages)));
        }
    }
}
