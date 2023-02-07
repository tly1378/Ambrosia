using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Xml.Linq;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Tabler
{
    public class DatabaseManager : Singleton<DatabaseManager>
    {
        public MySqlConnection conn;

        const string server = "rm-bp1f2707574ik8gq3po.mysql.rds.aliyuncs.com";
        const string database = "ambrosia";
        const string uid = "karl";
        const string password = "123abc.";

        public DatabaseManager()
        {
            string constr = "SERVER=" + server + ";"
                + "DATABASE=" + database + ";"
                + "UID=" + uid + ";"
                + "PASSWORD=" + password + ";";

            conn = new MySqlConnection(constr);
            conn.Open();
            Console.WriteLine($"[MySql] Connection succeed");
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
            catch (MySqlException)
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

        internal int Verify(string email, string password)
        {
            string sql = $"SELECT * FROM users WHERE user_email = \"{email}\"";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader.GetString("user_password") == password)
                    {
                        // 密码正确
                        return 1;
                    }
                    else
                    {
                        // 密码错误
                        return -1;
                    }
                }

                return -2;
            }
        }

        public struct Post
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Author { get; set; }
            public string Body { get; set; }
            public int Like { get; set; }
            public int Follow { get; set; }
        }

        internal List<Post> Search(string keyword, (int from, int to) range)
        {
            List<Post> posts = new List<Post>();
            if (!string.IsNullOrWhiteSpace(keyword))
                return posts;

            string sql = $"SELECT * FROM posts LIMIT {range.from},{range.to}";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    posts.Add(reader.GetPost("md"));
                }
            }
            return posts;
        }

        internal ulong GetLastInsertId()
        {
            string sql = $"SELECT LAST_INSERT_ID();";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return (ulong)reader[0];
                }
            }
            return 0;
        }

        public int Execute(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            return cmd.ExecuteNonQuery();
        }

        internal Post Search(int id, string format)
        {
            string sql = $"SELECT * FROM posts WHERE post_id={id}";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            Post post = new Post();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    post = reader.GetPost(format);
                }
            }
            return post;
        }
    }
}
