using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.IO;

namespace TaskRunner
{
    class SheduleTask
    {
        public String Name { get; set; }
        public DateTime Start_time { get; set; }
        public String Sql_File_name { get; set; }
        public String Frequency_type { get; set; }
        public int Frequency { get; set; }
        public String[] E_mails { get; set; }
        #region Private variables
        private OracleCommand command = null;
        private String Sql_File_Command { get; set; }
        private String FilePath { get; set; }
        #endregion Private variables

        

        public SheduleTask(string Name, DateTime Start_time, string Sql_File_name, string Frequency_type, int Frequency, string[] emails)
        {
            this.Name = Name;
            this.Start_time = Start_time;
            this.Sql_File_name = Sql_File_name;
            this.Frequency_type = Frequency_type;
            this.Frequency = Frequency;
            this.E_mails = emails;
            FilePath = GetPathSqlFile();
            if (File.Exists(FilePath))
            {
                using (StreamReader reader = new StreamReader(GetPathSqlFile()))
                {
                    Sql_File_Command = reader.ReadToEnd();
                }
            }
            else
                throw new Exception("Файл не существует в директории Tasks");
            
            
        }

        public async Task<int> SheduleTaskRunner(OracleConnection connection)
        {
            if (connection != null || !String.IsNullOrEmpty(Sql_File_Command))
            {
                command = new OracleCommand(Sql_File_Command, connection);
            }
            int resultCode = await command.ExecuteNonQueryAsync();

            return resultCode;
        }
        
        private String GetPathSqlFile()
        {
            string path = String.Empty;
            if (!String.IsNullOrEmpty(Sql_File_name))
            {
                return $"Tasks/{Sql_File_name}";
            }
            else
                throw new Exception("Имя файла не указано. Не удается найти путь");
                
        }


    }
}
