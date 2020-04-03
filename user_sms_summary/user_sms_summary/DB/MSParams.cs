using System;
using System.Collections.Generic; using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Collections;


namespace user_sms_summary
{
    /// <summary>
    /// Represents column name with its MySqlDbType <para>and size defined in Mysql Database.</para>
    /// </summary>
    public class MSParams
    {
        #region Variables

        private string _ParamName;
        private MySqlDbType _dbtype;
        private int _paramsize;

        #endregion

        #region Properties
        public string ParamName
        {
            get { return this._ParamName; }
        }
        public MySqlDbType DbType
        {
            get { return this._dbtype; }
        }
        public int ParamSize
        {
            get { return this._paramsize; }
        }
        #endregion

        #region Constructors

        public MSParams(string _ParamName, MySqlDbType _dbtype, int _paramsize)
        {
            this._ParamName = _ParamName;
            this._dbtype = _dbtype;
            this._paramsize = _paramsize;
        }
        public MSParams(string _ParamName, MySqlDbType _dbtype)
        {
            this._ParamName = _ParamName;
            this._dbtype = _dbtype;
        }

        #endregion

        #region The static Dictionary

        /// <summary>
        /// A static Dictionary holding mysql parameter name as keys and an MSParam object which holds MySqlDbType and size as its Value.
        /// </summary>
        public static Dictionary<string, MSParams> allparams = new Dictionary<string, MSParams>() { 
            /***************************************************
             * Global Values 
             * *************************************************/
            { "global.bigint" ,                         new MSParams("global.bigint",MySqlDbType.UInt64)}, 
            { "global.int" ,                            new MSParams("global.int" , MySqlDbType.Int32)},                          
            { "global.text" ,                           new MSParams("global.text",MySqlDbType.Text)},
            { "global.datetime" ,                       new MSParams("global.date",MySqlDbType.DateTime)},
            { "global.date" ,                           new MSParams("global.date",MySqlDbType.Date)},
            { "global.time" ,                           new MSParams("global.time",MySqlDbType.Time)},
            
            /******************************************************/

        };

        #endregion

        #region Methods

        /// <summary>
        /// Returns object of MSParams
        /// </summary>
        /// <param name="paramname">A string that represents the param name to be found in dictionary</param>
        /// <returns>Gets a MSParam object with its MysqlDbType and size.</returns>
        public static MSParams getMSParams(string paramname)
        {
            MSParams msOut;
            if (paramname.Contains("@"))
                paramname = paramname.Split('@')[0];
            try
            {
                allparams.TryGetValue(paramname, out msOut);
                return msOut;
            }
            catch (Exception exc)
            {
                string error = exc.ToString();
            }
            return null;
        }


        #endregion
    }
}
