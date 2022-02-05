using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class ChatMessage
    {
        public int ChatMessageId { get; set; }
        public int ChatId { get; set; }
        [ForeignKey("ChatId")]
        public Chat Chat { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public User Author { get; set; }
        public string Message { get; set; }
        public DateTimeOffset SendAt { get; set; }
    }
}
