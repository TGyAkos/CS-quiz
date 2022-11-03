using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace Quiz
{
    internal class Connection
    {
        private static readonly string CREATE_DATABASE = "CREATE DATABASE IF NOT EXISTS quiz";
        private static readonly string connStrWoDb= "server=localhost;user=tanulo;port=3306;password=tanulo";
        private static readonly string connStr = "server=localhost;user=tanulo;database=quiz;port=3306;password=tanulo";
        public static MySqlConnection CreateConnection()
        {
            CreateDataBase();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to db...");
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return conn;
        }
        public static void CloseConnection(MySqlConnection conn)
        {
            Console.WriteLine("db closed");
            conn.Close();
        }
        public static void CreateDataBase()
        {
            MySqlConnection conn = new MySqlConnection(connStrWoDb);

            try
            {
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = CREATE_DATABASE;
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            conn.Close();
        }
    }
}
