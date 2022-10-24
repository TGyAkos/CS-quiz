/*sing System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    internal class AnswerController
    {
        private AnswerModel answerModel;
        private Controller cont;
        public AnswerController()
        {
            cont = new();
        }
        public AnswerModel GetAnswerModel() => answerModel;
        public void NewAnswerModelInstance() => answerModel = new(cont.RandomQuestionAnswer());
        public void SetAnswerModelAnswer(string answer) => answerModel.Answer = answer;
    }
}
*/