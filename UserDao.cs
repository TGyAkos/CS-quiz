using MySql.Data.MySqlClient;
using Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    internal class UserDao
    {
        private static readonly string SELECT_USER_BY_LOGIN = "SELECT uuid FROM `user_model` WHERE username = ?UserName AND password = ?Password";
        private static readonly string COUNT_USER_BY_USERNAME = "SELECT COUNT(username) FROM `user_model` WHERE username = ?UserName";
        private static readonly string ADD_USER = "INSERT INTO `user_model` (uuid, username, password) VALUES (?UUID, ?UserName, ?Password)";
        private static readonly string CREATE_RELEVANT_TABLES = @"
                CREATE TABLE IF NOT EXISTS `user_model`(
                uuid VARCHAR(36) NOT NULL,
                username VARCHAR(250) NOT NULL,
                password VARCHAR(128) NOT NULL,
                PRIMARY KEY (uuid)
                )";
        public UserDao() { MySqlConnection conn = Connection.CreateConnection(); CreateRelevantTables(conn); }
        public UserModel SelectUserByLogin(UserModel currentUserModel)
        {
            MySqlConnection conn = Connection.CreateConnection();
            MySqlCommand comm = conn.CreateCommand();

            comm.CommandText = SELECT_USER_BY_LOGIN;
            comm.Parameters.AddWithValue("?UserName", currentUserModel.UserName);
            comm.Parameters.AddWithValue("?Password", currentUserModel.Password);
            MySqlDataReader rdr = comm.ExecuteReader();

            while (rdr.Read())
            {
                rdr.GetValue(0).ToString();
                currentUserModel.UUID = rdr.GetValue(0).ToString();
                //Console.WriteLine(currentUserModel.ToString());
            }

            rdr.Close();
            Connection.CloseConnection(conn);
            if (string.IsNullOrEmpty(currentUserModel.UUID)) { return null; }
            return currentUserModel;
        }
        public int CheckNewUserModel(UserModel newUserModel)
        {
            MySqlConnection conn = Connection.CreateConnection();
            MySqlCommand comm = conn.CreateCommand();

            comm.CommandText = COUNT_USER_BY_USERNAME;
            comm.Parameters.AddWithValue("?UserName", newUserModel.UserName);
            MySqlDataReader rdr = comm.ExecuteReader();

            rdr.Read();
            if (Convert.ToInt32(rdr.GetValue(0)) >= 1) { return 1; }

            rdr.Close();
            Connection.CloseConnection(conn);
            return 0;

        }
        public int InsertNewUserModel(UserModel newUserModel)
        {
            if (CheckNewUserModel(newUserModel) == 1) { return 1; }

            MySqlConnection conn = Connection.CreateConnection();
            MySqlCommand comm = conn.CreateCommand();
            
            try
            {
                comm.CommandText = ADD_USER;
                comm.Parameters.AddWithValue("?UUID", newUserModel.UUID);
                comm.Parameters.AddWithValue("?UserName", newUserModel.UserName);
                comm.Parameters.AddWithValue("?Password", newUserModel.Password);
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }

            Connection.CloseConnection(conn);
            return 0;
        }
        public void CreateRelevantTables(MySqlConnection conn)
        {
            GetMySqlCommand(CREATE_RELEVANT_TABLES, conn).ExecuteNonQuery();
            Connection.CloseConnection(conn);
        }
        public MySqlCommand GetMySqlCommand(string SqlString, MySqlConnection conn)
        {
            MySqlCommand cmd = new(SqlString, conn);
            return cmd;
        }
    }
}
