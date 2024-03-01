using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDSClient.Models
{
    public class WDSEnums
    {
        public enum CallTypes { NotSet, StoredProc, SQL, GetDataFromFile, FileUpdate, DailyFileUpdate, WebService, FileOperations }
        public enum ConnectionTypes { Database, Filesystem, WebService };
        public enum LogLevels { Information, Warning, Error }
        public enum DataFormatTypes { NotSet, Json, CSV, Xml, Email }
        public enum DataDetailTypes { NotSet, Default, Simple }
        public enum SourcedFromTypes { File, Database, WebService };
    }
}
