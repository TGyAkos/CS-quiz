using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz
{
    class Model
    {
        public int Id { get; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public Model(int newId)
        {
            Id = newId;
        }
    }
}
