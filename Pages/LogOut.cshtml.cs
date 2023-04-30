using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SessionFinalProject.Pages
{
    public class LogOutModel : PageModel
    {
        private readonly UserContext userContext;
        public LogOutModel(UserContext userContext)
        {
            this.userContext = userContext;
        }
        public async Task<IActionResult> OnGet()
        {
            var sessionToRemove = userContext.Sessions.FirstOrDefault(s => s.SessionId == Request.Cookies["sessionId"]);
            if (sessionToRemove != null)
            {
                Response.Cookies.Append("sessionId", " ");
                userContext.Sessions.Remove(sessionToRemove);
                await userContext.SaveChangesAsync();
            }
            return RedirectToPage("/index");
        }
    }
}
