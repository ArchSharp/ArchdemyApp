using Application.DTOs;
using Application.Services.Interfaces;
using Identity.Data.Dtos.Request.MessageBroker;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using RazorEngineCore;
using SendGrid;
using SendGrid.Helpers.Mail;
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
        //private readonly ISendGridClient _sendGridClient;
        //private readonly SendGridEmailSettings _sendGridEmailSettings;

        public EmailService(
            IOptions<EmailSender> sender
            //ISendGridClient sendGridClient,
            //IOptions<SendGridEmailSettings> sendGridEmailSettings
            )
        {
            _sender = sender.Value;
            //_sendGridClient = sendGridClient;
            //_sendGridEmailSettings = sendGridEmailSettings.Value;
        }

        public string GetEmailTemplate<T>(string emailTemplate, T emailTemplateModel)
        {
            string mailTemplate = LoadTemplate(emailTemplate);

            IRazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate modifiedMailTemplate = razorEngine.Compile(mailTemplate);

            return modifiedMailTemplate.Run(emailTemplateModel);
        }        
        public void SendEmailUsingMailKit(EmailRequest payload)
        {
            var emailTemplate = LoadTemplate(payload.TemplateName);
            string messageBody = string.Format(emailTemplate, payload.EmailSubject, String.Format("{0:dddd, MMMM d, yyyy} ", DateTime.UtcNow), payload.Variables["name"], payload.ReceiverEmail, payload.Variables["link"]);
           
            var emailObject = new MimeMessage();
            //emailObject.From.Add(MailboxAddress.Parse(_sender.Email));
            emailObject.From.Add(new MailboxAddress("ArchDemy", _sender.Email));
            //emailObject.To.Add(MailboxAddress.Parse(payload.ReceiverEmail));
            emailObject.To.Add(new MailboxAddress(payload.Variables["name"], payload.ReceiverEmail));
            emailObject.Subject = payload.EmailSubject;
            emailObject.Body = new TextPart(TextFormat.Html) { Text = messageBody };

            //port 587 uses SecureSocketOptions.startTls
            //port 465 uses SecureSocketOptions.Auto
            using var smtp = new SmtpClient();
            smtp.Connect(_sender.Host, _sender.Port, SecureSocketOptions.Auto);
            //smtp.Connect(_sender.Host, _sender.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_sender.Email, _sender.Password);
            smtp.Send(emailObject);
            smtp.Disconnect(true);
        }

        /*public async Task<SuccessResponse<object>> SendEmailUsingSendGrid(Email email)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_sendGridEmailSettings.FromEmail, _sendGridEmailSettings.FromName),
                Subject = email.Subject,
                HtmlContent = email.Body
            };

            // add attachment
            await msg.AddAttachmentAsync(
                email.Attachment?.FileName,
                email.Attachment?.OpenReadStream(),
                email.Attachment?.ContentType,
                "attachment"
            );

            msg.AddTo(email.To);
            var response = await _sendGridClient.SendEmailAsync(msg);
            string message = response.IsSuccessStatusCode ? "Email Send Successfully" :
            "Email Sending Failed";
            return new SuccessResponse<object> { Data = message,};
        }*/

        private string LoadTemplate(string emailTemplate)
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
    }
}
