using System;
using System.Collections.Generic;
using System.Text;
using IrrationalCalculator.Irrationals.Base;
using System.Linq;

namespace IrrationalCalculator
{
    internal static class InputActions
    {
        internal static string ErrorCheck(string entry, int maxSelections)
        {
            string error = null;
            int entryInt = 0;
            if (!int.TryParse(entry, out entryInt))
            {
                error = "Please enter a numerical value in the menu.";
            }
            else if (entryInt >= maxSelections)
            {
                error = "The value you entered does not match the numerical values displayed above.";
            }
            return error;
        }

        internal static List<BaseIrrational> GetInput(List<BaseIrrational> instances, out string error)
        {

            int i = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" ".Repeat(20) + "-".Repeat(25));
            instances.ForEach((inst) =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($" ".Repeat(20) + $"#{i++} - {inst.Name}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" - " + inst.Description);
            });
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" ".Repeat(20) + "-".Repeat(25));
            Console.Write(" ".Repeat(20) + "Your Selection #: ");

            // we could have multiple entries so we handle that here:
            string entry = Console.ReadLine();
            string[] entries = entry.Split(',');

            List<string> errors = entries.Select(x => ErrorCheck(x, instances.Count)).Where(y => y != null)?.ToList();
            if (errors != null && errors.Count > 0)
            {
                error = errors.First();
                return null;
            }

            Console.Write($"{" ".Repeat(20)}Number of tasks to spawn (default: processor/thread count: {Environment.ProcessorCount}): #");
            string taskNumberStr = Console.ReadLine();

            if ((taskNumberStr ?? "") == "") taskNumberStr = Environment.ProcessorCount.ToString();
            int taskNumber = 0;

            Console.Write($"{" ".Repeat(20)}Number of calculations per task (default: 10000): #");
            string calcNumberStr = Console.ReadLine();

            if ((calcNumberStr ?? "") == "") calcNumberStr = "10000";
            int calcNumber = 0;

            if (!int.TryParse(taskNumberStr, out taskNumber) || taskNumber < 1 || !int.TryParse(calcNumberStr, out calcNumber) || calcNumber < 1)
            {
                error = "You must enter an integer value greater than 0 for # of tasks and number of calculations";
                return null;
            }

            List<BaseIrrational> selectedClasses = entries.Select(x =>
            {
                BaseIrrational instance = instances[Convert.ToInt32(x)];
                instance.TaskIterations = taskNumber;
                instance.InternalIterations = calcNumber;
                return instance;
            }).ToList();

            error = null;
            return selectedClasses;
        }

        internal static void Pause()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{" ".Repeat(20)}Press any key to return to the menu");
            Console.ReadKey();
            ConsoleRenderer.RenderMenu();
        }
    }
}
