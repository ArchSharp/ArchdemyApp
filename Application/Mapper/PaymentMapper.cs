using AutoMapper;
using Domain.Entities.PayStack;
using PayStack.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class PaymentMapper : Profile
    {
        public PaymentMapper()
        {
            CreateMap<TransactionInitializeResponse, PayStackPaymentInitializeResponse>().ReverseMap();
        }
    }
}
