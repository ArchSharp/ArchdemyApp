using Application.DTOs;
using Application.Helpers;
using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUserService : IAutoDependencyService
    {
        Task<SuccessResponse<GetUserDto>> Register(CreateUserDto model);
        Task<SuccessResponse<GetUserDto>> Login(LoginUserDto model);
        Task<SuccessResponse<ForgotPasswordDto>> ForgotPassword(ForgotPasswordDto model);
        Task<SuccessResponse<CreateUserDto>> ResetPassword(ResetPasswordDto model);
        Task<SuccessResponse<CreateUserDto>> ChangePassword(ChangePasswordDto model);
        Task<SuccessResponse<CreateUserDto>> VerifyEmail(string email, string token);
        Task<SuccessResponse<UpdateUserDto>> UpdateUser(string email, UpdateUserDto model);
    }
}
