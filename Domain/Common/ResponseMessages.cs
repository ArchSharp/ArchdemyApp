using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public static class ResponseMessages
    {
        public const string NewUserCreated = "User created successfully";
        public const string LoginSuccessful = "Login successfully";
        public const string UserNotFound = "User not found";
        public const string UserAlreadyExist = "User already exist";
        public const string ForgotPasswordLinkSent = "Forgot password link has been sent to your email";
    }
}
