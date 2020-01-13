using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using IrrationalCalculator.Irrationals.Base;
using System.Diagnostics;

namespace IrrationalCalculator
{
    internal static class ConsoleRenderer
    {
        internal static void RenderMenu(string error = null)
        {
            Console.Clear();
            string line = "-".Repeat(75);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($@"
                { line }
                |    Irrational Calculator: Enter a # to calculate. ex: 1
                     If you want to compare the performance of multiple calcs separate by commas, ex: 0,1
                { line }
                ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {error ?? "None"}");

            // here we are building our instances of all classes in the root of the Irrationals folder to display for usage in the console / inputs, we ignore some system classes with generics <,>
            List<string> classNames = GetClasses("IrrationalCalculator.Irrationals").Where(x => !x.Contains('<')).ToList();
            var instances = classNames.Select(x =>
            {
                // activator gave me trouble because each derived class requires a non-default constructor which itself does not take any params, so here we do it manually with the constructor exposed:
                Type clsType = Type.GetType($"IrrationalCalculator.Irrationals.{x}");
                var construct = clsType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
                var obj = (BaseIrrational)construct.Invoke(new object[0]);
                return obj;
            }).ToList();

            // if error is null then we have no issue and continue, otherwise we redisplay the menu options with the error displayed
            error = null;
            List<BaseIrrational> selectedClasses = InputActions.GetInput(instances, out error);
            if (error != null) RenderMenu(error);
            selectedClasses.ForEach(RunCalculation);
            InputActions.Pause();
        }

        private static void RunCalculation(BaseIrrational selectedClass) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            decimal result = selectedClass.GetSequence().GetAwaiter().GetResult();
            stopwatch.Stop();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" ".Repeat(20) + "-".Repeat(50));
            Console.WriteLine(" ".Repeat(20) + selectedClass.Name + "-" + selectedClass.Description);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{" ".Repeat(20)}Result: {result}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{" ".Repeat(20)}Time took: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"{" ".Repeat(20)}Difference from control is: {string.Format("{0:E2}", result - selectedClass.ControlValue)}");
        }

        // taken from: https://stackoverflow.com/questions/79693/getting-all-types-in-a-namespace-via-reflection
        private static List<string> GetClasses(string nameSpace)
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            List<string> namespacelist = new List<string>();
            List<string> classlist = new List<string>();

            foreach (Type type in asm.GetTypes())
            {
                if (type.Namespace == nameSpace)
                    namespacelist.Add(type.Name);
            }

            foreach (string classname in namespacelist)
                classlist.Add(classname);

            return classlist;
        }
    }
}
