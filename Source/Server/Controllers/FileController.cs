using WDS.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using DataService.Core.Settings;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using DataService.Models.Settings;
using static WDSClient.Models.WDSEnums;

namespace WDS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class FileController : WDSControllerBase
    {
        #region Members
        
        
        #endregion

        public FileController() : base("WDS")
        {
            
        }

        [HttpPost("UploadFile")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public async Task<IActionResult> UploadFile(string recordsetName, IFormFile file)
        {
            var rs = TryGetRS(recordsetName, out ObjectResult result); 
            
            if (rs == null)
            {
                // Got to settings to determine file path
                var filePath = Path.GetTempFileName();
            
                try
                {
                    var fileToUpload = new StreamWriter(file.OpenReadStream());
                    fileToUpload.WriteAsync(filePath).Wait();

                    // Send path information to the database to get filePath
                    var fileId = 0;

                    result = new ObjectResult(fileId);
                }
                catch (Exception ex) 
                {
                    result = new ObjectResult(null);
                }
            }

            return result; 
        }

        [HttpPost("UpdateFile")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult UpdateFile(string recordsetName, string fileId, Dictionary<string, string> metaData)
        {
            var rs = TryGetRS(recordsetName, out ObjectResult result);

            if (rs == null)
            {
                result = new ObjectResult(false);
            }

            return result;
        }

        [HttpPut("DeleteFile")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult DeleteFile(string recordsetName, string fileId)
        {
            var rs = TryGetRS(recordsetName, out ObjectResult result);

            if (rs == null)
            {
                result = new ObjectResult(false);
            }

            return result;
        }

        [HttpGet("GetFile")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public async Task<IActionResult> GetFile(string recordsetName, string fileId)
        {
            var rs = TryGetRS(recordsetName, out ObjectResult result);

            if (rs == null)
            {
                // Get the filePath from the Database
                var filePath = fileId;

                // Make this a function call in file operations
                Stream fileStream = new FileStream(fileId, FileMode.Open);
                if (fileStream == null)
                    return new ObjectResult(StatusCode((int)HttpStatusCode.NotFound, "File was not found"));

                return File(fileStream, "application/octet-stream", $"{Path.GetFileName(filePath)}");
            }

            return result; 
        }

        [HttpGet("GetFileInfo")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult GetFileInfo(string recordsetName, string fileId)
        {
            var rs = TryGetRS(recordsetName, out ObjectResult result);

            if (rs == null)
            {
                result = new ObjectResult(false);
            }

            return result;
        }

        [HttpGet("UpdateFileInfo")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult GetFileInfo(string recordsetName, string fileId, Dictionary<string, string> metaData)
        {
            var rs = TryGetRS(recordsetName, out ObjectResult result);

            if (rs == null)
            {
                result = new ObjectResult(false);
            }

            return result;
        }

        private Recordset TryGetRS(string recordsetName, out ObjectResult result)
        {
            var rs = DataServiceSettings.Rs(recordsetName);
            result = null;

            if (recordsetName.IndexOf('.') == -1)
                result = StatusCode((int)HttpStatusCode.NotFound, $"Application was not included in call ({recordsetName}) ie (Application.Call)."); ;
            if (rs == null)
                result = StatusCode((int)HttpStatusCode.NotFound, $"Recordset was not found ({recordsetName}).");
            if (rs.Model.CallType == CallTypes.FileOperations)
                result = StatusCode((int)HttpStatusCode.BadRequest, $"Recordset was found ({recordsetName}), but is not the correct call type ({rs.Model.CallType}).");

            return rs; 
        }
    }
}
