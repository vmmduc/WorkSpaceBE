namespace Models.Objects
{
    public class RefreshTokenObj
    {
        public int UserId { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresTime { get; set; }
    }
}