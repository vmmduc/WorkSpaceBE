namespace Models.Objects.Conversations
{
    public class ConversationObj
    {
        public int conversationId { get; set; }
        public bool? isGroup { get; set; }
        public MessageObj? message { get; set; }
        public string? displayName { get; set; }
        public int? unreadMessageCount { get; set; }
        public List<MessageObj> lsMessage { get; set; } = new List<MessageObj>();
        public DateTime? lastUpdatedDate { get; set; }
        public List<ConversationMemberObj> conversationMembers { get; set; } = new List<ConversationMemberObj>();
    }
}
