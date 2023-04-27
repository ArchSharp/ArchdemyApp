using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PayStack
{
    public class PayStackPayment
    {
        public string Email { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }        
    }

    public class PayStackPaymentInitializeResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }            
    }

    public class Data
    {
        public string AuthorizationUrl { get; set; }
        public string AccessCode { get; set; }
        public string Reference { get; set; }
    }
}
