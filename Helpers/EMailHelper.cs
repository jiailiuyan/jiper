using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Jiper.Helpers
{
    public class EMailHelper
    {

        public static void SendEMail(string msg)
        {
            var user = "anonymous310@163.com";
            var sendto = "753668790@qq.com";

            var title = "通知报备";

            var smtpclient = new SmtpClient();
            smtpclient.Host = "smtp.163.com";
            smtpclient.Port = 25;
            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpclient.EnableSsl = true;
            smtpclient.UseDefaultCredentials = true;

            smtpclient.Credentials = new NetworkCredential(user, "anonymous310");

            var mailmessage = new MailMessage(user, sendto);

            mailmessage.Subject = title;
            mailmessage.Body = msg;
            mailmessage.BodyEncoding = Encoding.UTF8;
            mailmessage.Priority = MailPriority.Normal;

            smtpclient.Send(mailmessage);
        }
    }
}
