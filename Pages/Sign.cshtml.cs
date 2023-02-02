using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tabler.Pages
{
    public class SignModel : PageModel
    {
        public bool isWaiting = false;
        public bool isSucceed = false;
        
        public string errorMassage = string.Empty;
        public string Email { get; set; } = "Please enter your email address";
        public string Password { get; set; } = "Please enter your password";

        public void OnGet()
        {

        }

        public void OnPostSignIn(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
            Console.WriteLine(
                "New User:\n" +
                $"\t{Email}\n" +
                $"\t{Password}"
                );
            if(DatabaceManager.Instance.AddUser(Email, Password))
            {
                errorMassage = string.Empty;
                isSucceed = true;
            }
            else
            {
                errorMassage = "The email address is already registered.";
            }
        }
    }
}
