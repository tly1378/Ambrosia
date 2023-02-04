using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace Tabler.Pages
{
    public class TypographyModel : PageModel
    {
        public static ushort MaxWordCount { get; } = ushort.MaxValue;
        public string errorMassage_Title { get; set; } = string.Empty;

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
            if (!DatabaceManager.Instance.Verify(Email, Password))
            {
                errorMassage_Title = "账号密码不正确";
                Console.WriteLine("账号密码不正确");
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
            try
            {
                System.IO.File.WriteAllText($@"D:\temp\test\{Title}.txt", Text, Encoding.UTF8);
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
