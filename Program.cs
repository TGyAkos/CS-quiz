using System;


namespace Quiz
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Dao dao = new();
            Model[] a = dao.AllQuestionAnswers();
            Console.WriteLine(a);
            Console.WriteLine("Hello World!");
            Controller cont = new();
            Console.WriteLine(cont.RandomQuestionAnswer());
            cont.RightScore(1);
            Console.WriteLine(cont.RightScore());*/
            Console.WriteLine(!int.TryParse(null, out _));
            View vw = new();
            vw.DrawUi();
        }
    }
}
/*
 * https://stackoverflow.com/questions/17285071/mysql-get-number-of-rows
 * https://stackoverflow.com/questions/20940979/what-is-an-indexoutofrangeexception-argumentoutofrangeexception-and-how-do-i-f
 */
