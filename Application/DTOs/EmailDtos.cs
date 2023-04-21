using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class EmailDtos
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }

    public class EmailSender
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
