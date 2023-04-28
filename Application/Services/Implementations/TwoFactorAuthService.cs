﻿using Application.DTOs;
using Application.Services.Interfaces;
using Google.Authenticator;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Utilities;
using System.Security.Cryptography;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Infrastructure.Repositories.Interfaces;
using Application.Helpers;
using Domain.Common;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net;
using static QRCoder.PayloadGenerator;
using MailKit.Net.Smtp;

namespace Application.Services.Implementations
{
    public class TwoFactorAuthService : ITwoFactorAuthService
    {
        private readonly GoogleTwoFactorAuthSettings _googleTwoFactorAuthSettings;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public TwoFactorAuthService(
            IOptions<GoogleTwoFactorAuthSettings> googleTwoFactorAuthSettings,
            IRepository<User> userRepository,
            IMapper mapper
        )
        {
            _googleTwoFactorAuthSettings = googleTwoFactorAuthSettings.Value;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> ValidateTwoFactorPin(string email, string twoFactorCode)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.Email == email);

            if (findUser == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);

            var tfa = new TwoFactorAuthenticator();
            var response = tfa.ValidateTwoFactorPIN(findUser.TwoFactorSecretKey, twoFactorCode, true);

            if (!findUser.IsTwoFactorEnabled && response == true)
            {
                findUser.IsTwoFactorEnabled = response;
                await _userRepository.SaveChangesAsync();
            }

            return response;
        }

        public async Task<SuccessResponse<GoogleTwoFactorAuthResponse>> GenerateNewSecretKey(string email)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.Email == email);

            if (findUser == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);

            string UserUniqueKey = ConvertToBase32(_googleTwoFactorAuthSettings.SecretKey + email);
            findUser.TwoFactorSecretKey = UserUniqueKey;
            await _userRepository.SaveChangesAsync();

            var tfa = new TwoFactorAuthenticator();
            var setupCode = tfa.GenerateSetupCode("ArchDemy Application", email, UserUniqueKey, true, 3);
            
            Console.WriteLine(setupCode.QrCodeSetupImageUrl);
            
            var setupResponse = new GoogleTwoFactorAuthResponse();
            setupResponse.Account = setupCode.Account;
            setupResponse.ManualEntryKey = setupCode.ManualEntryKey;
            setupResponse.QrCodeSetupImageUrl = setupCode.QrCodeSetupImageUrl;

           
            return new SuccessResponse<GoogleTwoFactorAuthResponse>
            {
                Data = setupResponse,
                code = 201,
                Message = ResponseMessages.NewUserCreated,
                ExtraInfo = "",
            };
        }

        private string GenerateBase32SecretKey(int byteLength)
        {
            var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[byteLength];
            rng.GetBytes(bytes);
            return Base32Encoding.ToString(bytes);
        }
        public string ConvertToBase32(string input)
        {
            var bytes = Encoding.ASCII.GetBytes(input);
            return Base32Encoding.ToString(bytes);
        }
    }
}
