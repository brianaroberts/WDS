using Newtonsoft.Json;
using System.Data;
using WDS.Core.Extensions;
using WDS.Models;

namespace WDSClient.Models
{
    public class WDSClientResult : WDSResponseContract
    {
        public WDSClientResult(string error) : base(error)
        {
        }
    }
}
