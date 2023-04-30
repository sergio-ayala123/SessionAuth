using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SessionFinalProject.Pages
{
    public class SessionsModel : PageModel
    {
        private readonly UserContext userContext;
        public List<activeSessions> activeSessions { get; set; } = new List<activeSessions>();
        public bool isAdmin { get; set; }
        public SessionsModel(UserContext userContext)
        {
            this.userContext = userContext;
        }
        public void OnGet()
        {
            var sessions = userContext.Sessions.ToList();
            var users = userContext.Users.ToList();

            if(sessions.Count > 0)
            {
                var currentUserId = sessions.FirstOrDefault(u => u.SessionId == Request.Cookies["sessionId"]).UserId;
                var currentUser = users.FirstOrDefault(s => s.Id == currentUserId);
                if(currentUser.Role == "admin")
                {
                    isAdmin = true;
                }

                foreach(var session in sessions)
                {
                    activeSessions.Add(new activeSessions { SessionId=session.SessionId, Email=users.FirstOrDefault(u=> u.Id == session.UserId).Email, ExpiresOn = session.ExpiresOn });
                }
            }
        }

        public async Task<IActionResult> OnPostDeleteSession(string sessionId)
        {
            if(sessionId == Request.Cookies["sessionId"])
            {
                Response.Cookies.Append("sessionId", " ");
            }
            var sessionToRemove = userContext.Sessions.FirstOrDefault(s => s.SessionId == sessionId);
            userContext.Sessions.Remove(sessionToRemove);
            await userContext.SaveChangesAsync();
            return RedirectToPage("/Sessions");
        }

    }

    public struct activeSessions
    {
        public string SessionId;
        public DateTime ExpiresOn;
        public string Email;
    }
}
