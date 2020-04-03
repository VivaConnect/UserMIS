using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace user_sms_summary
{
    class Activaterslhirerchy
    {

        public List<Activaterslhirerchy> _resellerh = new List<Activaterslhirerchy>();
        public Activaterslhirerchy(string username, List<string> user_name)
        {

            try
            {


                string query = "select * from customer where parentresellername='" + username + "'";
                DataSet ds = DL.DL_ExecuteSimpleQuery(query, MSCon.DecryptConnectionString(ConfigurationManager.AppSettings["sms_connstring"].ToString()));

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["AccountType"].ToString().Trim().ToLower() == "reseller")
                        {
                            user_name.Add(ds.Tables[0].Rows[i]["username"].ToString().Trim());
                            _resellerh.Add(new Activaterslhirerchy(ds.Tables[0].Rows[i]["UserName"].ToString(), user_name));
                        }
                    }

                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
