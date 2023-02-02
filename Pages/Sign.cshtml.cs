using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tabler.Pages
{
    public class SignModel : PageModel
    {
        [BindProperty] public string Email { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";

        public void OnGet()
        {

        }

        public void OnPostSignIn()
        {
            Console.WriteLine(
                "New User:\n" +
                $"\t{Email}\n" +
                $"\t{Password}"
                );
        }
    }
}
