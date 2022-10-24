using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz
{
    internal class QuestionAnswerModel
    {
        public string ?Question { get; }
        public string ?Answer { get; }
        public string ?UserAnswer { get; set; }

        public QuestionAnswerModel(string newQuestion, string newAnswer)
        {
            Question = newQuestion;
            Answer = newAnswer;
        }

        public override string ToString() {
            return string.Format("Model: {0} {1} {2} ", Question, Answer, UserAnswer);
        }
    }
}