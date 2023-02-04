using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tabler.Pages
{
    public class ListModel : PageModel
    {
        public List<DatabaceManager.Post> Posts { get; set; } = new List<DatabaceManager.Post>();

        public void OnGet()
        {
            string keyword = Request.Query["search"];
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                Posts = DatabaceManager.Instance.Search(keyword);
            }
            else
            {
                Posts = new List<DatabaceManager.Post>();
            }
        }
    }
}
