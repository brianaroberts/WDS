namespace WDSClient.Models.Users
{
    public interface IApplicationUser
    {
        List<IClaims> Claims { get; set; }
        DateTime LastAccessed { get; set; }
        List<IApplicationUserRole> Roles { get; set; }
    }
}