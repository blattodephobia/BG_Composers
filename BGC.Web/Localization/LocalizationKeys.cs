using System;

namespace BGC.Web.LocalizationKeys
{
    public class Administration
{
    public const string Name = "Administration.Name";
    public class Edit
    {
            public class Add
            {
                        public const string AddEntry = "Administration.Edit.Add.AddEntry";
                        public const string NamePromptFormat = "Administration.Edit.Add.NamePromptFormat";
                        public const string UploadImagesPrompt = "Administration.Edit.Add.UploadImagesPrompt";
}
public class List
{
            public const string AddEntry = "Administration.Edit.List.AddEntry";
            public const string PublishedArticles = "Administration.Edit.List.PublishedArticles";
}
public class Update
{
            public const string UpdateArticle = "Administration.Edit.Update.UpdateArticle";
            public const string ExplainOrder = "Administration.Edit.Update.ExplainOrder";
}
}
public class Glossary
{
        public class ListGlossary
        {
                    public const string Glossary = "Administration.Glossary.ListGlossary.Glossary";
                    public const string TermName = "Administration.Glossary.ListGlossary.TermName";
                    public const string InOtherLanguages = "Administration.Glossary.ListGlossary.InOtherLanguages";
                    public const string Definition = "Administration.Glossary.ListGlossary.Definition";
                    public const string ConfirmDelete = "Administration.Glossary.ListGlossary.ConfirmDelete";
                    public const string ErrorWhileDeleting = "Administration.Glossary.ListGlossary.ErrorWhileDeleting";
}
public class Edit
{
            public const string AddEntry = "Administration.Glossary.Edit.AddEntry";
            public const string EditEntry = "Administration.Glossary.Edit.EditEntry";
}
}
public class Account
{
        public class Activities
        {
                    public const string WelcomeToAdminArea = "Administration.Account.Activities.WelcomeToAdminArea";
                    public const string PublishedArticles = "Administration.Account.Activities.PublishedArticles";
                    public const string Statistics = "Administration.Account.Activities.Statistics";
                    public const string SendInvite = "Administration.Account.Activities.SendInvite";
                    public const string ContentManagement = "Administration.Account.Activities.ContentManagement";
                    public const string UserSettings = "Administration.Account.Activities.UserSettings";
                    public const string ApplicationSettings = "Administration.Account.Activities.ApplicationSettings";
                    public const string GlossaryManagement = "Administration.Account.Activities.GlossaryManagement";
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
            public const string ResetPasswordLabel = "Administration.Account.ResetPassword.ResetPasswordLabel";
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
                    public const string RegisterTitle = "Administration.UserManagement.Register.RegisterTitle";
                    public const string AvailableRoles = "Administration.UserManagement.Register.AvailableRoles";
                    public const string FillOutFormPrompt = "Administration.UserManagement.Register.FillOutFormPrompt";
                    public const string RegisterNewUser = "Administration.UserManagement.Register.RegisterNewUser";
                    public const string UserNameRequired = "Administration.UserManagement.Register.UserNameRequired";
                    public const string UserNameInUse = "Administration.UserManagement.Register.UserNameInUse";
                    public const string ConfirmPassword = "Administration.UserManagement.Register.ConfirmPassword";
}
public class SendInvite
{
            public const string Invitation_sent = "Administration.UserManagement.SendInvite.Invitation_sent";
            public const string SendInvitationTitle = "Administration.UserManagement.SendInvite.SendInvitationTitle";
            public const string ChooseRoles = "Administration.UserManagement.SendInvite.ChooseRoles";
            public const string UserExists = "Administration.UserManagement.SendInvite.UserExists";
}
}
public class Authentication
{
        public class Login
        {
                    public const string LoginLabel = "Administration.Authentication.Login.LoginLabel";
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
                        public const string SearchResults = "Public.Main.Search.SearchResults";
                        public const string SearchAgain = "Public.Main.Search.SearchAgain";
}
public class Index
{
            public const string NoArticles = "Public.Main.Index.NoArticles";
            public const string Welcome = "Public.Main.Index.Welcome";
            public const string OrBrowse = "Public.Main.Index.OrBrowse";
            public const string InOtherLanguages = "Public.Main.Index.InOtherLanguages";
}
public class Read
{
            public const string LanguagesSelectionCaption = "Public.Main.Read.LanguagesSelectionCaption";
}
}
}
public class Global
{
    public const string ApplicationName = "Global.ApplicationName";
    public const string Error = "Global.Error";
    public const string ErrorOccurredDuringRequest = "Global.ErrorOccurredDuringRequest";
    public const string Forbidden = "Global.Forbidden";
    public const string InternalServerError = "Global.InternalServerError";
    public const string Welcome = "Global.Welcome";
    public const string Save = "Global.Save";
    public const string Publish = "Global.Publish";
    public const string Send = "Global.Send";
    public const string InvitationEmailFormat = "Global.InvitationEmailFormat";
    public const string Email = "Global.Email";
    public const string Ok = "Global.Ok";
    public const string Submit = "Global.Submit";
    public const string Name = "Global.Name";
    public const string Logout = "Global.Logout";
}
}
