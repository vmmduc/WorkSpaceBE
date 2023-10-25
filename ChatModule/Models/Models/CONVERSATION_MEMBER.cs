using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class CONVERSATION_MEMBER
    {
        [Key]
        public int PK_CONVERSATION_MEMBER_ID { get; set; }
        public int? FK_USER_ID { get; set; }
        public string? PSEUDONYM { get; set; }
        public int? FK_CONVERSATION_ID { get; set; }
        public int? UNREAD_COUNT { get; set; }
        public bool? IS_DELETED { get; set; }
    }
}
