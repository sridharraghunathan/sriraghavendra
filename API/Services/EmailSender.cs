using System;
using System.Collections.Generic;
using System.IO;
using API.Entities;
using API.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace API.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public void SendEmail<T>(T message, string password, string fromEmail)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage, password, fromEmail);
        }

        private MimeMessage CreateEmailMessage<T>(T message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            var type = typeof(T);
            var Toproperty = type.GetProperty("To");
            var Subjectproperty = type.GetProperty("Subject");
            emailMessage.To.AddRange((List<MailboxAddress>)Toproperty.GetValue(message));
            emailMessage.Subject = (string)Subjectproperty.GetValue(message);

            // READ THE DATA FROM HTML
            string Content = ReadFile(message);
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = Content };
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage, string password = null, string fromEmail = null)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.CheckCertificateRevocation = false;
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    if (string.IsNullOrEmpty(fromEmail))
                    {
                        client.Authenticate(_emailConfig.From, password);
                    }
                    else
                    {
                        client.Authenticate(fromEmail, password);
                    }
                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private static string ReadFile<T>(T message)
        {
            string templatedata = "";
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Emailtemplates");
            if (env == "Production")
            {
                filePath = Path.Combine(Directory.GetCurrentDirectory(), @"../API/", "Emailtemplates");
            }
            var type = typeof(T);
            string AmountReceivedValue, SevaNameValue, AddressValue, FullNameValue;
            var TypeValue = type.GetProperty("Type").GetValue(message);

            switch (TypeValue)
            {
                case CommonItem.EmailFlow.AradhanaAmountReceived:
                    filePath = Path.Combine(filePath, "amountReceived.html");
                    using (StreamReader file = File.OpenText(filePath))
                    {
                        templatedata = file.ReadToEnd().ToString();
                    }
                    EmailProp(message, type, out AmountReceivedValue, out SevaNameValue, out AddressValue, out FullNameValue);
                    templatedata = templatedata.Replace("{fullName}", FullNameValue)
                                 .Replace("{amount}", AmountReceivedValue)
                                 .Replace("{sevaname}", SevaNameValue)
                                 .Replace("{address}", AddressValue);
                    break;

                case CommonItem.EmailFlow.Donation:
                    filePath = Path.Combine(filePath, "donation.html");
                    using (StreamReader file = File.OpenText(filePath))
                    {
                        templatedata = file.ReadToEnd().ToString();
                    }
                    EmailProp(message, type, out AmountReceivedValue, out SevaNameValue, out AddressValue, out FullNameValue);
                    templatedata = templatedata.Replace("{fullName}", FullNameValue)
                                .Replace("{amount}", AmountReceivedValue)
                                .Replace("{sevaname}", SevaNameValue)
                                .Replace("{address}", AddressValue);
                    break;
                case CommonItem.EmailFlow.Clarification:
                    filePath = Path.Combine(filePath, "clarification.html");
                    using (StreamReader file = File.OpenText(filePath))
                    {
                        templatedata = file.ReadToEnd().ToString();
                    }


                    string QuestionValue = type.GetProperty("Question").GetValue(message).ToString();
                    string ReplyValue = type.GetProperty("Reply").GetValue(message).ToString();
                    FullNameValue = type.GetProperty("FullName").GetValue(message).ToString();

                    templatedata = templatedata.Replace("{fullName}", FullNameValue)
                                .Replace("{QuestionFromUser}", QuestionValue)
                                .Replace("{ReplyFromTrust}", ReplyValue);
                    break;


                default:
                    templatedata = "Thanks for your Valuable contribution, Incase of questions please contact us.";
                    break;
            }
            return templatedata;

        }

        private static void EmailProp<T>(T message, System.Type type, out string AmountReceivedValue,
        out string SevaNameValue,
        out string AddressValue, out string FullNameValue)
        {
            AmountReceivedValue = type.GetProperty("AmountReceived").GetValue(message).ToString();
            SevaNameValue = type.GetProperty("SevaName").GetValue(message).ToString();
            AddressValue = type.GetProperty("Address").GetValue(message).ToString();
            FullNameValue = type.GetProperty("FullName").GetValue(message).ToString();
        }
    }
}