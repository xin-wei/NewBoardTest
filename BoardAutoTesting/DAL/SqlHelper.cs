using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace BoardAutoTesting.DAL
{
    /// <summary>
    /// IT给的库中存在bug：在多线程并发访问的情况下，文件加解密的速度跟不上并发读取的速度
    /// 造成加解密错误，从而影响函数调用
    /// 这个bug存在于日志读写中，故重写这部分代码
    /// </summary>
    public class SqlHelper
    {
        private static readonly string _strConn =
            "Database = centercontrol; Data Source = 127.0.0.1; User Id = root; Password = ; Port = 3306";

        public static string StrConn
        {
            get { return _strConn; }
        }

        public static int ExecuteNonQuery(string connectionString, CommandType cmdType,
            string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand mySqlCommand = new MySqlCommand();
            int result;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    PrepareCommand(mySqlCommand, mySqlConnection, null, 
                        cmdType, cmdText, commandParameters);
                    mySqlCommand.CommandTimeout = 84100;
                    int num = mySqlCommand.ExecuteNonQuery();
                    mySqlCommand.Parameters.Clear();
                    result = num;
                }
            }
            catch (MySqlException ex)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
            return result;
        }

        public static DataSet GetDataSet(string connectionString, CommandType cmdType, 
            string cmdText, params MySqlParameter[] commandParameters)
        {
            DataSet dataSet = new DataSet();
            MySqlCommand mySqlCommand = new MySqlCommand();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    PrepareCommand(mySqlCommand, mySqlConnection, null, 
                        cmdType, cmdText, commandParameters);
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
                    mySqlDataAdapter.Fill(dataSet);
                    mySqlCommand.Parameters.Clear();
                }
            }
            catch (MySqlException ex)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
            return dataSet;
        }

        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn,
            MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            if (cmdParms == null) return;

            foreach (MySqlParameter value in cmdParms.Where(value => !cmd.Parameters.Contains(value)))
            {
                cmd.Parameters.Add(value);
            }
        }
    }
}