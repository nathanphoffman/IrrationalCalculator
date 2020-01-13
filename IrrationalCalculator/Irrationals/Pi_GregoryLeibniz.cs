using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace IrrationalCalculator.Irrationals
{
    internal class Pi_GregoryLeibniz : Base.BaseIrrational
    {
        internal Pi_GregoryLeibniz() : base("Pi","The Gregory-Leibniz Conversion Series", 3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679m, (x,y)=>x+y) { }
        
        internal override decimal IterationCalculation(decimal i)
        {
            decimal first = 4 / ((i -1)*3 + i);
            decimal second = 4 / (((i - 1) * 3 + i) + 2);
            return first - second;
        }
    }
}
