using Application.DTOs;
using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IEmailService : IAutoDependencyService
    {
        void SendEmail(string to, string subject,string body);
    }
}
