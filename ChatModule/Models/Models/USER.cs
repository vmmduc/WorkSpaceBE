using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class USER
    {
        [Key]
        public int PK_USER_ID { get; set; }
        public string? EMAIL { get; set; }
        public string? FULL_NAME { get; set; }
        public string? PHONE_NUMBER { get; set; }
        public string? PASSWORD { get; set; }
        public bool? DEACTIVATE { get; set; }
    }
}
