using System;
using System.Collections.Generic;
using System.Text;

namespace IrrationalCalculator.Irrationals
{
    internal class Pi_JohnWallis : Base.BaseIrrational
    {
        internal Pi_JohnWallis() : base("Pi", "The John Wallis Conversion Series", 3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679m, (x, y) => x * y) { }

        internal override decimal IterationCalculation(decimal i)
        {
            decimal first = (i * 2) / (i * 2 - 1);
            decimal second = (i * 2) / (i * 2 + 1);
            return first * second;
        }

        internal override decimal CalculateFinalSum()
        {
            return Sequence * 2;
        }
    }
}
