using System;

namespace ApplicationCore.Dtos
{
    public class ChatMessageDto
    {
        public int AuthorId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTimeOffset SendAt { get; set; }
    }
}
