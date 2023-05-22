using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PayStack
{
    public class PayStackPayment
    {
        public string Email { get; set; } = null!;
        public int Amount { get; set; }
        public string Name { get; set; } = null!;
    }

    public class PayStackPaymentInitializeResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = null!;
        public object Data { get; set; } = null!;
    }

    public class Data
    {
        public string AuthorizationUrl { get; set; } = null!;
        public string AccessCode { get; set; } = null!; 
        public string Reference { get; set; } = null!;
    }
}
