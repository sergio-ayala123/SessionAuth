using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace SessionFinalProject.Pages
{
    public class CreateAccountModel : PageModel
    {
        private readonly UserContext userContext;
        public string email = "";
        public CreateAccountModel(UserContext userContext)
        {
            this.userContext = userContext;
        }


        public void OnGet(string code)
        {
            var signUpCodeRecord = userContext.SignUpCodes.FirstOrDefault(c=> c.Code == code);
            email = signUpCodeRecord.Email;
        }

        public async Task<IActionResult> OnPost(string password, string email) 
        {
            
            var hash = PasswordHasher.HashPasword(password, out var salt);
            var saltLength = salt.Length;
            await userContext.Users.AddAsync(new User { Email = email, Salt = Convert.ToHexString(salt), HashedPassword = hash, Role = "default"});
            await userContext.SaveChangesAsync();


            return Redirect("https://localhost:7114/login");
        }


    }
}