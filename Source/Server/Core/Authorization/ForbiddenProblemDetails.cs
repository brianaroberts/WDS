using Microsoft.AspNetCore.Mvc;

namespace WDS.Authentication
{
    public class ForbiddenProblemDetails : ProblemDetails
    {
        public ForbiddenProblemDetails(string details = null)
        {
            Title = "Unauthorized";
            Detail = details;
            Status = 401;
            Type = "https://httpstatuses.com/401"; 
        }
    }
}
