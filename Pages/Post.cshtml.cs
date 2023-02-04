using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace Tabler.Pages
{
    public class PostModel : PageModel
    {
        public static ushort MaxWordCount { get; } = ushort.MaxValue;
        public string errorMassage_Title { get; set; } = string.Empty;
        public string errorMassage_Email { get; set; } = string.Empty;
        public string errorMassage_Password { get; set; } = string.Empty;

        public void OnGet()
        {

        }

        public void OnPost(
            string Email,
            string Password,
            string Title,
            string Subtitle,
            int Tags,
            string Text
            )
        {
            Console.WriteLine(
                "New Post..."
                );

            errorMassage_Title = string.Empty;

            // 格式验证
            if (string.IsNullOrWhiteSpace(Title))
            {
                errorMassage_Title = "标题不可为空";
                Console.WriteLine("标题不可为空");
                return;
            }

            // 账户密码验证 
            int result = DatabaceManager.Instance.Verify(Email, Password);
            if (result == -1)
            {
                errorMassage_Email = "密码不正确";
                Console.WriteLine("密码不正确");
                return;
            }
            else if (result == -2)
            {
                errorMassage_Password = "账号不存在";
                Console.WriteLine("账号不存在");
                return;
            }

            // 写入数据库
            if (!DatabaceManager.Instance.AddPost(Email, Title, Subtitle, Tags))
            {
                errorMassage_Title = "写入数据库错误";
                Console.WriteLine("写入数据库错误");
                return;
            }

            // 写入服务器
            UInt64 postId = DatabaceManager.Instance.GetLastInsertId();
            try
            {
                System.IO.File.WriteAllText($@"D:\temp\test\{postId}.md", Text, Encoding.UTF8);
            }
            catch
            {
                errorMassage_Title = "写入服务器错误";
                Console.WriteLine("写入服务器错误");
                return;
            }

            // 输出日志
            Console.WriteLine(
                "New Post:\n" +
                $"\t{Email}\n" +
                $"\t{Title}\n"
                );
        }
    }
}
