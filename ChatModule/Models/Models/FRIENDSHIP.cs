using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class FRIENDSHIP
    {
        [Key]
        public int PK_FRIENDSHIP_ID { get; set; }
        public int FK_USER_ID { get; set; }
        public int FK_FRIEND_ID { get; set; }
        public int FK_STATE_ID { get; set; }
    }
}
