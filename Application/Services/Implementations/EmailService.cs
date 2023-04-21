using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
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
