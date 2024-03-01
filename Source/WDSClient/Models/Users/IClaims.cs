namespace WDSClient.Models.Users
{
    public interface IClaims
    {
        string Type { get; set; }
        string Value { get; set; }
    }
}