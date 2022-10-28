using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Text;

namespace Quiz
{
    internal class Dao
    {
        private static readonly string SELECT_ALL = "SELECT * FROM `answer_question`";
        private static readonly string COUNT_ALL = "SELECT COUNT(*) FROM `answer_question`";
        private static readonly string INSERT_NEW_QUESTION_ANSWER = "INSERT INTO `answer_question` (`id`, `question`, `answer`) VALUES (NULL, ?question , ?answer, ?UUID)";
        private static readonly string CREATE_RELEVANT_TABLES = @"
                CREATE TABLE IF NOT EXISTS `answer_question`(
                id INT NOT NULL AUTO_INCREMENT,
                question VARCHAR(250) NOT NULL,
                answer VARCHAR(50) NOT NULL,
                uuid VARCHAR(36) NOT NULL,
                PRIMARY KEY (id)
                )";

        public Dao() { MySqlConnection conn = Connection.CreateConnection(); CreateRelevantTables(conn); }

        public QuestionAnswerModel[] AllQuestionAnswers()
        {
            MySqlConnection conn = Connection.CreateConnection();
            //needs to be here otherwise the Sql connection is bound
            Console.WriteLine(GetAllRows(conn));
            QuestionAnswerModel[] a = new QuestionAnswerModel[GetAllRows(conn)];

            MySqlDataReader rdr = GetMySqlCommand(SELECT_ALL, conn).ExecuteReader();//<-- To this

            //https://stackoverflow.com/questions/30600370/why-is-datareader-giving-enumeration-yielded-no-results

            while (rdr.Read())
            {
                QuestionAnswerModel ModelFromDataBase = new(rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString());
                Console.WriteLine(ModelFromDataBase.ToString());
                int b = Convert.ToInt32(rdr.GetValue(0)) - 1;
                a[b] = ModelFromDataBase;
            }

            rdr.Close();
            Connection.CloseConnection(conn);
            return a;
        }
        public int GetAllRows(MySqlConnection conn)
        {
            int AllRows;
            try
            {
                AllRows = Convert.ToInt32(GetMySqlCommand(COUNT_ALL, conn).ExecuteScalar());
                //Console.WriteLine("AllRows: " + AllRows);
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
                return 0;
            }
            return AllRows;
        }
        public void CreateRelevantTables(MySqlConnection conn)
        {
            GetMySqlCommand(CREATE_RELEVANT_TABLES, conn).ExecuteNonQuery();
            //https://stackoverflow.com/questions/22993857/create-table-in-mysql-from-c-sharp
            Connection.CloseConnection(conn);
        }
        public MySqlCommand GetMySqlCommand(string SqlString, MySqlConnection conn)
        {
            MySqlCommand cmd = new(SqlString, conn);
            return cmd;
        }
        public int InsertNewQuestionAnswer(QuestionAnswerModel newQuestionAnswerModel, UserModel currentUserModel)
        {
            MySqlConnection conn = Connection.CreateConnection();

            try
            {
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = INSERT_NEW_QUESTION_ANSWER;
                comm.Parameters.AddWithValue("?question", newQuestionAnswerModel.Question);
                comm.Parameters.AddWithValue("?answer", newQuestionAnswerModel.Answer);
                comm.Parameters.AddWithValue("?uuid", currentUserModel.UUID);
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
    }
}
