using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace SessionFinalProject.Pages
{
    public class LogInModel : PageModel
    {
        private readonly UserContext userContext;
        private readonly ISessionValidator sessionValidator;
        public string message = "";
        public bool hasActiveSession = false;
        public LogInModel(UserContext userContext, ISessionValidator sessionValidator)
        {
            this.userContext = userContext;
            this.sessionValidator = sessionValidator;
        }
        public async Task<IActionResult> OnGet(string message)
        {
            this.message = message;
            var sessionId = Request.Cookies["sessionId"];
            var hasValidSession = await sessionValidator.ValidateSession(sessionId);
            if (hasValidSession == false)
            {
                Response.Cookies.Append("sessionId", " ");
            }
            else
            {
                hasActiveSession = true;
            }
            return Page();
        }



        public async Task<IActionResult> OnPost(string email, string password)
        {
            
            var user = userContext.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                var result = PasswordHasher.HashPasword(password, user.Salt);
                if (result == user.HashedPassword)
                {
                    var sessionId = Guid.NewGuid().ToString();
                    Response.Cookies.Append("sessionId", sessionId);
                    await userContext.Sessions.AddAsync(new Session { UserId = user.Id, SessionId = sessionId, ExpiresOn = DateTime.Now.AddMinutes(60) });
                    await userContext.SaveChangesAsync();
                    return RedirectToPage("/index");
                }
                else
                {
                    message = "There was an error logging in";
                    return RedirectToPage("/login", new { message = "There was an error logging in" });
                }
            }
            else
            {
                message = "There was an error logging in";
                return RedirectToPage("/login", new { message = "There was an error logging in" });

            }
            
        }
    }
}
