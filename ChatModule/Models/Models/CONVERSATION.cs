using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class CONVERSATION
    {
        [Key]
        public int PK_CONVERSATION_ID { get; set; }
        public string? CONVERSATION_NAME { get; set; }
        public bool? IS_GROUP { get; set; }
    }
}
