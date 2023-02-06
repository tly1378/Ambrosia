using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tabler.Pages
{
    public class ListModel : PageModel
    {
        public (int from, int to) Range = (0, 10);
        public List<DatabaseManager.Post> Posts = new List<DatabaseManager.Post>();

        public void OnGet()
        {
            // ½á¹û·¶Î§
            string rangeString = Request.Query["range"];
            if (rangeString != null)
            {
                var rangeStrings = rangeString.Split('-');
                Range.from = int.Parse(rangeStrings[0]);
                Range.to = int.Parse(rangeStrings[1]);
            }

            // ËÑË÷¹Ø¼ü´Ê
            string keywordString = Request.Query["search"];

            // ËÑË÷
            Posts = DatabaseManager.Instance.Search(keywordString, Range);
        }
    }
}
