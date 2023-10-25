using Common.Base;

namespace Models.Objects.Conversations
{
    public class GetConverationById : Paging
    {
        public int conversationId { get; set; }
    }
}
