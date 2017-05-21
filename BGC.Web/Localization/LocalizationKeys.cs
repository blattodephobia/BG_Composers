using System;

namespace BGC.Web.LocalizationKeys
{
    public class Administration
{
    public const string Name = "Administration.Name";
    public class Edit
    {
            public class List
            {
                        public const string Add_entry = "Administration.Edit.List.Add_entry";
}
}
public class Account
{
        public class Activities
        {
                    public const string SendInvite = "Administration.Account.Activities.SendInvite";
                    public const string UserSettings = "Administration.Account.Activities.UserSettings";
                    public const string ApplicationSettings = "Administration.Account.Activities.ApplicationSettings";
}
public class RequestPasswordReset
{
            public const string ResetPassword = "Administration.Account.RequestPasswordReset.ResetPassword";
            public const string BackToLogin = "Administration.Account.RequestPasswordReset.BackToLogin";
            public const string PasswordResetEmailSent = "Administration.Account.RequestPasswordReset.PasswordResetEmailSent";
            public const string TypeEmailForPasswordReset = "Administration.Account.RequestPasswordReset.TypeEmailForPasswordReset";
}
public class ResetPassword
{
            public const string UnknownEmailError = "Administration.Account.ResetPassword.UnknownEmailError";
            public const string UnknownError = "Administration.Account.ResetPassword.UnknownError";
}
public class ChangePassword
{
            public const string PasswordTooShort = "Administration.Account.ChangePassword.PasswordTooShort";
            public const string ChangePasswordTitle = "Administration.Account.ChangePassword.ChangePasswordTitle";
            public const string PasswordsMismatch = "Administration.Account.ChangePassword.PasswordsMismatch";
            public const string UnknownError = "Administration.Account.ChangePassword.UnknownError";
            public const string WrongPassword = "Administration.Account.ChangePassword.WrongPassword";
            public const string ChangePasswordLabel = "Administration.Account.ChangePassword.ChangePasswordLabel";
            public const string CurrentPassword = "Administration.Account.ChangePassword.CurrentPassword";
            public const string NewPassword = "Administration.Account.ChangePassword.NewPassword";
            public const string ConfirmNewPassword = "Administration.Account.ChangePassword.ConfirmNewPassword";
            public const string PasswordRequired = "Administration.Account.ChangePassword.PasswordRequired";
}
}
public class UserManagement
{
        public const string UserName = "Administration.UserManagement.UserName";
        public class Register
        {
                    public const string UserNameRequired = "Administration.UserManagement.Register.UserNameRequired";
                    public const string UserNameInUse = "Administration.UserManagement.Register.UserNameInUse";
}
public class SendInvite
{
            public const string Invitation_sent = "Administration.UserManagement.SendInvite.Invitation_sent";
            public const string Roles = "Administration.UserManagement.SendInvite.Roles";
            public const string UserExists = "Administration.UserManagement.SendInvite.UserExists";
}
}
public class Authentication
{
        public class Login
        {
                    public const string LoginButton = "Administration.Authentication.Login.LoginButton";
                    public const string Username = "Administration.Authentication.Login.Username";
                    public const string Password = "Administration.Authentication.Login.Password";
                    public const string Authentication_failure = "Administration.Authentication.Login.Authentication_failure";
                    public const string Forgot_password = "Administration.Authentication.Login.Forgot_password";
                    public const string RememberMe = "Administration.Authentication.Login.RememberMe";
}
}
}
public class Public
{
    public class Main
    {
            public const string SearchCaption = "Public.Main.SearchCaption";
            public class Search
            {
                        public const string NoResults = "Public.Main.Search.NoResults";
}
}
}
public class Global
{
    public const string Send = "Global.Send";
    public const string InvitationEmailFormat = "Global.InvitationEmailFormat";
    public const string Email = "Global.Email";
    public const string Ok = "Global.Ok";
    public const string Submit = "Global.Submit";
    public const string Name = "Global.Name";
    public const string Logout = "Global.Logout";
}
}
