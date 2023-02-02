using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tabler.Pages
{
    public class IndexModel : PageModel
    {
        public string Email { get; set; } = string.Empty;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ViewData["GameTypes"] = new string[]
            {
                "动作",
                "剧情",
                "解密",
                "经营",
                "模拟",
                "建造",
                "射击",
                "冒险",
                "色情",
                "竞技",
                "生存",
            };
        }
    }
}