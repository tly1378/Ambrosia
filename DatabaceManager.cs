using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

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
    }
}
