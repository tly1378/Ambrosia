using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tabler.Pages
{
    public class SignModel : PageModel
    {
        public bool isWaiting = false;
        
        public string errorMassage = string.Empty;
        public string Email { get; set; } = "Please enter your email address";
        public string Password { get; set; } = "Please enter your password";

        public void OnGet()
        {

        }

        public void OnPostSignIn(string Email, string Password)
        {
            Console.WriteLine(
                "New User:\n" +
                $"\t{Email}\n" +
                $"\t{Password}"
                );
            if(DatabaceManager.Instance.AddUser(Email, Password))
            {
                this.Email = Email;
                this.Password = Password;
                errorMassage = string.Empty;
            }
            else
            {
                this.Email = "Please enter your email address";
                this.Password = "Please enter your password";
                errorMassage = "The email address is already registered.";
            }
        }
    }
}
