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

        public View() { cont = new Controller(); }

        public void DrawUi()
        {
            while (true)
            {
                string[] options = { "Play", "Login", "Register", "Exit" };
                WriteAllOptions(options);
                WriteLine("Enter the desired number");
                switch (IntNoNullInput())
                {
                    case 1:
                        DrawScoreAllQuestionAnswer();
                        break;
                    case 2:
                        AfterLoginOrRegister(Login());
                        break;
                    case 3:
                        AfterLoginOrRegister(Register());
                        break;
                    case 4:
                        Environment.Exit(1);
                        break;
                    default:
                        break;
                }
            }
        }
        public void AfterLoginOrRegister(UserModel currentUserModel)
        {
            if (currentUserModel == null) { return; }

            while (true) 
            {
                string[] options = { "Play", "Add a new question", "Delete a question", "Logout" };
                WriteAllOptions(options);
                WriteLine("Enter the desired number");
                switch (IntNoNullInput())
                {
                    case 1:
                        DrawScoreAllQuestionAnswer();
                        break;
                    case 2:
                        AddNewQuestionAnswer(currentUserModel);
                        break;
                    case 3:
                        DeleteQuestionById();
                        break;
                    case 4:
                        return;
                    default:
                        break;
                }
            }
        }
        public UserModel Login()
        {
            UserController userController = new UserController();

            WriteLine("Username: ");
            string newUserName = StringNoNullInput();
            WriteLine("Password: ");
            string newPassword = StringNoNullInput();

            UserModel currentUserModel = new UserModel(newUserName, newPassword);

            return CheckLoginDetails(currentUserModel, userController);

        }
        public UserModel CheckLoginDetails(UserModel currentUserModel, UserController userController)
        {
            UserModel returnUserModel = userController.ContSelectUserByLogin(currentUserModel);
            if (returnUserModel == null) { LoginNotFound(); return null; }
            return returnUserModel;
        }
        //I don't know how to do this better
        public void LoginNotFound()
        {
            // Implement remaning tries
            WriteLine("Wrong username or password");
        }
        public UserModel Register()
        {
            UserController userController = new UserController();

            WriteLine("Username: ");
            string newUserName = StringNoNullInput();
            WriteLine("Password: ");
            string newPassword = StringNoNullInput();
            WriteLine("Password Again: ");
            string newPasswordAgain = StringNoNullInput();

            while (newPassword != newPasswordAgain)
            {
                WriteLine("Password Again: ");
                newPasswordAgain = StringNoNullInput();
            }

            UserModel newUsermodel = new UserModel(newUserName, newPassword);
            UserModel ?AddedUserModel = null;
            try
            {
                AddedUserModel = userController.AddReturnNewUser(newUsermodel);
                if (AddedUserModel != null)
                {
                    WriteLine("Successfully registered");
                }
                else
                {
                    WriteLine("User already exists");
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.ToString());
            }
            return AddedUserModel;
        }
        public void AddNewQuestionAnswer(UserModel currentUserModel)
        {
            WriteLine("Question: ");
            string newQuestion = StringNoNullInput();
            WriteLine("Answer: ");
            string newAnswer = StringNoNullInput();
            if (cont.NewQuestionAnswer(newQuestion, newAnswer, currentUserModel) == 0)
            {
                WriteLine("Succesfully added new question");
            }
            else
            {
                WriteLine("Failed to add new question or question already exists");
            }
        }
        //This isn't as secure as i wanted it to be, because it isn't saved who deleted it
        public void DeleteQuestionById()
        {
            WriteLine("Question: ");
            string question = StringNoNullInput();
            if (cont.DeleteQuestionAnswerById(question) == 0) 
            {
                WriteLine("Succesfully deleted");
            }
            else
            {
                WriteLine("Failed to delete");
            }
        }
        public void DrawScoreAllQuestionAnswer()
        {
            Controller newController = new Controller();
            WriteSetNumberOfQuestions(newController);

            for (int i = 0; i < newController.GetNumberOfQuestions(); i++)
            {
                newController.TestAnswer(WriteGetQuestionAnswer(newController));
            }

            WriteLine(string.Format("Your score: {0} / {1}", newController.RightScore(), newController.GetNumberOfQuestions()));
        }
        public void WriteSetNumberOfQuestions(Controller currentController)
        {
            WriteLine("Number of questions: " + currentController.GetNumberOfQuestions());
            currentController.SetNumberOfQuesitons(IntNoNullInput()); //int.parse could also be used
        }
        public QuestionAnswerModel WriteGetQuestionAnswer(Controller currentController)
        {
            QuestionAnswerModel answerModel = currentController.RandomQuestionAnswer();

            WriteLine(string.Format("Question: {0}", answerModel.Question));

            answerModel.UserAnswer = StringNoNullInput();
            return answerModel;
        }
        public int IntNoNullInput()
        {
            string ?input = ReadLine();
            while (string.IsNullOrEmpty(input) || !int.TryParse(input, out _))
            {
                WriteLine("Please enter a number: ");
                input = ReadLine();
            }
            return Convert.ToInt32(input);
        }
        public string StringNoNullInput()
        {
            string ?input = ReadLine();
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
