using System;

namespace Controller.Dtos.Chat
{
    public class ChatMessageResponse
    {
        public int AuthorId { get; set; }
        public string Message { get; set; }
        public DateTimeOffset SendAt { get; set; }
    }
}
