namespace Models.Models
{
    public class USER_TOKEN
    {
        public int PK_USER_TOKEN_ID { get; set; }
        public int? FK_USER_ID { get; set; }
        public string? REFRESH_TOKEN { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? EXPIRES_TIME { get; set; }
        public bool IS_REVOKED { get; set; }
        public DateTime? REVOKE_DATE { get; set; }
    }
}