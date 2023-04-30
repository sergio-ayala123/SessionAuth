using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SessionFinalProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISessionValidator sessionValidator;
        private readonly UserContext userContext;
        public string loggedInUser { get; set; } = "";
        public bool HasValidSession { get; set; } = false;

        public IndexModel(ILogger<IndexModel> logger, ISessionValidator sessionValidator, UserContext userContext)
        {
            _logger = logger;
            this.sessionValidator = sessionValidator;
            this.userContext = userContext;
        }

        public async Task OnGet()
        {
            var sessionId = Request.Cookies["sessionId"];
            var hasValidSession = await sessionValidator.ValidateSession(sessionId);


            if(hasValidSession)
            {
                var currentSession = userContext.Sessions.FirstOrDefault(s => s.SessionId == sessionId);
                var user = userContext.Users.FirstOrDefault(u => u.Id == currentSession.UserId);
                loggedInUser = user.Email;
                if(user.Role == "admin")
                {
                    ViewData["admin"] = "true";
                }
            }
            else
            {
                Response.Cookies.Append("sessionId", " ");
            }
        }
    }
}