namespace WDSClient.Models.Users
{
    public interface IUser
    {
        string DisplayName { get; set; }
        string Division { get; set; }
        string EDIPI { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string Fullname { get; }
        string LastName { get; set; }
        string MiddleInitial { get; set; }
        string Office { get; set; }
        string Organization { get; set; }
        string Phone { get; set; }
        char PTC { get; set; }
        string Rank { get; set; }
        string ServiceBranch { get; set; }
        string SessionKey { get; set; }
        string Title { get; set; }
    }
}