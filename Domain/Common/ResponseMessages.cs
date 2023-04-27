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
        public const string ResetSuccessful = "Password has been reset successfully";
        public const string VerifyEmail = "Please verify your email";
        public const string StripeCustomerCreated = "Stripe customer has been created";
        public const string StripePaymentSuccess = "Stripe payment successful";
        public const string UserEmailNotVerified = "Please click on the email verification link in your mail";
        public const string VerifiedEmail = "Your email is now verified";
        public const string InvalidRefreshToken = "Invalid Refresh Token";
        public const string InvalidToken = "Invalid token";
        public const string TooManyRequest = "Too many request, please try again in next three minutes";
        public const string RenewedToken = "Token is now renewed";
        public const string PasswordChangedSuccessful = "Password has been changed successfully";
        public const string UserNotFound = "User not found";
        public const string InCorrectPassword = "Wrong password";
        public const string UserAlreadyExist = "User already exist";
        public const string ForgotPasswordLinkSent = "Forgot password link has been sent to your email";
        public const string NewCourseCreated = "Course created successfully";
        public const string CourseNotFound = "Course with this id cannot be found";
        public const string CourseUpdated = "Course updated successfully";
        public const string CourseFetchedSuccesss = "Course fetched successfully";
        public const string CourseModuleNotFound = "Course module not found";
    }
}
