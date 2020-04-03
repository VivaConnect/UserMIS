using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace user_sms_summary
{
    class GetCount
    {
        public static Dictionary<string, long> getsmscount(string startdate, string enddate, string str_username, int type, int show_balance, int dlrId, string sendConnstring)
        {
            try
            {
                Dictionary<string, long> dict_userdata = new Dictionary<string, long>();
                string smsconnstring = MSCon.DecryptConnectionString(sendConnstring);
                string misdlrconnstring = MSCon.DecryptConnectionString(ConfigurationManager.AppSettings["mis_connstring"].ToString());

                long sent_count = 0;
                long delivrdcount = 0;
                long expirdcount = 0;
                long undelivrdcount = 0;
                long dnccount = 0;
                long smsccount = 0;
                long othercount = 0;
                long invoicecount = 0;
                long balance = 0;
                string getsentcount = "";

                DataSet ds_sentcount;



                if (type == 1)
                {
                    getsentcount = "select username,sum(Usms+Nsms+ncpr_dnc_cr_dc_n+ncpr_dnc_cr_dc_u+ncpr_dnc_cr_rf_n+ncpr_dnc_cr_rf_u) as sentcount from usc_smscount  where UserName='" + str_username + "' and send_date>=" + startdate + " and send_date<=" + enddate + ";";
                    ds_sentcount = DL.DL_ExecuteSimpleQuery(getsentcount, smsconnstring);

                    if (show_balance == 1)
                    {
                        string qry_getbal = "select Total_Credits_Assigned-Total_Credits_Used as balance from customer where UserName='" + str_username + "';";
                        DataSet ds_getbal = DL.DL_ExecuteSimpleQuery(qry_getbal, smsconnstring);
                        if (ds_getbal != null && ds_getbal.Tables[0].Rows.Count > 0)
                        {
                            balance = long.Parse(ds_getbal.Tables[0].Rows[0]["balance"].ToString());
                        }
                    }

                }
                else
                {
                    getsentcount = "select smppname_username as username,sum(smppcount) as sentcount from mis_smpp_count  where smppname_username='" + str_username + "' and senddate>=" + startdate + " and senddate<=" + enddate + " group by smppname_username;";
                    ds_sentcount = DL.DL_ExecuteSimpleQuery(getsentcount, smsconnstring);
                }




                #region Check DLR
                if (ds_sentcount != null && ds_sentcount.Tables[0].Rows.Count > 0)
                {
                    sent_count = ds_sentcount.Tables[0].Rows[0]["sentcount"] == DBNull.Value ? 0 : long.Parse(ds_sentcount.Tables[0].Rows[0]["sentcount"].ToString());
                    if (sent_count > 0)
                    {
                        string deliveredcount = @"select username,sum(if(dlrstatus like 'DEL%', delivercount*messagelength,0)) as DELIVRD,
                                                    sum(if(dlrstatus like 'EXP%', delivercount*messagelength,0)) as EXPIRD,
                                                    sum(if(dlrstatus like 'UNDE%', delivercount*messagelength,0)) as UNDELIV ,
                                                    sum(if(dlrstatus like 'DNC%', delivercount,0)) as DNC_REJECT,
                                                    sum(if(dlrstatus not in('DELIVRD','DNC REJECT.','ndnc_reject','EXPIRED','UNDELIV'), delivercount*messagelength,0)) as Other,senddate
                                                    from dlr_summary  
                                                    where  username='" + str_username + "'" +
                                                       " and senddate>=" + startdate + " and senddate<= " + enddate + "" +
                                                       " group by username";
                        if (type == 1)
                        {
                            deliveredcount += " ," + dlrId + "";
                        }
                        DataSet ds_dlrcount = DL.DL_ExecuteSimpleQuery(deliveredcount, misdlrconnstring);
                        if (ds_dlrcount != null && ds_dlrcount.Tables[0].Rows.Count > 0)
                        {
                            string uname = ds_dlrcount.Tables[0].Rows[0]["username"] == DBNull.Value ? "NA" : ds_dlrcount.Tables[0].Rows[0]["username"].ToString();
                            if (uname != "NA")
                            {
                                delivrdcount = ds_dlrcount.Tables[0].Rows[0]["DELIVRD"] == DBNull.Value ? 0 : long.Parse(ds_dlrcount.Tables[0].Rows[0]["DELIVRD"].ToString());
                                expirdcount = ds_dlrcount.Tables[0].Rows[0]["EXPIRD"] == DBNull.Value ? 0 : long.Parse(ds_dlrcount.Tables[0].Rows[0]["EXPIRD"].ToString());
                                undelivrdcount = ds_dlrcount.Tables[0].Rows[0]["UNDELIV"] == DBNull.Value ? 0 : long.Parse(ds_dlrcount.Tables[0].Rows[0]["UNDELIV"].ToString());
                                dnccount = ds_dlrcount.Tables[0].Rows[0]["DNC_REJECT"] == DBNull.Value ? 0 : long.Parse(ds_dlrcount.Tables[0].Rows[0]["DNC_REJECT"].ToString());
                                othercount = ds_dlrcount.Tables[0].Rows[0]["Other"] == DBNull.Value ? 0 : long.Parse(ds_dlrcount.Tables[0].Rows[0]["Other"].ToString());
                                smsccount = sent_count - (delivrdcount + expirdcount + undelivrdcount + dnccount + othercount);

                            }

                            invoicecount = delivrdcount + smsccount;

                            dict_userdata.Add("TotalSent", sent_count);
                            dict_userdata.Add("DELIVRD", delivrdcount);
                            dict_userdata.Add("UNDELIVRD", undelivrdcount);
                            dict_userdata.Add("DNC", dnccount);
                            dict_userdata.Add("EXPIRD", expirdcount);
                            dict_userdata.Add("Other", othercount);
                            dict_userdata.Add("SMSC", long.Parse(smsccount.ToString()));
                            dict_userdata.Add("Invoicecount", invoicecount);
                            dict_userdata.Add("balance", balance);
                            return dict_userdata;
                        }
                        else { return dict_userdata; }

                    }
                    else { return dict_userdata; }
                }
                else { return dict_userdata; }
                #endregion

            }
            catch (Exception ex)
            { return null; }


        }
    }
}
