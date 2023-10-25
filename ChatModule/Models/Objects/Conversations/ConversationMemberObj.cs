namespace Models.Objects.Conversations
{
    public class ConversationMemberObj
    {
        public int? userId { get; set; }
        public string? fullName { get; set; }
        public string? pseudonym { get; set; }

        public int? unreadMessageCount { get; set; }
        public string? newMessage { get; set; }
        public int? conversationId { get; set; }
    }
}
