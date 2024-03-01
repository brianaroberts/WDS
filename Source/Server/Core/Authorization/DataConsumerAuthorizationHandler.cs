using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using WDS.Settings;

namespace WDS.Authorization
{
    public class DataConsumerAuthorizationHandler : AuthorizationHandler<DataConsumerRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DataConsumerRequirement requirement)
        {
            if (ConnectionsModel.ConnectionEnvironment == ConnectionsModel.ConnectionEnvironmentTypes.DevelopmentFullAccess 
                || context.User.IsInRole(Roles.WDS_Admin)  || context.User.Identity.AuthenticationType == "API Key")
                context.Succeed(requirement);

            if (requirement.Type == DataConsumerTypes.WDS && context.User.IsInRole(Roles.WDS_DataConsumer))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
