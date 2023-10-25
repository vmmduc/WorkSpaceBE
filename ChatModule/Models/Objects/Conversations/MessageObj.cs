namespace Models.Objects.Conversations
{
    public class MessageObj
    {
        public int messageId { get; set; }
        public int? senderId { get; set; }
        public string? messageContent { get; set; }
        public DateTime? createdDate { get; set; }
        public int conversationId { get; set; }
        public int? groupId { get; set; }
    }
}
