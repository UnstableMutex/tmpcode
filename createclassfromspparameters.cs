  private static void Main(string[] args)
        {
            string cs =
                @"Data Source=SSMRDB2\GENESISINSTANCE;Initial Catalog=Genesis;Persist Security Info=True;User ID=AllUsers;Password=qW12Ltakz;MultipleActiveResultSets=True";
            string spName = "LoadWritedOff";
            string className = "Par";
            SqlParameterCollection parameters;
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = spName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);
                    parameters = cmd.Parameters;
                }
            }
            var outStream = Console.Out;
            outStream.Write("class {0}\n", className);
            outStream.Write("{\n");
            WriteParameters(outStream, parameters);
            outStream.Write("}");
            Console.ReadKey();
        }
        private static void WriteParameters(TextWriter outStream, DbParameterCollection parameters)
        {
            foreach (var parameter in parameters)
            {
                DbParameter p = (DbParameter)parameter;
                outStream.Write("\tpublic {0} {1} {{get;set;}}", ConvertType(p.DbType), ConvertName(p.ParameterName));
                outStream.Write(Environment.NewLine);
            }
        }
        private static string ConvertName(string parameterName)
        {
            string s= parameterName.RemoveLeftIfPresent("@", ":");
           return ConvertSQLTOCSharp(s);
        }
        private static string ConvertSQLTOCSharp(string s)
        {
          bool isSQL=  Regex.IsMatch(s, "^[A-Z_]*$");
            if (isSQL)
            {
                var res= s.ToTitleCase('_');
                return res.Replace("_", string.Empty);
            }
            else
            {
                return s;
            }
        }
        private static string ConvertType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    return "string";
                case DbType.Int32:
                    return "int";
                case DbType.Int16:
                    return "short";
                case DbType.Int64:
                    return "long";
                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                    return "DateTime";
                default:
                    return dbType.ToString();
            }
        }
    }
    public static class Ext
    {
        public static string RemoveLeftIfPresent(this string s, params string[] toRemove)
        {
            foreach (var str in toRemove)
            {
                if (s.StartsWith(str))
                {
                    s = s.Substring(str.Length);
                }
            }
            return s;
        }
        public static string ToTitleCase(this string s, params char[] separators)
        {
            bool upnext=true;
            var b=new StringBuilder(s.Length);
            foreach (var ch in s)
            {
                b.Append(upnext ? ch.ToString().ToUpper() : ch.ToString().ToLower());
                upnext = separators.Any(x => x == ch);
            }
            return b.ToString();
        }
    }
