namespace Models.Objects.Friends
{
    public class FriendObj
    {
        public int? PK_USER_ID { get; set; }
        public string? EMAIL { get; set; }
        public string? FULL_NAME { get; set; }
        public string? NICK_NAME { get; set; }
        public string? PHONE { get; set; }
        public string? StateCode { get; set; }
        public bool? IsSendRequest { get; set; }
        public int? FriendShipId { get; set; }
    }
}
