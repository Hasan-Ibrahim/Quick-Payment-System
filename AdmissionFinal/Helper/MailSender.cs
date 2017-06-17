using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace AdmissionFinal.Helper
{
    public class MailSender
    {
                string to;
                string message;
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                
                public MailSender(string to, string message)
                {
                    this.to = to;
                    this.message = message;
                    mailMessage.From = new MailAddress("bit0213@iit.du.ac.bd");
                    mailMessage.To.Add(new MailAddress(to));
                    mailMessage.Subject = "Username and Password";
                    mailMessage.Body = message;
                    mailMessage.IsBodyHtml = true;
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("bit0213@iit.du.ac.bd", "darkpassenger");
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);
                }
    }
}