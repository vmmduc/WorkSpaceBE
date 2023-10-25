namespace Models.Objects.Hubs
{
    public class SendMessage
    {
        public int conversationId { get; set; }
        public string? messageContent { get; set; }
        public int groupId { get; set; }
        public int senderId { get; set; }
    }
}
