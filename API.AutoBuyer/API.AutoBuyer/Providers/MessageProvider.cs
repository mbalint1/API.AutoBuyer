using System;
using System.Net;
using System.Net.Mail;
using AutoBuyer.API.Core.Utilities;

namespace AutoBuyer.API.Providers
{
    public class MessageProvider : IMessageProvider
    {
        public void Send(string subject, string body, string emailTo)
        {
            try
            {
                var client = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(ConnectionUtility.GetFromEmail(),
                        ConnectionUtility.GetEmailPassword())
                };
                client.Send(new MailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = { emailTo }
                });

                //var client = new SmtpClient("smtp.gmail.com", 587)
                //{
                //    UseDefaultCredentials = false,
                //    Credentials = new NetworkCredential(ConnectionUtility.GetFromEmail(), ConnectionUtility.GetEmailPassword()),
                //    EnableSsl = true
                //};

                //client.Send(ConnectionUtility.GetFromEmail(), emailTo, subject, body);
            }
            catch (Exception ex)
            {
                //TODO: Logging
            }
        }
    }
}