using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using System.Threading;

namespace TaskRunner
{
    //ставлю sealed, чтобы никто не додумался от класса наследоваться. Данные выражения применимы только если ты хочешь выполнять SQL, уот так уот.
    //может добавить базу...
    sealed class SheduleTask
    {
        public String Name { get; set; }
        public DateTime Start_time { get; set; }
        public String Sql_File_name { get; set; }
        public String Frequency_type { get; set; }
        public int Frequency { get; set; }
        public String[] E_mails { get; set; }
        #region Private variables
        private OracleConnection connection = null;
        private OracleCommand command = null;
        private String Sql_File_Command;
        private String FilePath;
        private int resultCode;
        private bool IsComplete = false;
        #endregion Private variables

        #region Constant variable
        private const int TASK_ERROR_VALUE = -100;
        #endregion Constant variable



        public SheduleTask(string Name, DateTime Start_time, string Sql_File_name, string Frequency_type, int Frequency, string[] emails)
        {
            this.Name = Name;
            this.Start_time = Start_time;
            this.Sql_File_name = Sql_File_name;
            this.Frequency_type = Frequency_type;
            this.Frequency = Frequency;
            this.E_mails = emails;
            this.FilePath = GetPathSqlFile();
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

        public async Task<int> SheduleTaskRunner()
        {
            try
            {
                using (connection = new DatabaseConnect().GetOracleConnection())
                {
                    connection.Open();
                    using (command = new OracleCommand(Sql_File_Command, connection))
                    {

                        Console.WriteLine("{0} Начало выполнения задачи {1}", DateTime.Now, Name);
                        resultCode = await command.ExecuteNonQueryAsync();
                        IsComplete = true;
                        Console.WriteLine("{0} Конец выполнения задачи {1}", DateTime.Now, Name, resultCode);
                        connection.Close();
                        return resultCode;
                    }
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine("{0} Error with Task: {1}\nError is {2}\n{3}", DateTime.Now, Name, ex.Message, ex.StackTrace);
                return TASK_ERROR_VALUE;
            }

        }

        private String GetPathSqlFile()
        {
            string path = String.Empty;
            if (!String.IsNullOrEmpty(Sql_File_name))
            {
                path = $"Tasks/{Sql_File_name}";
                return path;
            }
            else
                return path;

        }


    }
}
