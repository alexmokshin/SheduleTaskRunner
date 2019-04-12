using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskRunner
{
    class Program
    {
         static void Main(string[] args)
        {
            List<SheduleTask> tasks = new List<SheduleTask>();
            XmlDocument xDoc = new XmlDocument();
            string xmlPath = @"Tasks\TaskSheduller.xml";
            if (File.Exists(xmlPath))
            {
                xDoc.Load(xmlPath);
                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                {
                    string name = string.Empty, sql_file_name = string.Empty, frequency_type = string.Empty;
                    string[] emails = null;
                    int frequency = 0;
                    DateTime start_time = DateTime.Now;
                    if (xnode.Attributes.Count > 0)
                    {
                        XmlNode attr = xnode.Attributes.GetNamedItem("name");
                        if (attr != null)
                            name = attr.Value;

                    }
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        if (childnode.Name == "start_time")
                            DateTime.TryParse(childnode.InnerText, out start_time);

                        if (childnode.Name == "sql_file_name")
                            sql_file_name = childnode.InnerText;

                        if (childnode.Name == "frequency_type")
                        {
                            //TODO: Добавить проверку в шедъюл таск, на то, что фреквенси тайп может содержать только некоторые значения
                            frequency_type = childnode.InnerText;
                        }

                        if (childnode.Name == "frequency_value")
                            Int32.TryParse(childnode.InnerText, out frequency);

                        if (childnode.Name == "e_mail")
                            emails = childnode.InnerText.Split(',');

                    }

                    try
                    {
                        SheduleTask sheduleTask = new SheduleTask(name, start_time, sql_file_name, frequency_type, frequency, emails);
                        tasks.Add(sheduleTask);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            else
            {
                Console.WriteLine("Файла TaskSheduller.xml не существует по пути {0}\nЗадания не загружены", xmlPath);
            }


            Console.WriteLine("Hello World!");

            if (tasks.Count > 0)
            {
                
                    DatabaseConnect databaseConnect = new DatabaseConnect();
                    var connection = databaseConnect.GetOracleConnection();
                    foreach (var task in tasks)
                    {
                        if (task.Start_time < DateTime.Now)
                        {
                            try
                            {
                                //connection.Open();
                                RunSheduleTask(task, connection);
                                //connection.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                
            }
            Console.ReadKey();
            
        }

        static async void RunSheduleTask(SheduleTask sheduleTask, Oracle.ManagedDataAccess.Client.OracleConnection connection)
        {
            
            await Task.Run(() => sheduleTask.SheduleTaskRunner(connection));
            
        }
    }
}
