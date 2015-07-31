   public static class MyFile
    {
        public static void Copy(string sourceFileName, string destFolder)
        {
            string fn = Path.GetFileName(sourceFileName);
            try
            {
                Directory.CreateDirectory(destFolder);
            }
            catch (Exception)
            {
                
               
            }
            File.Copy(sourceFileName, Path.Combine(destFolder, fn));
        }
    }
   public static class MyFile
    {
        public static void Copy(string sourceFileName, string destFolder)
        {
            string fn = Path.GetFileName(sourceFileName);
            File.Copy(sourceFileName,Path.Combine(destFolder,fn));
        }
    }
    
  public static partial class ext
    {
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> collection) where T:class
        {
            return collection.Where(x => x != null);
        }

    }
     public static partial class Ext
    {
        public static string CapitalizeWord(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;
            else
            {
               return s[0].ToString().ToUpper() + s.Substring(1).ToLower();
            }
        }
        public static string RemoveEndIfExists(this string s, string end, bool ignorecase)
        {
            if (s.EndsWith(end, ignorecase, CultureInfo.CurrentCulture))
            {
                return s.Substring(0, s.Length - end.Length);
            }
            return s;

        }

          public static string GetStringTrim(this IDataRecord r,int index)
       {
           return r.GetString(index).Trim();
       }
               public static string GetStringTrim(this IDataRecord r, string fieldName)
        {
            return r.GetString(r.GetOrdinal(fieldName)).Trim();
        }

        public static void AddIfNo<TKey, TVal>(this Dictionary<TKey, TVal> dic, TKey key, TVal val)
        {
            if (!dic.ContainsKey(key))
            {
                dic.Add(key, val);
            }
        }
         public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dic,TKey key, TValue defValue)
        {
            TValue result;
            bool b = dic.TryGetValue(key, out result);
            if (!b)
            {
                result = defValue;
            }
            return result;
        }
        public static T GetData<T>(this IDataObject dataObject)
        {
            var data = (T)dataObject.GetData(typeof (T));
            return data;
        }
        
    }
