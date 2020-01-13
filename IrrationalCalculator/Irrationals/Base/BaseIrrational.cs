using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace IrrationalCalculator.Irrationals.Base
{
    internal abstract class BaseIrrational
    {
        // ControlValue is the target and is used to test the accuracy of the calculation
        internal decimal ControlValue { get; private set; }
        internal decimal Sequence { get; private set; } = 0;
        internal string Description { get; private set; }
        internal string Name { get; private set; }

        //internal TaskIterations = iterations done inside each task, baseIteratons = how many tasks to spawn and combine results so we can get our sequence
        internal int TaskIterations, InternalIterations;
        private Func<decimal, decimal, decimal> Combiner; // used to join sequences together, some alg. will require * or +, etc.

        // i prefer `this` for clarity, 
        internal BaseIrrational(string name, string description, decimal controlValue, Func<decimal, decimal, decimal> combiner)
        {
            this.Name = name;
            this.Description = description;
            this.ControlValue = controlValue;
            this.Combiner = combiner;
        }

        abstract internal decimal IterationCalculation(decimal iteration);

        // some calculations give a pi/2 or some similar fraction of the value you are looking for, or add a final sum, this is for final calculation once the sequence is complete, some calcs will not need this:
        virtual internal decimal CalculateFinalSum() { return Sequence; }

        // for/while or recur. is fine here, I don't think recur is good in this case because of the amount of stack pollution will cause overflow
        internal decimal RunCalculation(int iteration)
        {

            decimal sequence = 0;

            for (int i = 1; i <= InternalIterations; i++)
            {
                decimal calc = IterationCalculation((decimal)i + InternalIterations * (iteration - 1));
                sequence = sequence == 0 ? calc : Combiner(sequence, calc);
            }

            return sequence;
        }

        internal async Task<decimal> GetSequence()
        {

            Task<decimal>[] calculations = new Task<decimal>[TaskIterations];

            for (int i = 1; i <= TaskIterations; i++)
            {
                int cloneI = i; // clone is necessary because run is async and may fetch other i values depending on the timing of the Task.Run spawn
                calculations[i - 1] = Task.Run(() => RunCalculation(cloneI));
            }

            await Task.WhenAll(calculations);
            decimal[] values = calculations.Select(x => x.Result).ToArray();
            Sequence = Sequence == 0 ? values.Aggregate(Combiner) : Combiner(Sequence, values.Aggregate(Combiner));

            // uses Sequence above^
            return CalculateFinalSum();

        }
    }
}
