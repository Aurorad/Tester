using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester1._0._1
{
    public class Question
    {
        public int number { set; get; }
        public string question { set; get; }
        public float score { set; get; }//к-сть балів за питання
        public virtual string Show()
        {
            string str = number + "\t" + score + "\n" + question;
            return str;
        }
       
    }

    
    
}
