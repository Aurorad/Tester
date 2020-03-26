using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester1._0._1
{
   public class ChooseRightOne : Question
    {
        public List<varForChooseRightOne> options { set; get; }
        public ChooseRightOne(int number, string question, float score)
        {
            this.number = number;
            this.question = question;
            this.score = score;
            options = new List<varForChooseRightOne>();
        }

        public ChooseRightOne(Question q, List<varForChooseRightOne> listForChooseRightOnes)
        {
            number = q.number;
            question = q.question;
            score = q.score;
            options = listForChooseRightOnes;
        }
        public void AddListOptions(List<varForChooseRightOne> list)
        {
            options = list;
        }
        public override string Show()
        {
            string strToReturn = number + "\t" + score + "\n" + question + "\n";
            
            for (int i=0; i<options.Count; i++)
            {
                strToReturn += options[i].answer + "\t" + options[i].answerOption;
            }
            return strToReturn;
        }
    }
}
