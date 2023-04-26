using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using RazorEngineCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSender _sender;

        public EmailService(IOptions<EmailSender> sender)
        {
            _sender = sender.Value;
        }

        public string GetEmailTemplate<T>(string emailTemplate, T emailTemplateModel)
        {
            string mailTemplate = LoadTemplate(emailTemplate);

            IRazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate modifiedMailTemplate = razorEngine.Compile(mailTemplate);

            return modifiedMailTemplate.Run(emailTemplateModel);
        }

        public string LoadTemplate(string emailTemplate)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string upThreeLevels = Path.Combine(baseDir, "..\\..\\..\\..\\");
            string templateDir = Path.Combine(upThreeLevels, "Application/Files/MailTemplates");
            string templatePath = Path.Combine(templateDir, $"{emailTemplate}.html");

            using FileStream fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);

            string mailTemplate = streamReader.ReadToEnd();
            streamReader.Close();

            return mailTemplate;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var emailObject = new MimeMessage();
            emailObject.From.Add(MailboxAddress.Parse(_sender.Email));
            emailObject.To.Add(MailboxAddress.Parse(to));
            emailObject.Subject = subject;
            emailObject.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect(_sender.Host, _sender.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_sender.Email, _sender.Password);
            smtp.Send(emailObject);
            smtp.Disconnect(true);
        }        
    }
}
