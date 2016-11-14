  public class Person
    {
        public Guid ID { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
    }
    public class MyReflectionRowMapper<T>  where T : new()
    {
        public MyReflectionRowMapper()
        {
            var t = typeof(T);
            var props = t.GetProperties();
            var d = new Dictionary<string, PropertyInfo>();
            foreach (var propertyInfo in props)
            {
                d.Add(propertyInfo.Name.ToLower(), propertyInfo);
            }
            dic = d;
        }
        protected string GetPropertyNameFromFieldName(string fieldName)
        {
            return fieldName;
        }
        private readonly IReadOnlyDictionary<string, PropertyInfo> dic;
        public T MapRow(IDataRecord row)
        {
            T obj = new T();
            for (int i = 0; i < row.FieldCount; i++)
            {
                var name = row.GetName(i);
                var n = GetPropertyNameFromFieldName(name);
                if (dic.ContainsKey(n.ToLower()))
                {
                    var p = dic[n.ToLower()];
                    p.SetValue(obj, row[i]);
                }
            }
            return obj;
        }
    }
    public static class PersonGetter
    {
        const string selectSP = "_SelectAll";

        public static IList<T> SelectAll<T>() where T:new()
        {
            var m = new MyReflectionRowMapper<T>();
            List<T> result = new List<T>();
            using (var conn = new SqlConnection(Constants.CS))
            using (var cmd = new SqlCommand(nameof(T) + selectSP))
            {
                (cmd.Connection = conn).Open();
                cmd.CommandType = CommandType.StoredProcedure;
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                    {
                        var e = m.MapRow(r);
                        result.Add(e);
                    }

                return result;

            }
        }
    }

    public static class ExtPerson
    {

        const string insertSP = "_Create";
        const string deleteSP = "_Delete";
        const string updateSP = "_Update";
        public static void Update(this Person p)
        {
            RunSP(p, nameof(Person) + updateSP);
        }

        private static void RunSP(Person p, string spName)
        {
            var t = p.GetType();
            using (var conn = new SqlConnection(Constants.CS))
            using (var cmd = new SqlCommand(spName))
            {
                (cmd.Connection = conn).Open();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(cmd);
                foreach (SqlParameter par in cmd.Parameters)
                {
                    if (par.ParameterName=="@RETURN_VALUE")
                       continue;
                    var n = par.ParameterName;
                    n = n.Replace("@", string.Empty);
                    var prop = t.GetProperty(n);

                    var v = prop.GetValue(p);
                    par.Value = v;
                }
                cmd.ExecuteNonQuery();
            }
        }

        public static void Create(this Person p)
        {
            RunSP(p, nameof(Person) + insertSP);
        }
        public static void Delete(this Person p)
        {
            RunSP(p, nameof(Person) + deleteSP);
        }

    }
