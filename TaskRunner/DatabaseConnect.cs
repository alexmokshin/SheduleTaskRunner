using System;
using System.Collections.Generic;
using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace TaskRunner
{
    class DatabaseConnect
    {
        private OracleConnection connection = null;
        static string connectionString = new DbUtils().GetConnectionString("sharp", "ab123456", "HPIHA", 1521, "XE");
        public OracleConnection GetOracleConnection()
        {
            if (connection != null)
                return connection;
            else
            {
                connection = new OracleConnection(connectionString);
                return connection;
            }
        }
    }
}
