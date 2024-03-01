using Newtonsoft.Json;
using System.Data;
using System.Text;

namespace WDS.Core.Extensions
{
    public static class DataTableExtensions
    {
        public static string ToCsv(this DataTable dataTable)
        {
            StringBuilder sbData = new StringBuilder();
            string sep = ","; 

            if (dataTable == null || dataTable.Rows.Count == 0)
                return null; 

            for (int i=0; i< dataTable.Columns.Count; i++)
            {
                sbData.Append(dataTable.Columns[i]);
                if (i < dataTable.Columns.Count - 1)
                    sbData.Append(sep);
            }
            sbData.AppendLine();

            foreach (DataRow dr in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    sbData.Append(dr[i].ToString());

                    if (i < dataTable.Columns.Count - 1)
                        sbData.Append(sep);
                }
                sbData.AppendLine();
            }

            return sbData.ToString();
        }

        public static void LoadCsv(this DataTable datatable, string csv)
        {
            if (csv.IsNullOrEmpty()) return; 

            datatable.Clear();
            var csvRows = csv.Split(Environment.NewLine);

            if (csvRows.Length == 0) return;
            var csvCols = csvRows[0].Split(",");

            for (int i = 0; i < csvCols.Length; i++)
            {
                datatable.Columns.Add(csvCols[i].Replace("\"", ""));
            }
            
            //TODO: Handle exceptions when data length is different that columns
            for (int i = 1; i < csvRows.Length-1; i++)
            {
                datatable.Rows.Add(csvRows[i].Replace("\"", "").Split(",")); 
            }
        }

        public static void LoadCsv(this DataTable datatable, StreamReader sr)
        {
            datatable.LoadCsv(sr.ReadToEnd());
        }

        public static string ToJson(this DataTable datatable)
        {
            return JsonConvert.SerializeObject(datatable);
        }

        public static string ToXml(this DataTable datatable)
        {
            var ds = new DataSet();
            ds.Tables.Add(datatable);
            var returnValue = ds.GetXml();
            return returnValue;
        }

    }
}
