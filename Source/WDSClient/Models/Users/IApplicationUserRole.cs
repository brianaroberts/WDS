namespace WDSClient.Models.Users
{
    public interface IApplicationUserRole
    {
        DateTime Expires { get; set; }
        bool IsActive { get; set; }
        string Role { get; set; }
    }
}