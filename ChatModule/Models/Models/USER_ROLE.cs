using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class USER_ROLE
    {
        [Key]
        public int PK_USER_ROLE_ID { get; set; }
        public int FK_USER_ID { get; set; }
        public int FK_ROLE_ID { get; set; }
    }
}
