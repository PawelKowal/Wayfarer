using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class Chat
    {
        public int ChatId { get; set; }
        public int User1Id { get; set; }
        [ForeignKey("User1Id")]
        public User User1 { get; set; }
        public int User2Id { get; set; }
        [ForeignKey("User2Id")]
        public User User2 { get; set; }
        public List<ChatMessage> Messages { get; set; }
        public DateTimeOffset LastMessageTime { get; set; }
    }
}
