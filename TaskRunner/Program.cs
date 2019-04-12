using System;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace TaskRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"resources\config_list.xml");
            //xDoc.Load(@"resources\config_list.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                ApplicationServiceBase application = new ApplicationServiceBase();
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");
                    if (attr != null)
                        application.Name = attr.Value;

                }
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "HostAppName")
                        application.HostApplicationName = childnode.InnerText;

                    if (childnode.Name == "HostAddress")
                        application.HostAddress = childnode.InnerText;

                    if (childnode.Name == "IsManipulate")
                        application.IsManipulate = Convert.ToBoolean(childnode.InnerText);

                    if (childnode.Name == "Username")
                        application.Username = childnode.InnerText;

                    if (childnode.Name == "Password")
                        application.Password = childnode.InnerText;

                    if (childnode.Name == "CommandPrefix")
                        application.CommandPrefix = childnode.InnerText;
                    if (childnode.Name == "OS")
                        application.OS = childnode.InnerText;
                }
                
            }

            Console.WriteLine("Hello World!");
        }
    }
}
