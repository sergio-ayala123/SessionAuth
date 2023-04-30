using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;

namespace SessionFinalProject.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly UserContext userContext;
        private readonly ISessionValidator sessionValidator;
        public SignUpModel(UserContext userContext, ISessionValidator sessionValidator)
        {
            this.userContext = userContext;
            this.sessionValidator = sessionValidator;
        }
        public async Task OnGet(string message)
        {
            var sessionId = Request.Cookies["sessionId"];
            var hasValidSession = await sessionValidator.ValidateSession(sessionId);
            if (hasValidSession == false)
            {
                Response.Cookies.Append("sessionId", " ");
            }

            Message = message;
        }
        public string Message { get; set; }
        public async Task<IActionResult> OnPost(string email)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 2525;
            client.Host = "localhost";
            if (userContext.Users.Any(u => u.Email == email))
            {
                var message = new MailMessage();
                message.From = new MailAddress("no-reply@localhost");
                message.To.Add(new MailAddress(email));
                message.Subject = "Sign up";
                message.Body = $"Someone tried to signup with this email. Was this you?";
                client.Send(message);
            }
            else
            {
                var link = new SignUpCode
                {
                    Email = email,
                    ExpiresOn = DateTime.Now.AddMinutes(5),
                    Code = Guid.NewGuid().ToString()
                };
                userContext.SignUpCodes.Add(link);
                await userContext.SaveChangesAsync();
                var message = new MailMessage();
                message.From = new MailAddress("no-reply@localhost");
                message.To.Add(new MailAddress(email));
                message.Subject = "Sign up";
                message.Body = $"Click this <a href= 'https://localhost:7114/createaccount?code={link.Code}'> Signup Link </a> to complete the signup process";
                message.IsBodyHtml = true;
                client.Send(message);
            }
            return RedirectToPage("/signup", new { message = "Check your email for a link to create an account." });
        }
    }
}