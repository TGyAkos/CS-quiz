using System;
using System.Linq;
namespace Quiz
{
    internal class Controller
    {
        private Dao dao;
        private Random rnd;
        private ScoreModel scr;
        private QuestionAnswerModel[] AllQuestionAnswer;
        private QuestionAnswerModel[] AllQuestionAnswerRandom;
        private int NumberOfQuestions;
        private int ModelCounter;

        public Controller()
        {
            dao = new Dao();
            rnd = new Random();
            scr = new ScoreModel();
            AllQuestionAnswer = dao.AllQuestionAnswers();
            AllQuestionAnswerRandom = RandomModelArray(AllQuestionAnswer);
            NumberOfQuestions = AllQuestionAnswer.Length;
            ModelCounter = 0;
        }
        public int RightScore() => scr.Right;
        public int WrongScore() => scr.Wrong;
        public void RightScore(int right) => scr.Right += right;
        public void WrongScore(int wrong) => scr.Wrong += wrong;
        public int GetNumberOfQuestions() => NumberOfQuestions;
        public void SetNumberOfQuesitons(int Number) => NumberOfQuestions = Number > NumberOfQuestions ? NumberOfQuestions : Number;
        public QuestionAnswerModel RandomQuestionAnswer()
        {
            QuestionAnswerModel ToBeReturned;
            if (ModelCounter != NumberOfQuestions)
            {
                ToBeReturned = AllQuestionAnswerRandom[ModelCounter];
                ModelCounter++;
                return ToBeReturned;
            }
            else if (ModelCounter == NumberOfQuestions)
            {
                ModelCounter = 0;
            }
            return null;
        }
        public QuestionAnswerModel[] RandomModelArray(QuestionAnswerModel[] ModelArray)
        {
            return ModelArray.OrderBy(x => rnd.Next()).ToArray();
        }
        public void TestAnswer(QuestionAnswerModel CurrentQuestionAnswer)
        {
            if (CurrentQuestionAnswer.Answer == CurrentQuestionAnswer.UserAnswer)
            {
                RightScore(1);
            }
            else
            {
                WrongScore(1);
            }
        }
        public int NewQuestionAnswer(string newQuestion, string newAnswer, UserModel currentUserModel)
        {
            try
            {
                QuestionAnswerModel newQuestionAnswerModel = new QuestionAnswerModel(newQuestion, newAnswer);
                if (dao.InsertNewQuestionAnswer(newQuestionAnswerModel, currentUserModel) == 1) { return 1; }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }
        public int DeleteQuestionAnswerById(string question) => dao.DeleteQuestionAnswerById(question);
    }
}
