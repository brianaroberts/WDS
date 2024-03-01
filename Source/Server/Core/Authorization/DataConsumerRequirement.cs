using Microsoft.AspNetCore.Authorization;

namespace WDS.Authorization
{
    public enum DataConsumerTypes { WDS }

    public class DataConsumerRequirement : IAuthorizationRequirement
    {
        public DataConsumerTypes Type { get; set; }

        // Empty for now, can add new properties and methods here later
        public DataConsumerRequirement(DataConsumerTypes type)
        {
            Type = type; 
        }
    }
}
