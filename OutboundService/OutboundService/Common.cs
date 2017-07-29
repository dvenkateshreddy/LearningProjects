using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace OutboundService
{
    class Common
    {
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
