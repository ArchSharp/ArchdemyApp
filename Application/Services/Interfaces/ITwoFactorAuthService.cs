using Application.DTOs;
using Application.Helpers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ITwoFactorAuthService : IAutoDependencyService
    {
        Task<bool> ValidateTwoFactorPin(string email, string twoFactorCode);
        Task<SuccessResponse<GoogleTwoFactorAuthResponse>> GenerateNewSecretKey(string email);
    }
}
