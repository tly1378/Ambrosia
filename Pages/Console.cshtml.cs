using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace Tabler.Pages
{
    public class ConsoleModel : PageModel
    {
        // 生成一篇帖子
        public void OnPostGeneratePost()
        {
            PostRandom();
        }

        // 生成十篇帖子
        public void OnPostGeneratePosts()
        {
            for (int i = 0; i < 10; i++)
                PostRandom();
        }

        // 删除用户信息
        public void OnPostDeleteUserinfo()
        {
            Console.WriteLine($"Delete {DatabaseManager.Instance.Execute("delete from users")} users.");
        }

        // 删除本地数据
        public void OnPostDeliteLocalData()
        {
            string path = Configuration.Instance["postsPath"];
            int count = 0;
            foreach (string entries in Directory.GetFileSystemEntries(path))
            {
                FileInfo fileInfo = new FileInfo(entries);
                if (fileInfo.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fileInfo.Attributes = FileAttributes.Normal;
                System.IO.File.Delete(entries);//直接删除其中的文件 
                count++;
            }
            Console.WriteLine($"Delete {count} posts on local.");
        }

        // 删除所有帖子
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

        #region 5.0 生成随机字符串 + static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
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
