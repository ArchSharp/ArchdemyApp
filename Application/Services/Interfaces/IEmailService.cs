using Application.DTOs;
using Application.Helpers;
using Domain.Entities;
using Identity.Data.Dtos.Request.MessageBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IEmailService : IAutoDependencyService
    {
        void SendEmailUsingMailKit(EmailRequest email);
        //Task<SuccessResponse<object>> SendEmailUsingSendGrid(Email email);
        string GetEmailTemplate<T>(string emailTemplate, T emailTemplateModel);        

    }
}
