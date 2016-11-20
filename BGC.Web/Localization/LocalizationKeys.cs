using System;

namespace BGC.Web.LocalizationKeys
{
    public class Administration
{
    public class Edit
    {
            public class List
            {
                        public static readonly string Add_entry = "Administration.Edit.List.Add_entry";
}
}
public class Account
{
        public class Activities
        {
                    public static readonly string SendInvite = "Administration.Account.Activities.SendInvite";
                    public static readonly string UserSettings = "Administration.Account.Activities.UserSettings";
                    public static readonly string ApplicationSettings = "Administration.Account.Activities.ApplicationSettings";
}
public class RequestPasswordReset
{
            public static readonly string PasswordResetEmailSent = "Administration.Account.RequestPasswordReset.PasswordResetEmailSent";
            public static readonly string TypeEmailForPasswordReset = "Administration.Account.RequestPasswordReset.TypeEmailForPasswordReset";
}
public class PasswordReset
{
            public static readonly string UnknownEmailError = "Administration.Account.PasswordReset.UnknownEmailError";
            public static readonly string UnknownError = "Administration.Account.PasswordReset.UnknownError";
}
}
public class UserManagement
{
        public class SendInvite
        {
                    public static readonly string Invitation_sent = "Administration.UserManagement.SendInvite.Invitation_sent";
                    public static readonly string Roles = "Administration.UserManagement.SendInvite.Roles";
}
}
public class Authentication
{
        public class Login
        {
                    public static readonly string Authentication_failure = "Administration.Authentication.Login.Authentication_failure";
                    public static readonly string Forgot_password = "Administration.Authentication.Login.Forgot_password";
}
}
}
public class Public
{
}
public class Global
{
    public static readonly string Email = "Global.Email";
    public static readonly string Ok = "Global.Ok";
    public static readonly string Submit = "Global.Submit";
    public static readonly string Name = "Global.Name";
    public static readonly string Logout = "Global.Logout";
}
}
