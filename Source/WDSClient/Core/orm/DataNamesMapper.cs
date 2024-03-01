using System.Data;

namespace DataService.Core.Data
{
    public class DataNamesMapper<TEntity> where TEntity : class, new()
    {
        public static TEntity Map(DataRow row)
        {
            
            var columnNames = row.Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

            
            var properties = (typeof(TEntity)).GetProperties()
                                              .Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any())
                                              .ToList();
            
            TEntity entity = new TEntity();
            foreach (var prop in properties)
            {
                PropertyMapHelper.Map(typeof(TEntity), row, prop, entity);
            }

            return entity;
        }
        public static IEnumerable<TEntity> Map(DataTable table)
        {
            var columnNames = table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

            var properties = (typeof(TEntity)).GetProperties()
                                                .Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any())
                                                .ToList();

            List<TEntity> entities = new List<TEntity>();
            foreach (DataRow row in table.Rows)
            {
                TEntity entity = new TEntity();
                foreach (var prop in properties)
                {
                    PropertyMapHelper.Map(typeof(TEntity), row, prop, entity);
                }
                entities.Add(entity);
            }

            return entities;
        }

    }
}
