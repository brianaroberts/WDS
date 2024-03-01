using System.Collections.Generic;

namespace DataService.Models.Settings
{
    public interface IRequestParameters
    {
        public Dictionary<string,string> UserParameters { get; set; }
    }
}
