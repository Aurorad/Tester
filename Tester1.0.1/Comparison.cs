using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester1._0._1
{
    public class Comparison : Question
    {
        public List<varForComparison> options { set; get; }
        public Comparison(int number, string question, float score)
        {
            this.number = number;
            this.question = question;
            this.score = score;
            options = new List<varForComparison>();
        }
        public Comparison(Question q, List<varForComparison> listForComparisons)
        {
            number = q.number;
            question = q.question;
            score = q.score;
            options = listForComparisons;
        }
        public void AddListOptions(List<varForComparison> list)
        {
            this.options = list;
        }
        public override string Show()
        {
            string strToReturn = number + "\t" + score + "\n" + question;
            
            for (int i = 0; i < options.Count; i++)
            {
                strToReturn += options[i].var1 + "\t" + options[i].var2;
            }
            return strToReturn;
        }
    }
}
