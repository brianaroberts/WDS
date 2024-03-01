namespace DataService.Models.Azure
{
    public class ChangeRequestModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AssignedTo { get; set; }
        public string RemedyTicketNumber { get; set; }

        public string Description { get; set; }

    }
}
