using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tabler.Pages
{
    public class ListModel : PageModel
    {
        public (int from, int to) Range = (0, 10);
        public List<DatabaceManager.Post> Posts = new List<DatabaceManager.Post>();

        public void OnGet()
        {
            // �����Χ
            string rangeString = Request.Query["range"];
            if (rangeString != null)
            {
                var rangeStrings = rangeString.Split('-');
                Range.from = int.Parse(rangeStrings[0]);
                Range.to = int.Parse(rangeStrings[1]);
            }

            // �����ؼ���
            string keywordString = Request.Query["search"];

            // ����
            Posts = DatabaceManager.Instance.Search(keywordString, Range);
        }
    }
}
