using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Tabler.DatabaseManager;

namespace Tabler.Pages
{
    public class TextModel : PageModel
    {
        public Post Post;

        public void OnGet()
        {
            string idString = Request.Query["id"];
            if (idString != null)
            {
                int id = int.Parse(idString);
                Post = DatabaseManager.Instance.Search(id, "md");
            }
        }
    }
}
