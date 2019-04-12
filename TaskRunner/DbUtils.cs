using System.Text;

namespace TaskRunner
{
    internal class DbUtils
    {
        
        private string DbHost { get; set; }
        private int DbPort { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string DbInstance { get; set; }

        public string GetConnectionString(string username, string password, string dbhost, int port, string instance)
        {
            this.Username = username;
            this.DbPort = port;
            this.DbHost = dbhost;
            this.Password = password;
            this.DbInstance = instance;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = {2})));Password={3};User ID={4}", DbHost, DbPort, DbInstance, Password, Username);
            string connectionString = sb.ToString();
            return connectionString;

        }

        public DbUtils() { }
    }
}
