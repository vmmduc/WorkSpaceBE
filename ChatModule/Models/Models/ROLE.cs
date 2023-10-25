using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class ROLE
    {
        [Key]
        public int PK_ROLE_ID { get; set; }
        public string? ROLE_CODE { get; set; }
        public string? ROLE_NAME { get; set; }
    }
}
