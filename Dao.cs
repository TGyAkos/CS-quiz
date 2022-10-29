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
        private static readonly string COUNT_QUESTION_ANSWER_BY_QUESTION = "SELECT COUNT(question) FROM `answer_question` WHERE question = ?Question";
        private static readonly string INSERT_NEW_QUESTION_ANSWER = "INSERT INTO `answer_question` (`id`, `uuid`, `question`, `answer` ) VALUES (NULL, ?UUID, ?question , ?answer)";
        private static readonly string DELETE_QUESTION_BY_ID = "DELETE FROM `answer_question` WHERE question = ?question";
        private static readonly string CREATE_RELEVANT_TABLES = @"
                CREATE TABLE IF NOT EXISTS `answer_question`(
                id INT NOT NULL AUTO_INCREMENT,
                uuid VARCHAR(36) NOT NULL,
                question VARCHAR(250) NOT NULL,
                answer VARCHAR(50) NOT NULL,
                PRIMARY KEY (id)
                )";

        public Dao() { MySqlConnection conn = Connection.CreateConnection(); CreateRelevantTables(conn); }

        public QuestionAnswerModel[] AllQuestionAnswers()
        {
            MySqlConnection conn = Connection.CreateConnection();

            //Console.WriteLine(GetAllRows(conn));
            //needs to be here otherwise the Sql connection is bound
            QuestionAnswerModel[] a = new QuestionAnswerModel[GetAllRows(conn)];

            MySqlDataReader rdr = GetMySqlCommand(SELECT_ALL, conn).ExecuteReader();//<-- To this

            //https://stackoverflow.com/questions/30600370/why-is-datareader-giving-enumeration-yielded-no-results

            int counter = 0;
            while (rdr.Read())
            {
                QuestionAnswerModel ModelFromDataBase = new(rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(1).ToString());
                //Console.WriteLine(ModelFromDataBase.ToString());
                a[counter] = ModelFromDataBase;
                counter++;
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
            if (CheckNewQuestionAnswer(newQuestionAnswerModel) == 1) { return 1; }

            MySqlConnection conn = Connection.CreateConnection();

            try
            {
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = INSERT_NEW_QUESTION_ANSWER;
                comm.Parameters.AddWithValue("?uuid", currentUserModel.UUID);
                comm.Parameters.AddWithValue("?question", newQuestionAnswerModel.Question);
                comm.Parameters.AddWithValue("?answer", newQuestionAnswerModel.Answer);
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
        public int CheckNewQuestionAnswer(QuestionAnswerModel newQuestionAnswerModel)
        {
            MySqlConnection conn = Connection.CreateConnection();
            MySqlCommand comm = conn.CreateCommand();

            comm.CommandText = COUNT_QUESTION_ANSWER_BY_QUESTION;
            comm.Parameters.AddWithValue("?Question", newQuestionAnswerModel.Question);
            MySqlDataReader rdr = comm.ExecuteReader();

            rdr.Read();
            if (Convert.ToInt32(rdr.GetValue(0)) >= 1) { return 1; }

            rdr.Close();
            Connection.CloseConnection(conn);
            return 0;
        }
        //Should this be an obj parameter or is this good enough?
        public int DeleteQuestionAnswerById(string question)
        {
            MySqlConnection conn = Connection.CreateConnection();
            MySqlCommand comm = conn.CreateCommand();

            try
            {
                comm.CommandText = DELETE_QUESTION_BY_ID;
                comm.Parameters.AddWithValue("question", question);
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
