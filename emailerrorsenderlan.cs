    public static class StringExtensions
    {
        public static string Repeat(this string input, int count)
        {
            if (input == null)
            {
                return null;
            }

            var sb = new StringBuilder();

            for (var repeat = 0; repeat < count; repeat++)
            {
                sb.Append(input);
            }

            return sb.ToString();
        }
    }
    class EmailErrorSender
    {
        private readonly string _exchangeserver;
        private readonly string _appName;
        private readonly string _mailto;
        private readonly string _mailfrom;

        public EmailErrorSender(string exchangeserver, string appName,string mailto,string mailfrom)
        {
            _exchangeserver = exchangeserver;
            _appName = appName;
            _mailto = mailto;
            _mailfrom = mailfrom;
        }

        public void Write(Exception ex)
        {
            var smtpClient = new SmtpClient();
            var message = new MailMessage();
            MailAddress fromAddress = new MailAddress(_mailfrom);
            smtpClient.Host = _exchangeserver;
            smtpClient.Port = 25;
            smtpClient.UseDefaultCredentials = true;
            message.From = fromAddress;
            message.To.Add(_mailto);
            var body = getException(ex);
            message.Body = body;
            message.Subject =string.Format("{0} error", _appName);
            message.IsBodyHtml = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Send(message);
        }
        string getException(Exception ex,int indent=0)
        {
              string body = string.Empty;
            string ind = "\t".Repeat(indent);
            body += string.Format(ind+"ex.message = {0}\n", ex.Message);
            body += string.Format(ind+"ex.stacktrace = {0}\n", ex.StackTrace);
            body += string.Format(ind+"ex.source = {0}\n", ex.Message);
            if (ex.InnerException != null)
            {
                body += "inner exception:\n";
                body += getException(ex,indent+1);

            }
            return body;
        }

      
    }
