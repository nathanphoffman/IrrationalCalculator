using System;
using System.Collections.Generic;
using System.Text;

namespace IrrationalCalculator.Irrationals
{
    internal class Pi_Nilakantha : Base.BaseIrrational
    {
        internal Pi_Nilakantha() : base("Pi", "The Nilakantha Conversion Series", 3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679m, (x, y) => x + y) { }

        internal override decimal IterationCalculation(decimal i)
        {
            decimal i4 = i * 4;
            decimal first = 4 / ((i4-2) * (i4 - 1) * i4);
            decimal second = 4 / (i4 * (i4 + 1) * (i4 + 2));
            return first - second;
        }

        internal override decimal CalculateFinalSum()
        {
            return Sequence + 3;
        }

    }
}
