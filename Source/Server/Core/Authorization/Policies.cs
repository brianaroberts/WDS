using Microsoft.AspNetCore.Authorization;

namespace WDS.Authorization
{
    public static class Policies
    {          
        public const string Full = nameof(Full);        // Create, Read, Update, Delete
        public const string Read = nameof(Read);        // Read only
        public const string Create = nameof(Create);    // Create and Read
        

        public static void AddPolicies(AuthorizationOptions options)
        {  
            options.AddPolicy(Full, policy => policy.Requirements.Add(new DataWriterRequirement(DataWriterTypes.WDS)));
            options.AddPolicy(Read, policy => policy.Requirements.Add(new DataConsumerRequirement(DataConsumerTypes.WDS)));
            options.AddPolicy(Create, policy => policy.Requirements.Add(new DataWriterRequirement(DataWriterTypes.WDS)));
        }
    }
}
