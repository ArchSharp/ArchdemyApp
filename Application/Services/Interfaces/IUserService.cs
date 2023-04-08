using Application.DTOs;
using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUserService : IAutoDependencyService
    {
        Task<SuccessResponse<CreateUserDto>> CreateUser(CreateUserDto model);
        Task<SuccessResponse<CreateUserDto>> LoginUser(LoginUserDto model);
        Task<SuccessResponse<ForgotPasswordDto>> ForgotPassword(ForgotPasswordDto model);
    }
}
