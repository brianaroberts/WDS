namespace WDSClient.Models.Users
{
    public class ApplicationUserRole : IApplicationUserRole
    {
        public string Role { get; set; }
        public DateTime Expires { get; set; }
        public bool IsActive { get; set; }
    }
}
