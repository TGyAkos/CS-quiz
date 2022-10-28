using MySql.Data.MySqlClient;
using Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_quiz
{
    internal class UserDao
    {
        private static readonly string SELECT_USER_BY_LOGIN = "SELECT uuid FROM `user_model` WHERE username = ?UserName AND password = ?Password";
        private static readonly string ADD_USER = "INSERT INTO `user_model` (uuid, username, password) VALUES (?UUID, ?UserName, ?Password)";
        private static readonly string CREATE_RELEVANT_TABLES = @"
                CREATE TABLE IF NOT EXISTS `user_model`(
                uuid VARCHAR(36) NOT NULL,
                username VARCHAR(250) NOT NULL,
                password VARCHAR(50) NOT NULL,
                PRIMARY KEY (uuid)
                )";
        public UserDao() { MySqlConnection conn = Connection.CreateConnection(); CreateRelevantTables(conn); }
        public UserModel SelectUserByLogin(UserModel currentUserModel)
        {
            MySqlConnection conn = Connection.CreateConnection();
            MySqlCommand comm = conn.CreateCommand();

            comm.CommandText = SELECT_USER_BY_LOGIN;
            comm.Parameters.AddWithValue("?UUID", currentUserModel.UUID);
            comm.Parameters.AddWithValue("?UserName", currentUserModel.UserName);
            comm.Parameters.AddWithValue("?Password", currentUserModel.Password);
            MySqlDataReader rdr = comm.ExecuteReader();

            while (rdr.Read())
            {
                currentUserModel.UUID = rdr.GetValue(0).ToString();
                Console.WriteLine(currentUserModel.ToString());
            }

            rdr.Close();
            Connection.CloseConnection(conn);
            return currentUserModel;
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
