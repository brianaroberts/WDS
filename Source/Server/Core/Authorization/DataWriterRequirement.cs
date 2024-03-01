using Microsoft.AspNetCore.Authorization;

namespace WDS.Authorization
{
    public enum DataWriterTypes { WDS }
    public class DataWriterRequirement : IAuthorizationRequirement
    {
        public DataWriterTypes Type { get; set; }   

        // Empty for now, can add new properties and methods here later
        public DataWriterRequirement(DataWriterTypes type)
        {
            Type = type; 
        }
    }
}
