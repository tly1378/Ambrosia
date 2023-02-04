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

            // ��ʽ��֤
            if (string.IsNullOrWhiteSpace(Title))
            {
                errorMassage_Title = "���ⲻ��Ϊ��";
                Console.WriteLine("���ⲻ��Ϊ��");
                return;
            }

            // �˻�������֤ 
            if (!DatabaceManager.Instance.Verify(Email, Password))
            {
                errorMassage_Title = "�˺����벻��ȷ";
                Console.WriteLine("�˺����벻��ȷ");
                return;
            }

            // д�����ݿ�
            if (!DatabaceManager.Instance.AddPost(Email, Title, Subtitle, Tags))
            {
                errorMassage_Title = "д�����ݿ����";
                Console.WriteLine("д�����ݿ����");
                return;
            }

            // д�������
            try
            {
                System.IO.File.WriteAllText($@"D:\temp\test\{Title}.txt", Text, Encoding.UTF8);
            }
            catch
            {
                errorMassage_Title = "д�����������";
                Console.WriteLine("д�����������");
                return;
            }

            // �����־
            Console.WriteLine(
                "New Post:\n" +
                $"\t{Email}\n" +
                $"\t{Title}\n"
                );
        }
    }
}
