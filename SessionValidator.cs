namespace SessionFinalProject
{
    public interface ISessionValidator
    {
        public Task<bool> ValidateSession(string sessionId);
    }

    public class SessionValidator : ISessionValidator
    {
        private readonly UserContext userContext;
        public SessionValidator(UserContext userContext)
        {
            this.userContext = userContext;
        }


        public async Task<bool> ValidateSession(string sessionId)
        {
            var currentSession = userContext.Sessions.FirstOrDefault(s=> s.SessionId == sessionId);
            if(currentSession != null)
            {
                if(currentSession.ExpiresOn > DateTime.Now)
                {
                    return true;
                }
                else
                {
                    userContext.Sessions.Remove(currentSession);
                    await userContext.SaveChangesAsync();
                    return false;
                }
            }
            else
            {
                return false;
            }

            
        }


    }
}
