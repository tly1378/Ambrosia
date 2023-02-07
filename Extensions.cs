using Google.Protobuf.Collections;
using Google.Protobuf;
using MySql.Data.MySqlClient;
using static Tabler.DatabaseManager;

namespace Tabler
{
    public static class Extensions
    {
        public static Post GetPost(this MySqlDataReader reader, string extension = "")
        {
            Post post = new Post();
            post.Id = reader.GetString("post_id");
            post.Title = reader.GetString("post_title");
            post.Subtitle = reader.GetString("post_subtitle");
            post.Author = reader.GetString("post_author");
            if (!string.IsNullOrWhiteSpace(extension))
            {
                string path = Configuration.Instance.GetPostPath(reader.GetString("post_id"), extension);
                if (!string.IsNullOrWhiteSpace(path))
                    post.Body = File.ReadAllText(path);
            }
            post.Like = reader.GetInt32("post_like");
            post.Follow = reader.GetInt32("post_follow");
            return post;
        }
    }
}
