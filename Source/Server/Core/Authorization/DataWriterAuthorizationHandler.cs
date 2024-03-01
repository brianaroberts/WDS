using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using WDS.Authentication;
using WDS.Settings;

namespace WDS.Authorization
{
    public class DataWriterAuthorizationHandler : AuthorizationHandler<DataWriterRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DataWriterRequirement requirement)
        {
            if (ConnectionsModel.ConnectionEnvironment == ConnectionsModel.ConnectionEnvironmentTypes.DevelopmentFullAccess 
                || context.User.IsInRole(Roles.WDS_Admin) || context.User.Identity.AuthenticationType == "API Key")
                context.Succeed(requirement); 

            foreach (var req in context.Requirements)
            {
                if (req.GetType() ==  typeof(RolesAuthorizationRequirement))
                {
                    foreach (var role in ((RolesAuthorizationRequirement)req).AllowedRoles)
                        if (context.User.IsInRole(role.ToString()))
                            context.Succeed(requirement); 
                }
            }

            if ((requirement.Type == DataWriterTypes.WDS && context.User.IsInRole(Roles.WDS_DataWriter)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

            
        }

    }
}
