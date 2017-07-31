using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Globalization;

namespace OutboundService
{
    public class Common
    {
        public string logFile;
        internal string GetDirectory()
        {
            string slash = "\\";
            string root = ConfigurationManager.AppSettings["RootDirectory"] ?? string.Empty;
            string rootWithYear = root + slash + DateTime.Now.Year.ToString();
            string rootWithMonth = rootWithYear + slash + DateTime.Now.ToString("MMMM");
            string rootWithDate = rootWithMonth + slash + DateTime.Now.Day.ToString();

            if (Directory.Exists(rootWithDate))
            {
                return rootWithDate;
            }
            else
            {
                //Directory.Exists(root + slash + year) ? Directory.Exists(root + slash + year + slash + month) : 
                //string path = Directory.Exists(root) ? root : 

                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                    Directory.CreateDirectory(rootWithYear);
                    Directory.CreateDirectory(rootWithMonth);
                    Directory.CreateDirectory(rootWithDate);
                }
                else
                {
                    if (!Directory.Exists(rootWithYear))
                    {
                        Directory.CreateDirectory(rootWithYear);
                        Directory.CreateDirectory(rootWithMonth);
                        Directory.CreateDirectory(rootWithDate);
                    }
                    else
                    {
                        if (!Directory.Exists(rootWithMonth))
                        {
                            Directory.CreateDirectory(rootWithMonth);
                            Directory.CreateDirectory(rootWithDate);
                        }
                        else if (!Directory.Exists(rootWithDate))
                        {
                            Directory.CreateDirectory(rootWithDate);
                        }
                    }
                }
            }
            return rootWithDate;
        }

        internal void Log(string filePath, string message, string sourceFile = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    bool isFileCreated = true;
                    if (!File.Exists(filePath))
                    {
                        isFileCreated = false;
                        File.Create(filePath).Dispose();
                    }

                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        var line = Environment.NewLine;
                        string logMessage = string.Empty;
                        if (!isFileCreated)
                        {
                            logMessage += "Log Written Date: " + " " + DateTime.Now.ToString(CultureInfo.CurrentCulture) + line;
                        }

                        if (!string.IsNullOrEmpty(sourceFile))
                        {
                            logMessage += "Source Name: " + sourceFile + line;
                        }

                        logMessage += message + line;
                        sw.WriteLine(logMessage);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
                throw;
            }
        }

        internal void SetLogFile(string fileName)
        {
            logFile = string.IsNullOrEmpty(fileName) ? null : GetDirectory() + "\\" + fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
        }

        //public string RetriveDirectory(string directory)
        //{
        //    if (Directory.Exists(directory))
        //    {
        //        return directory;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
