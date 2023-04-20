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
