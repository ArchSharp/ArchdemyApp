using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Stripe
{
    public class StripeSettings
    {
        public string PublishableKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
    }
}
