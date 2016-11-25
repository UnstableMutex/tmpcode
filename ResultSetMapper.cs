  public class MyReflectionResultSetMapper<T> : IResultSetMapper<T> where T : new()
    {
        public MyReflectionResultSetMapper()
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
        protected virtual string GetPropertyNameFromFieldName(string fieldName)
        {
            return fieldName;
        }
        private readonly IReadOnlyDictionary<string, PropertyInfo> dic;

        public IEnumerable<T> MapSet(IDataReader reader)
        {
            Dictionary<int,PropertyInfo> dicn=new Dictionary<int, PropertyInfo>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                var n = GetPropertyNameFromFieldName(name).ToLower();
                if (dic.ContainsKey(n))
                {
                    dicn[i] = dic[n];
                }
            }

            while (reader.Read())
            {
                T obj = new T();
                foreach (var key in dicn.Keys)
                {
                    var p = dicn[key];
                    p.SetValue(obj,reader[key]);
                }
                yield return obj;
            }
        }
    }
