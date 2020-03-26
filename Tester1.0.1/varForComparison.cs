using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester1._0._1
{
    
    /// <summary>
    /// клас-змінна для варіантів співставлення
    /// </summary>
    public class varForComparison
    {
        public int number { set; get; }
        public string var1 { set; get; }
        public string var2 { set; get; }
        public varForComparison(int number, string var1, string var2)
        {
            this.number = number;
            this.var1 = var1;
            this.var2 = var2;
        }
        public string Show()
        {
            string strToReturn = number + "\t" + var1 + "\t" + var2;
            return strToReturn;
        }
    }
}
