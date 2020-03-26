using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester1._0._1
{
    public class varForChooseRightOne
    {
        public int number { set; get; }
        public string answerOption { set; get; }//Варіант відповіді
        public bool answer { set; get; }//правильність варіанту (правильний/не правильний)
        public varForChooseRightOne(int number, string answerOption, bool answer)
        {
            this.number = number;
            this.answerOption = answerOption;
            this.answer = answer;
        }
        public string Show()
        {
            string str = number + "\t" + answerOption + "\t" + answer;
            return str;
        }
    }
}
