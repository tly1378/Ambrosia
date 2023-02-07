using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace Tabler.Pages
{
    public class ConsoleModel : PageModel
    {
        // ����һƪ����
        public void OnPostGeneratePost()
        {
            PostRandom();
        }

        // ����ʮƪ����
        public void OnPostGeneratePosts()
        {
            for (int i = 0; i < 10; i++)
                PostRandom();
        }

        // ɾ���û���Ϣ
        public void OnPostDeleteUserinfo()
        {
            Console.WriteLine($"Delete {DatabaseManager.Instance.Execute("delete from users")} users.");
        }

        // ɾ����������
        public void OnPostDeliteLocalData()
        {
            string path = Configuration.Instance["postsPath"];
            int count = 0;
            foreach (string entries in Directory.GetFileSystemEntries(path))
            {
                FileInfo fileInfo = new FileInfo(entries);
                if (fileInfo.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fileInfo.Attributes = FileAttributes.Normal;
                System.IO.File.Delete(entries);//ֱ��ɾ�����е��ļ� 
                count++;
            }
            Console.WriteLine($"Delete {count} posts on local.");
        }

        // ɾ����������
        public void OnPostDeletePosts()
        {
            int count = DatabaseManager.Instance.Execute("delete from posts");
            Console.WriteLine($"Delete {count} posts on remote.");
        }

        public void PostRandom()
        {
            string Title = GetRandomString(10);
            string Subtitle = GetRandomString(10);
            string Text = GetRandomString(2500);
            string Email = "[Auto]";
            int Tags = 0;
            PostModel.Post(Email, Title, Subtitle, Tags, Text, out _);
        }

        #region 5.0 ��������ַ��� + static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        ///<summary>
        ///��������ַ��� 
        ///</summary>
        ///<param name="length">Ŀ���ַ����ĳ���</param>
        ///<param name="useNum">�Ƿ�������֣�1=������Ĭ��Ϊ����</param>
        ///<param name="useLow">�Ƿ����Сд��ĸ��1=������Ĭ��Ϊ����</param>
        ///<param name="useUpp">�Ƿ������д��ĸ��1=������Ĭ��Ϊ����</param>
        ///<param name="useSpe">�Ƿ���������ַ���1=������Ĭ��Ϊ������</param>
        ///<param name="custom">Ҫ�������Զ����ַ���ֱ������Ҫ�������ַ��б�</param>
        ///<returns>ָ�����ȵ�����ַ���</returns>
        public static string GetRandomString(
            int length,
            string custom = "",
            bool useNum = false,
            bool useLow = false,
            bool useUpp = true,
            bool useSpe = false
            )
        {
            byte[] b = new byte[4];
            Random random = new Random();
            random.NextBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = "";
            string str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        #endregion
    }
}
