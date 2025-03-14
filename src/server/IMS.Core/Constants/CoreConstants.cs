using System;

namespace IMS.Core.Constants;

public class CoreConstants
{

    public static Guid AdminId = Guid.Parse("f894a1cc-c3fa-47c4-aa63-16a9742816b8");
    public struct Schemas
    {
        public const string Common = "Common";
        public const string Security = "Security";
    }

    public struct UserRoles
    {
        public const string Recruiter = "recruiter";
        public const string Manager = "manager";
        public const string Interviewer = "interviewer";
        public const string Admin = "admin";



    }
}
