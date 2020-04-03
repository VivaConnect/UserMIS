using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.IO;


namespace user_sms_summary
{
    public class MSCon
    {
        public static string ConnectionString
        {
            get { return DecryptConnectionString(); }
        }
        private static string DecryptConnectionString()
        {
            try
            {
                string strConDe = ConfigurationManager.AppSettings["MysqlConnectionString"];
                Byte[] b = Convert.FromBase64String(strConDe);
                return System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (Exception exc)
            {
                string errMessage = exc.ToString();
                return "";
            }
        }

        public static string DecryptConnectionString(string strConDe)
        {
            try
            {
               // string strConDe = ConfigurationManager.AppSettings["MysqlConnectionString"];
                Byte[] b = Convert.FromBase64String(strConDe);
                return System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (Exception exc)
            {
                string errMessage = exc.ToString();
                return "";
            }
        }




    }
}
