using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IrrationalCalculator
{
    internal static class ExtensionMethods
    {
        internal static string Repeat(this String str, int count)
        {
            return string.Concat(Enumerable.Repeat(str, count));
        }
    }
}
