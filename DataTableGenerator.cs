  using CsvHelper;
using CsvHelper.Configuration;


  public class DataTableGenerator<T>
    {
        public DataTable GetTable(string csvFile)
        {
            var conf = new CsvConfiguration
            {
                Delimiter = CultureInfo.CurrentCulture.TextInfo.ListSeparator,
                Encoding = Encoding.GetEncoding(1251)
            };

            conf.RegisterClassMap(conf.AutoMap<T>());
            conf.TrimHeaders = true;
   
            DataTable dt = CreateTable();
            var t = typeof(T);

            using (var sr = new StreamReader(csvFile, conf.Encoding))
            using (var r = new CsvReader(sr, conf))
            {
                while (r.Read())
                {
                    T obj = r.GetRecord<T>();
                    var row = dt.NewRow();
                    foreach (DataColumn column in dt.Columns)
                    {
                        var p = t.GetProperty(column.ColumnName);
                        row[column] = p.GetValue(obj, null);
                    }
                    dt.Rows.Add(row);
                }


            }

            return dt;

        }

        private DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            var t = typeof(T);
            foreach (var pi in t.GetProperties())
            {
                var dc = GetColumn(pi);
                dt.Columns.Add(dc);
            }
            return dt;
        }

        private DataColumn GetColumn(PropertyInfo pi)
        {
            var t = pi.PropertyType;
            var pkatt = t.GetCustomAttribute<PKAttribute>();

            DataColumn dt = new DataColumn(pi.Name, t);
            if (pkatt != null)
            {
                dt.Unique = true;
            }
            return dt;
        }
    }
