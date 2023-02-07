using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

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

            // ��ʽ��֤
            if (string.IsNullOrWhiteSpace(Title))
            {
                errorMassage_Title = "���ⲻ��Ϊ��";
                Console.WriteLine("���ⲻ��Ϊ��");
                return;
            }

            // �˻�������֤ 
            int result = DatabaseManager.Instance.Verify(Email, Password);
            if (result == -1)
            {
                errorMassage_Email = "���벻��ȷ";
                Console.WriteLine("���벻��ȷ");
                return;
            }
            else if (result == -2)
            {
                errorMassage_Password = "�˺Ų�����";
                Console.WriteLine("�˺Ų�����");
                return;
            }

            Post(Email, Title, Subtitle, Tags, Text, out string output);
            errorMassage_Title = output;

            // �����־
            Console.WriteLine(
                "New Post:\n" +
                $"\t{Email}\n" +
                $"\t{Title}\n"
                );
        }

        public static void Post(
            string Email, 
            string Title, 
            string Subtitle, 
            int Tags,
            string Text,
            out string errorMassage)
        {
            errorMassage = string.Empty;

            // д�����ݿ�
            if (!DatabaseManager.Instance.AddPost(Email, Title, Subtitle, Tags))
            {
                errorMassage = "д�����ݿ����";
                Console.WriteLine("д�����ݿ����");
                return;
            }

            // д�������
            ulong postId = DatabaseManager.Instance.GetLastInsertId();
            try
            {
                var path = Configuration.Instance["postsPath"];
                string mdPath = Path.Combine(path, $"{postId}.md");
                System.IO.File.WriteAllText(mdPath, Text, Encoding.UTF8);
            }
            catch
            {
                errorMassage = "д�����������";
                Console.WriteLine("д�����������");
                return;
            }
        }

    }
}
