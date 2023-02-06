using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tabler.Pages
{
    public class SignInModel : PageModel
    {
        public string errorMassage_Email = string.Empty;
        public string errorMassage_Password = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public void OnGet()
        {

        }

        public void OnPostSignIn(string email, string password)
        {
            Console.WriteLine(
                "Old User:\n" +
                $"\t{email}\n" +
                $"\t{password}"
                );

            if (string.IsNullOrWhiteSpace(email))
            {
                errorMassage_Email = "邮箱格式不正确";
                return;
            }

            int result = DatabaseManager.Instance.Verify(email, password);
            if (result == 1)
            {
                Email = email;
                Password = password;
            }
            else if (result == -1)
            {
                errorMassage_Password = "密码不正确";
            }
            else if (result == -2)
            {
                errorMassage_Email = "账号不存在";
            }
        }
    }
}
