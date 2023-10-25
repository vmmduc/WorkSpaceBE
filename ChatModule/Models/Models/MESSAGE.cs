using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class MESSAGE
    {
        [Key]
        public int PK_MESSAGE_ID { get; set; }
        public int FK_SENDER_ID { get; set; }
        public string? MESSAGE_CONTENT { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int FK_CONVERSATION_ID { get; set; }
        public int? GROUP_ID {get; set;}
    }
}
