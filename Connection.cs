using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Quiz
{
    internal class Connection
    {
        public static MySqlConnection CreateConnection()
        {
            string connStr = "server=localhost;user=root;database=quiz;port=3306;password=";
            MySqlConnection conn = new(connStr);
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
    }
}
