using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System.Xml.Linq;

namespace Tabler
{
    public class DatabaceManager: Singleton<DatabaceManager>
    {
        public MySqlConnection conn;

        const string server = "rm-bp1f2707574ik8gq3po.mysql.rds.aliyuncs.com";
        const string database = "ambrosia";
        const string uid = "karl";
        const string password = "123abc.";

        public DatabaceManager()
        {
            string constr = "SERVER=" + server + ";"
                + "DATABASE=" + database + ";"
                + "UID=" + uid + ";"
                + "PASSWORD=" + password + ";";

            conn = new MySqlConnection(constr);
            try
            {
                conn.Open();
                Console.WriteLine($"[MySql] Connection succeed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MySql] Connection failed: {ex.Message}");
            }
        }

        internal bool AddUser(string email, string password)
        {
            string name = email.Split('@')[0];
            string data_in = $"INSERT INTO users (user_name, user_email, user_password, submission_date) VALUES ( \"{name}\", \"{email}\", \"{password}\", NOW());";
            MySqlCommand cmd = new MySqlCommand(data_in, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(MySqlException)
            {
                Console.WriteLine($"[MySql] The email address is already registered.");
                return false;
            }
            catch
            {
                throw;
            }
            return true;
        }

        internal bool AddPost(string email, string title, string subtitle, int tags)
        {
            string data_in = $"INSERT INTO posts " +
                $"(post_author, post_title, post_subtitle, post_tag, post_follow, post_like, submission_date) " +
                $"VALUES " +
                $"( \"{email}\", \"{title}\", \"{subtitle}\", {tags}, 0, 0, NOW());";
            MySqlCommand cmd = new MySqlCommand(data_in, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                Console.WriteLine($"[MySql] Post faile.");
                return false;
            }
            catch
            {
                throw;
            }
            return true;
        }

        internal bool Verify(string email, string password)
        {
            string sql = $"SELECT * FROM users WHERE user_email = \"{email}\"";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            using(MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if(reader.GetString("user_password") == password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public struct Post
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Author { get; set; }
            public string Body { get; set; }
            public int Like { get; set; }
            public int Follow { get; set; }
        }

        internal List<Post> Search(string keyword)
        {
            string sql = $"SELECT * FROM posts";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            List<Post> posts = new List<Post>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Post post = new Post();
                    post.Title = reader.GetString("post_title");
                    post.Subtitle = reader.GetString("post_subtitle");
                    post.Author = reader.GetString("post_author");
                    post.Body = GetBody(reader.GetString("post_id"));
                    post.Like = reader.GetInt32("post_like");
                    post.Follow = reader.GetInt32("post_follow");
                    posts.Add(post);
                }
            }
            return posts;
        }

        private string GetBody(string post_id)
        {
            string filepath = $@"D:\temp\test\{post_id}.md";
            Console.WriteLine(filepath);
            return filepath;
        }
    }
}
