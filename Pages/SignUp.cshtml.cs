using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tabler.Pages
{
    public class SignUpModel : PageModel
    {
        public string emailErrorMassage = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public void OnPostSignUp(string email, string password)
        {
            Console.WriteLine(
                "New User:\n" +
                $"\t{email}\n" +
                $"\t{password}"
                );

            if (string.IsNullOrWhiteSpace(email))
            {
                emailErrorMassage = "邮箱格式不正确";
                return;
            }

            if(DatabaceManager.Instance.AddUser(email, password))
            {
                Email = email;
                Password = password;
            }
            else
            {
                emailErrorMassage = "该邮箱已被注册";
            }
        }
    }
}
