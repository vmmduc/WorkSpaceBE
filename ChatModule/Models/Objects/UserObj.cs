namespace Models.Objects
{
    public class UserObj
    {
        public int userId { get; set; }
        public string? email { get; set; }
        public string? fullName { get; set; }
        public string? phoneNumber { get; set; }
        public bool? deactivate { get; set; }
        public List<RoleObj>? lsRoles { get; set; }
        public string? token { get; set; }
        public string? refreshToken { get; set; }
    }
}
