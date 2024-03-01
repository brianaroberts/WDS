using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;

namespace DataService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ADOController : Controller
    {
        

        private Dictionary<string, string> AzureDevOpsCalls = new Dictionary<string, string>()
        {
            { "GetWorkItems", "https://{ado_URL}/{collection}/{project}/_apis/wit/workitems?ids={ids}" },
            { "UpdateWorkItem", "https://{ado_URL}/{collection}/{project}/_apis/wit/workitems?ids={ids}" }, // HTTP Verb PATCH
            { "CreateWorkItem", "https://{ado_URL}/{collection}/{project}/_apis/wit/workitems/${type}" }, // HTTP Verb POST
            { "GetProjects", ""},
            // https://{ado_URL}/ccj6-es/Web%20Team/_apis/wit/workitems?ids=404,405,407,48,409,410,411,412
        }; 

        public IActionResult GetProjects()
        {
            return new ObjectResult(true);
        }
    }
}
