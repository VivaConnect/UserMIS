using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;

namespace user_sms_summary
{
    class Program
    {
        static void Main(string[] args)
        {

            int days = int.Parse(ConfigurationManager.AppSettings["adddays"].ToString());
            string startdate = DateTime.Now.AddDays(days).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.AddDays(days).ToString("yyyy-MM-dd");
            string startdate_numric = DateTime.Now.AddDays(days).ToString("yyyyMMdd");
            string enddate_numric = DateTime.Now.AddDays(days).ToString("yyyyMMdd");

            //string day_query = "select dlr_username,user_type,toemailid,subject,cc_isactive,ccid,bcc_isactive,bccid,show_balance,show_rslbalance,show_invoice_count from user_dlr_summary_mail  where alert_day_type=1 and is_active=1;";
            string day_query = @"select udsm.dlr_username,msd.server_connstring,udsm.dlr_server_id,
                               udsm.user_type,udsm.toemailid,udsm.subject,
                               udsm.cc_isactive,udsm.ccid,udsm.bcc_isactive,udsm.bccid,
                               udsm.show_balance,udsm.show_rslbalance,udsm.show_invoice_count 
                               from user_dlr_summary_mail udsm join mis_server_details msd 
                               on udsm.send_server_id=msd.serverid join dlr_serverdetails ds 
                               on udsm.dlr_server_id=ds.dlr_serverid where udsm.alert_day_type=1 and udsm.is_active=1;";
            Sendalert(days, startdate, enddate, startdate_numric, enddate_numric, day_query);

            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {

                startdate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                enddate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                startdate_numric = DateTime.Now.AddDays(-7).ToString("yyyyMMdd");
                enddate_numric = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");

                //day_query = "select dlr_username,user_type,toemailid,subject,cc_isactive,ccid,bcc_isactive,bccid,show_balance,show_rslbalance,show_invoice_count from user_dlr_summary_mail  where alert_day_type=2 and is_active=1;";
                day_query = @"select udsm.dlr_username,msd.server_connstring,udsm.dlr_server_id,
                               udsm.user_type,udsm.toemailid,udsm.subject,
                               udsm.cc_isactive,udsm.ccid,udsm.bcc_isactive,udsm.bccid,
                               udsm.show_balance,udsm.show_rslbalance,udsm.show_invoice_count 
                               from user_dlr_summary_mail udsm join mis_server_details msd 
                               on udsm.send_server_id=msd.serverid join dlr_serverdetails ds 
                               on udsm.dlr_server_id=ds.dlr_serverid where udsm.alert_day_type=2 and udsm.is_active=1;";
                Sendalert(days, startdate, enddate, startdate_numric, enddate_numric, day_query);

            }
        }





        public static void Sendalert(int days, string startdate, string enddate, string startdate_numric, string enddate_numric, string str_getusername)
        {
            DataSet ds_username = DL.DL_ExecuteSimpleQuery(str_getusername);
            if (ds_username != null && ds_username.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr_username in ds_username.Tables[0].Rows)
                {

                    StringBuilder sb = new StringBuilder();


                    #region For HTTP Users
                    if (dr_username["user_type"].ToString() == "1")
                    {

                        sb.Append("<table cellspacing='0' border='1' cellpadding='5' width='70%'><tr align='left' valign='top' style='background-color:#9c9c9c'><th>Sr No.</th><th>User Name</th><th>Total Sent </th><th>Delivered </th><th>Expired </th><th>Undelivered </th><th>Dnc Reject </th><th> Other</th><th>SMSC </th>");
                        if (dr_username["show_balance"].ToString() == "1")
                        {
                            sb.Append("<th>Balance</th>");
                        }
                        if (dr_username["show_invoice_count"].ToString() == "1")
                        {
                            sb.Append("<th>Credit deducted/Invoice count</th>");
                        }

                        sb.Append("</tr>");

                        int count = 1;

                        Dictionary<string, long> finalusersummery = GetCount.getsmscount(startdate_numric, enddate_numric, dr_username["dlr_username"].ToString(), 1, int.Parse(dr_username["show_balance"].ToString()), int.Parse(dr_username["dlr_server_id"].ToString()), dr_username["server_connstring"].ToString());
                        if (finalusersummery != null && finalusersummery.Count > 0)
                        {

                            sb.Append("<tr><td>" + count + "</td><td>" + dr_username["dlr_username"].ToString() + "</td><td>" + finalusersummery["TotalSent"] + "</td><td>" + finalusersummery["DELIVRD"] + "</td><td>" + finalusersummery["EXPIRD"] + "</td><td>" + finalusersummery["UNDELIVRD"] + "</td><td>" + finalusersummery["DNC"] + "</td><td>" + finalusersummery["Other"] + "</td><td>" + finalusersummery["SMSC"] + "</td>");
                            if (dr_username["show_balance"].ToString() == "1")
                            {
                                sb.Append("<td>" + finalusersummery["balance"] + "</td>");
                            }
                            if (dr_username["show_invoice_count"].ToString() == "1")
                            {
                                sb.Append("<td>" + finalusersummery["Invoicecount"] + "</td>");
                            }

                            sb.Append("</tr>");
                            count++;
                        }

                        sb.Append("</table><br>");
                    }
                    #endregion

                    #region For HTTP Reseller

                    if (dr_username["user_type"].ToString() == "2")
                    {


                        sb.Append("Reseller Name : <b>" + dr_username["dlr_username"].ToString() + "</b><br>");


                        if (dr_username["show_rslbalance"].ToString() == "1")
                        {
                            string qry_getbalrsl = "select c.Total_Credits_Assigned-r.Credits_Assigned as bal from customer c,resellers r where c.UserName=r.username and c.UserName='" + dr_username["dlr_username"] + "';";
                            DataSet ds_getrslbal = DL.DL_ExecuteSimpleQuery(qry_getbalrsl, MSCon.DecryptConnectionString(ConfigurationManager.AppSettings["sms_connstring"].ToString()));
                            if (ds_getrslbal != null && ds_getrslbal.Tables[0].Rows.Count > 0)
                            {
                                sb.Append("Balance : <b>" + ds_getrslbal.Tables[0].Rows[0]["bal"] + "</b><br>");
                            }

                        }

                        string str_smsconnstring = MSCon.DecryptConnectionString(ConfigurationManager.AppSettings["sms_connstring"].ToString());
                        string getuserlist = "select distinct c.username from customer c,usc_smscount usc where c.username=usc.username and c.parentresellername='" + dr_username["dlr_username"].ToString() + "' and usc.send_date>=" + startdate_numric + " and usc.send_date<=" + enddate_numric + ";";
                        DataSet ds_userlist = DL.DL_ExecuteSimpleQuery(getuserlist, str_smsconnstring);
                        if (ds_userlist != null && ds_userlist.Tables[0].Rows.Count > 0)
                        {
                            sb.Append("<table cellspacing='0' border='1' cellpadding='5' width='70%'><tr align='left' valign='top' style='background-color:#9c9c9c'><th>Sr No.</th><th>User Name</th><th>Total Sent </th><th>Delivered </th><th>Expired </th><th>Undelivered </th><th>Dnc Reject </th><th> Other</th><th>SMSC </th>");
                            if (dr_username["show_balance"].ToString() == "1")
                            {
                                sb.Append("<th>Balance</th>");
                            }
                            if (dr_username["show_invoice_count"].ToString() == "1")
                            {
                                sb.Append("<th>Credit deducted/Invoice count</th>");
                            }

                            sb.Append("</tr>");
                            int count = 1;
                            foreach (DataRow dr in ds_userlist.Tables[0].Rows)
                            {
                                Dictionary<string, long> finalusersummery = GetCount.getsmscount(startdate_numric, enddate_numric, dr["username"].ToString(), 1, int.Parse(dr_username["show_balance"].ToString()), int.Parse(dr_username["dlr_server_id"].ToString()), dr_username["server_connstring"].ToString());
                                sb.Append("<tr><td>" + count + "</td><td>" + dr["username"].ToString() + "</td><td>" + finalusersummery["TotalSent"] + "</td><td>" + finalusersummery["DELIVRD"] + "</td><td>" + finalusersummery["EXPIRD"] + "</td><td>" + finalusersummery["UNDELIVRD"] + "</td><td>" + finalusersummery["DNC"] + "</td><td>" + finalusersummery["Other"] + "</td><td>" + finalusersummery["SMSC"] + "</td>");
                                if (dr_username["show_balance"].ToString() == "1")
                                {
                                    sb.Append("<td>" + finalusersummery["balance"] + "</td>");
                                }
                                if (dr_username["show_invoice_count"].ToString() == "1")
                                {
                                    sb.Append("<td>" + finalusersummery["Invoicecount"] + "</td>");
                                }

                                count++;
                                sb.Append("</tr>");

                            }
                            sb.Append("</table><br>");
                        }


                    }
                    #endregion

                    #region For SMPP user
                    if (dr_username["user_type"].ToString() == "3")
                    {
                        sb.Append("SMPP Users:-" + "<br><br>");
                        sb.Append("<table cellspacing='0' border='1' cellpadding='5' width='70%'><tr align='left' valign='top' style='background-color:#9c9c9c'><th>Sr No.</th><th>User Name</th><th>Total Sent </th><th>Delivered </th><th>Expired </th><th>Undelivered </th><th>Dnc Reject </th><th> Other</th><th>SMSC </th>");
                        if (dr_username["show_balance"].ToString() == "1")
                        {
                            sb.Append("<th>Balance</th>");
                        }
                        if (dr_username["show_invoice_count"].ToString() == "1")
                        {
                            sb.Append("<th>Credit deducted/Invoice count</th>");
                        }

                        sb.Append("</tr>");
                        int count = 1;
                        Dictionary<string, long> finalusersummery = GetCount.getsmscount(startdate_numric, enddate_numric, dr_username["dlr_username"].ToString(), 2, int.Parse(dr_username["show_balance"].ToString()), int.Parse(dr_username["dlr_server_id"].ToString()), dr_username["server_connstring"].ToString());
                        if (finalusersummery != null && finalusersummery.Count > 0)
                        {

                            sb.Append("<tr><td>" + count + "</td><td>" + dr_username["dlr_username"].ToString() + "</td><td>" + finalusersummery["TotalSent"] + "</td><td>" + finalusersummery["DELIVRD"] + "</td><td>" + finalusersummery["EXPIRD"] + "</td><td>" + finalusersummery["UNDELIVRD"] + "</td><td>" + finalusersummery["DNC"] + "</td><td>" + finalusersummery["Other"] + "</td><td>" + finalusersummery["SMSC"] + "</td>");
                            if (dr_username["show_balance"].ToString() == "1")
                            {
                                sb.Append("<td>" + finalusersummery["balance"] + "</td>");
                            }
                            if (dr_username["show_invoice_count"].ToString() == "1")
                            {
                                sb.Append("<td>" + finalusersummery["Invoicecount"] + "</td>");
                            }


                            sb.Append("</tr>");
                            count++;
                        }

                        sb.Append("</table><br><br>");
                    }
                    #endregion

                    #region For whole reseller hirerchy
                    if (dr_username["user_type"].ToString() == "4")
                    {
                        string username = dr_username["dlr_username"].ToString();
                        List<string> user_name = new List<string>();
                        user_name.Add(username);
                        Activaterslhirerchy act_rsl_hirerchy = new Activaterslhirerchy(username, user_name);
                        if (user_name.Count > 0)
                        {


                            foreach (string str_rslname in user_name)
                            {

                                string str_smsconnstring = MSCon.DecryptConnectionString(ConfigurationManager.AppSettings["sms_connstring"].ToString());
                                string getuserlist = "select distinct c.username from customer c,usc_smscount usc where c.username=usc.username and c.parentresellername='" + str_rslname + "' and usc.send_date>=" + startdate_numric + " and usc.send_date<=" + enddate_numric + ";";
                                DataSet ds_userlist = DL.DL_ExecuteSimpleQuery(getuserlist, str_smsconnstring);
                                if (ds_userlist != null && ds_userlist.Tables[0].Rows.Count > 0)
                                {
                                    sb.Append("Reseller Name : <b>" + str_rslname + "</b><br>");
                                    sb.Append("<table cellspacing='0' border='1' cellpadding='5' width='70%'><tr align='left' valign='top' style='background-color:#9c9c9c'><th>Sr No.</th><th>User Name</th><th>Total Sent </th><th>Delivered </th><th>Expired </th><th>Undelivered </th><th>Dnc Reject </th><th> Other</th><th>SMSC </th>");
                                    if (dr_username["show_balance"].ToString() == "1")
                                    {
                                        sb.Append("<th>Balance</th>");
                                    }
                                    if (dr_username["show_invoice_count"].ToString() == "1")
                                    {
                                        sb.Append("<th>Credit deducted/Invoice count</th>");
                                    }

                                    sb.Append("</tr>");
                                    int count = 1;
                                    foreach (DataRow dr in ds_userlist.Tables[0].Rows)
                                    {
                                        Dictionary<string, long> finalusersummery = GetCount.getsmscount(startdate_numric, enddate_numric, dr["username"].ToString(), 1, int.Parse(dr_username["show_balance"].ToString()), int.Parse(dr_username["dlr_server_id"].ToString()), dr_username["server_connstring"].ToString());
                                        if (finalusersummery != null && finalusersummery.Count > 0)
                                        {

                                            sb.Append("<tr><td>" + count + "</td><td>" + dr["username"].ToString() + "</td><td>" + finalusersummery["TotalSent"] + "</td><td>" + finalusersummery["DELIVRD"] + "</td><td>" + finalusersummery["EXPIRD"] + "</td><td>" + finalusersummery["UNDELIVRD"] + "</td><td>" + finalusersummery["DNC"] + "</td><td>" + finalusersummery["Other"] + "</td><td>" + finalusersummery["SMSC"] + "</td>");
                                            if (dr_username["show_balance"].ToString() == "1")
                                            {
                                                sb.Append("<td>" + finalusersummery["balance"] + "</td>");
                                            }
                                            if (dr_username["show_invoice_count"].ToString() == "1")
                                            {
                                                sb.Append("<td>" + finalusersummery["Invoicecount"] + "</td>");
                                            }


                                            sb.Append("</tr>");
                                            count++;
                                        }

                                    }
                                    sb.Append("</table><br>");
                                }
                            }
                        }
                    }

                    #endregion

                    sb.Append("<br>Thanks & Regards<br>");
                    sb.Append("Vivaconnect Technical Team.");

                    #region Send Mail
                    try
                    {
                        ____logconfig.Log_Write(____logconfig.LogLevel.DEBUG, 0, "Email sending started for  sales person==>" + dr_username["dlr_username"].ToString());
                        EmailDetails obj = new EmailDetails(ConfigurationSettings.AppSettings["from_emailid"].ToString(),
                                                                                    ConfigurationSettings.AppSettings["host_address"].ToString(), ConfigurationSettings.AppSettings["credential_emailid"].ToString(),
                                                                                    ConfigurationSettings.AppSettings["credential_passworrd"].ToString(), Convert.ToInt32(ConfigurationSettings.AppSettings["port"].ToString()),
                                                                                    Convert.ToInt32(ConfigurationSettings.AppSettings["ssl_isactive"].ToString()), Convert.ToInt32(ConfigurationSettings.AppSettings["bodyhtml_isactive"].ToString()));
                        obj.Subject = dr_username["subject"].ToString() + "(" + startdate + " to " + enddate + ")";
                        obj.EmailText = sb.ToString();
                        obj.ToEmailId = dr_username["toemailid"].ToString();
                        if (int.Parse(dr_username["cc_isactive"].ToString()) == 1)
                        {
                            obj.CC = dr_username["ccid"].ToString();
                        }
                        if (int.Parse(dr_username["bcc_isactive"].ToString()) == 1)
                        { obj.Bcc = dr_username["bccid"].ToString(); }
                        obj.SendEmail(dr_username["ccid"].ToString(), dr_username["bccid"].ToString());
                        ____logconfig.Log_Write(____logconfig.LogLevel.DEBUG, 0, "Email sending Ends for  sales person==>" + dr_username["dlr_username"].ToString());

                    }
                    catch (Exception ex)
                    {
                        ____logconfig.Log_Write(____logconfig.LogLevel.DEBUG, 0, "Problem occured for the username" + dr_username["dlr_username"].ToString() + " and exception is==>" + ex.ToString());
                        ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, ex);
                    }

                    #endregion
                }
            }

        }







    }
}

    
