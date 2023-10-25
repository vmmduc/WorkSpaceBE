namespace Common.HttpContextAccessor;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    int UserId { get; }
    string? Email { get; }
    public string? FullName { get; }
    public List<string> Roles { get; set; }
}