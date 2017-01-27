using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftTrivia.Models
{
    class Question
    {
        public string question;
        public string a;
        public string b;
        public string c;
        public string d;
        public string correct;

        public Question()
        {
            Answers = new string[4];
        }

        public Question(string question, string a, string b, string c, string d, string correct)
        {
            this.question = question;
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.correct = correct;
        }

        public string QuestionText { get; set; }
        public string[] Answers { get; set; }
        public int CorrectAnswer { get; set; }

    }
}
