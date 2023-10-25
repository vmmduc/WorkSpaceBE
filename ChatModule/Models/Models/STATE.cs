using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class STATE
    {
        [Key]
        public int PK_STATE_ID {  get; set; }
        public string? STATE_CODE { get; set; }
        public string? STATE_NAME { get; set; }
    }
}
