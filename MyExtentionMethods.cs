  public static partial class ext
    {
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> collection) where T:class
        {
            return collection.Where(x => x != null);
        }

    }
     public static partial class Ext
    {
        public static string GetString(this DbDataReader reader, string fieldName)
        {
            return reader.GetString(reader.GetOrdinal(fieldName));
        }

        public static DateTime GetDateTime(this DbDataReader reader, string fieldName)
        {
            return reader.GetDateTime(reader.GetOrdinal(fieldName));
        }

        public static void AddIfNo<TKey, TVal>(this Dictionary<TKey, TVal> dic, TKey key, TVal val)
        {
            if (!dic.ContainsKey(key))
            {
                dic.Add(key, val);
            }
        }
    }
