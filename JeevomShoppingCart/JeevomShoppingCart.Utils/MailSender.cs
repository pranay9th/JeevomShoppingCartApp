using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JeevomShoppingCart.Utils
    {
    public class MailSender
        {
       public static int SendMail(MailMessage objMailMsg)
            {
            int flag_sendmail = 0;
            // Create the mail message            
            string HostAddress = "";
            SmtpClient mailClient = new SmtpClient();
            mailClient.Host = HostAddress;
            mailClient.Send(objMailMsg);
            flag_sendmail = 1;
            return flag_sendmail;
            }
        }
    }
