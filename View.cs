using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Quiz
{
    internal class View
    {
        private Controller cont;

        public View() { cont = new(); }

        public void DrawUi()
        {
            while (true)
            {
                string[] options = { "Play", "Add More Questions", "Exit" };
                WriteAllOptions(options);
                WriteLine("Enter the desired number");
                switch (NoNullInput())
                {
                    case "1":
                        DrawScoreAllQuestionAnswer();
                        break;
                    case "2":
                        AddNewQuestionAnswer();
                        break;
                    case "3":
                        Environment.Exit(1);
                        break;
                    default:
                        break;
                }

            }
        }
        public void AddNewQuestionAnswer()
        {
            WriteLine("Question: ");
            string newQuestion = NoNullInput();
            WriteLine("Answer: ");
            string newAnswer = NoNullInput();
            if (cont.NewQuestionAnswer(newQuestion, newAnswer) == 0)
            {
                WriteLine("Succesfully added new question");
            }
            else
            {
                WriteLine("Failed to add new question");
            }
        }
        public void DrawScoreAllQuestionAnswer()
        {
            Controller newController = new();
            WriteSetNumberOfQuestions(newController);

            for (int i = 0; i < newController.GetNumberOfQuestions(); i++)
            {
                newController.TestAnswer(WriteGetQuestionAnswer(newController));
            }

            WriteLine(string.Format("Your score: {0} / {1}", newController.RightScore(), newController.GetNumberOfQuestions()));
        }
        public void WriteSetNumberOfQuestions(Controller currentController)
        {
            WriteLine("Number of questions");
            currentController.SetNumberOfQuesitons(int.Parse(NoNullInput()));
        }
        public QuestionAnswerModel WriteGetQuestionAnswer(Controller currentController)
        {
            QuestionAnswerModel answerModel = currentController.RandomQuestionAnswer();

            WriteLine(string.Format("Question: {0}", answerModel.Question));

            answerModel.UserAnswer = NoNullInput();
            return answerModel;
        }
        public string NoNullInput()
        {
            string input = ReadLine();
            while (string.IsNullOrEmpty(input))
            {
                WriteLine("Please enter: ");
                input = ReadLine();
            }
            return input;
        }
        public void WriteAllOptions(string[] allOptions)
        {
            for (int i = 0; i < allOptions.Length; i++)
            {
                WriteLine(string.Format("{0}. - {1}", i + 1, allOptions[i]));
            }
        }
    }
}
