using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;


namespace user_sms_summary
{
    public class DL
    {
        //static Logs logWrt = new Logs("DataLayer.txt");
        //static Logs queryLog = new Logs("Queries.txt");
        #region Execute queries returning Dataset with Mysql parameters

        public static DataSet DL_ExecuteQuery(string strQuery, string[] arrValues)
        {
            try
            {

                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams;

                mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arrValues[i];
                }
                return MySqlHelper.ExecuteDataset(MSCon.ConnectionString, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                //logWrt.asyncWrite(strQuery + "\n" + exc.ToString());
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, new System.Diagnostics.StackTrace(true), "query failed: " + strQuery + "::values : " + string.Join(",", arrValues), "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
                return null;
            }
        }
        public static DataSet DL_ExecuteQuery(string strQuery, List<string> arrValues)
        {
            try
            {
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams;

                mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arrValues[i];
                }
                return MySqlHelper.ExecuteDataset(MSCon.ConnectionString, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                //logWrt.asyncWrite(strQuery + "\n" + exc.ToString());
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC,0, "query failed: " + strQuery + "::values : " + string.Join(",", arrValues), "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
                return null;
            }
        }
        public static DataSet DL_ExecuteQuery(string strQuery, string[] arrValues, string MSConParam)
        {
            try
            {
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams;

                mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arrValues[i];
                }
                return MySqlHelper.ExecuteDataset(MSConParam, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                //logWrt.asyncWrite(strQuery + "\n" + exc.ToString());
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, new System.Diagnostics.StackTrace(true), "query failed: " + strQuery + "::values : " + string.Join(",", arrValues), "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
                return null;
            }
        }
        public static DataSet DL_ExecuteQuery(string strQuery, List<string> arrValues, string MSConParam)
        {
            try
            {
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams;

                mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arrValues[i];
                }
                return MySqlHelper.ExecuteDataset(MSConParam, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                //logWrt.asyncWrite(strQuery + "\n" + exc.ToString());
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, new System.Diagnostics.StackTrace(true), "query failed: " + strQuery + "::values : " + string.Join(",", arrValues), "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
                return null;
            }
        }

        #endregion

        #region Execute queries returning Dataset without Mysql parameters

        public static DataSet DL_ExecuteSimpleQuery(string strQuery)
        {
            try { return MySqlHelper.ExecuteDataset(MSCon.ConnectionString, strQuery); }
            catch (Exception exc)
            {
                //logWrt.asyncWrite(strQuery + "\n" + exc.ToString());
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, new System.Diagnostics.StackTrace(true), "query failed: " + strQuery, "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
                return null;
            }

        }
        public static DataSet DL_ExecuteSimpleQuery(string strQuery, string MSConParam)
        {
            try { return MySqlHelper.ExecuteDataset(MSConParam, strQuery); }
            catch (Exception exc)
            {
                //logWrt.asyncWrite(strQuery + "\n" + exc.ToString());
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, new System.Diagnostics.StackTrace(true), "query failed: " + strQuery, "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
                return null;
            }

        }

        #endregion

        #region Execute queries returning number of rows affected with Mysql parameters

        public static int DL_ExecuteNonQuery(string strQuery, string[] arrValues)
        {
            try
            {
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arrValues[i];
                }
                return MySqlHelper.ExecuteNonQuery(MSCon.ConnectionString, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                //logWrt.asyncWrite(strQuery + "\n" + exc.ToString());
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, new System.Diagnostics.StackTrace(true), "query failed: " + strQuery + "::values : " + string.Join(",", arrValues), "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
                return 0;
            }
        }
        public static int DL_ExecuteNonQuery(string strQuery, List<string> arrValues)
        {
            try
            {
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arrValues[i];
                }
                return MySqlHelper.ExecuteNonQuery(MSCon.ConnectionString, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                //logWrt.asyncWrite(strQuery + "\n" + exc.ToString());
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, new System.Diagnostics.StackTrace(true), "query failed: " + strQuery + "::values : " + string.Join(",", arrValues), "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
                return 0;
            }
        }
        public static int DL_ExecuteNonQuery(string strQuery, string[] arrValues, string MSConParam)
        {
            try
            {
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arrValues[i];
                }
                return MySqlHelper.ExecuteNonQuery(MSConParam, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                //logWrt.asyncWrite(strQuery + "\n" + exc.ToString());
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, new System.Diagnostics.StackTrace(true), "query failed: " + strQuery + "::values : " + string.Join(",", arrValues), "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
                return 0;
            }
        }
        public static int DL_ExecuteNonQuery(string strQuery, List<string> arrValues, string MSConParam)
        {
            try
            {
                string[] arr_params = getQueryParameters(strQuery);
                MSParams msparam;
                MySqlParameter[] mysqlparams = new MySqlParameter[arr_params.Length];
                for (int i = 0; i < arr_params.Length; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arrValues[i];
                }
                return MySqlHelper.ExecuteNonQuery(MSConParam, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                //logWrt.asyncWrite(strQuery + "\n" + exc.ToString());
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, new System.Diagnostics.StackTrace(true), "query failed: " + strQuery + "::values : " + string.Join(",", arrValues), "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
                return 0;
            }
        }

        #endregion

        #region Execute queries returning number of rows affected without Mysql parameters

        public static int DL_ExecuteSimpleNonQuery(string strQuery)
        {
            try { return MySqlHelper.ExecuteNonQuery(MSCon.ConnectionString, strQuery); }
            catch (Exception exc)
            {
                //logWrt.asyncWrite(strQuery + "\n" + exc.ToString());
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, new System.Diagnostics.StackTrace(true), "query failed: " + strQuery, "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
                return 0;
            }

        }
        public static int DL_ExecuteSimpleNonQuery(string strQuery, string MSConParam)
        {
            try
            {
                return MySqlHelper.ExecuteNonQuery(MSConParam, strQuery);
            }
            catch (Exception exc)
            {
                //logWrt.asyncWrite(strQuery + "\n" + exc.ToString());
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, new System.Diagnostics.StackTrace(true), "query failed: " + strQuery, "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
                return 0;
            }

        }




        //-----------------------//////////////////////////////-------------------------------------//

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strQuery">is the Query</param>
        /// <param name="arrValues">is the Values to be inserted</param>
        /// <param name="MSConParam">is Connection String</param>
        /// <param name="param_array">is Parameter Arrays</param>
        /// <returns></returns>
        public static int DL_ExecuteNonQuery(string strQuery, List<string> arrValues, string MSConParam, List<string> param_array)
        {
            try
            {
                List<string> arr_params = param_array;
                MSParams msparam;
                MySqlParameter[] mysqlparams = new MySqlParameter[arr_params.Count];
                for (int i = 0; i < arr_params.Count; i++)
                {
                    msparam = MSParams.getMSParams(arr_params[i]);
                    mysqlparams[i] = new MySqlParameter(arr_params[i], msparam.DbType, msparam.ParamSize);
                    mysqlparams[i].Value = arrValues[i];
                }
                return MySqlHelper.ExecuteNonQuery(MSConParam, strQuery, mysqlparams);
            }
            catch (Exception exc)
            {
                //logWrt.asyncWrite(strQuery + "\n" + exc.ToString());
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, new System.Diagnostics.StackTrace(true), "query failed: " + strQuery + "::values : " + string.Join(",", arrValues), "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="first_half_query">for Ex- insert into table(column1,column2,column3,...) values</param>
        /// <param name="next_query">for ex- (value1,value2,value3,....) 
        /// The values which are supposed to pass by DataTable can be pass like (?table.columnName,?table.columnName,..)</param>
        /// <param name="data_val">is a DataTable having values for multiple columns</param>
        /// <param name="MSCon">is connection string</param>
        /// <param name="useSqlInjection"></param>
        /// <param name="len_of_one_row"></param>
        /// 
        public static void DL_DoBulkInsert(string first_half_query, List<string> next_query, DataTable data_val, string MSCon, bool useSqlInjection = true, int len_of_one_row = 100)
        {
            try
            {
                List<string> data;// = data_val;
                List<string> query_data;

                //1000000 bytes(1MB) is the max allowed packet for Mysql
                //len_of_one_row is in bytes
                //we are retriving no of rows to be execute in one bulk insert

                double abc = 1000000 / len_of_one_row;
                int no_of_row = Int32.Parse(Math.Ceiling(abc) + "");
                int len = 0;
                int i;

                while (true)
                {
                    if (len >= data_val.Rows.Count) break;
                    query_data = new List<string>();
                    data = new List<string>();
                    int count = 0;
                    List<string> param_array = new List<string>();
                    for (i = len; i < no_of_row + len; i++)
                    {
                        if (i >= data_val.Rows.Count) break;
                        for (int c = 0; c < data_val.Columns.Count; c++)
                        {
                            string data_query = "(";
                            for (int j = 0; j < next_query.Count; j++)
                            {
                                if (next_query[j].IndexOf('?') > -1)
                                {
                                    //changing
                                    if (useSqlInjection)
                                    {
                                        data_query += next_query[j] + "@" + count;
                                        param_array.Add(next_query[j].Remove(0, 1) + "@" + count);
                                        data.Add(data_val.Rows[i].ItemArray[c].ToString());
                                    }
                                    else
                                        data_query += data_val.Rows[i].ItemArray[c].ToString();
                                    c++;
                                }
                                else
                                    data_query += next_query[j];

                                if (j < next_query.Count - 1)
                                    data_query += ",";
                            }
                            data_query += ")";
                            query_data.Add(data_query);
                            count++;
                        }
                    }
                    string query1 = string.Join(",", query_data.ToArray());
                    if (useSqlInjection)
                        DL_ExecuteNonQuery(first_half_query + query1, data, MSCon, param_array);
                    else
                        DL_ExecuteSimpleQuery(first_half_query + query1, MSCon);
                    System.Threading.Thread.Sleep(200);
                    len = i;
                }
            }
            catch (Exception exc)
            {
                ____logconfig.Log_Write(____logconfig.LogLevel.EXC, new System.Diagnostics.StackTrace(true), "query failed: " + first_half_query + "::values : " + string.Join(",", next_query), "DataLayer");
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "DataLayer");
            }
        }
        #endregion


        protected static string[] getQueryParameters(string strQuery)
        {
            Regex reg = new Regex("\\?[a-zA-Z0-9_]*.[a-zA-Z0-9_@]*");
            string param = "";
            foreach (Match m in reg.Matches(strQuery))
                param += m.Value.TrimStart('?') + ",";
            string[] arr = param.TrimEnd(',').Split(',');
            return arr;
        }
    }
}
