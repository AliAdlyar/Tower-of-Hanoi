using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_of_Hanoi
{
	internal class Program
	{
		public static int counter = 1;

		static void SolveTowers(int n, char startPeg, char tempPeg, char endPeg)
		{
			if (n > 0)
			{
				SolveTowers(n - 1, startPeg, endPeg, tempPeg);

				Console.Write($"  {counter})");

				if (counter < 10)
					Console.Write("   ");
				else if (counter < 100)
					Console.Write("  ");
				else
					Console.Write(" ");

				Console.Write($"{startPeg} --> {endPeg}\n");

				counter++;

				SolveTowers(n - 1, tempPeg, startPeg, endPeg);
			}
		}

		static void Main()
		{
			char startPeg = 'A'; // start tower in output
			char tempPeg = 'B'; // temporary tower in output
			char endPeg = 'C'; // end tower in output
			int totalDisks; // number of disks
			bool cont = false;

			while (true)
			{
				counter = 1;
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write("\n  please enter the number of disks: ");
				Console.CursorVisible = false;

				try
				{
					totalDisks = Convert.ToInt32(Console.ReadLine());
				}
				catch (Exception) { continue; }

				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write($"\n  Solving Towers of Hanoi with {totalDisks} Disks, step by step:\n\n");

				Console.ForegroundColor = ConsoleColor.Cyan;
				SolveTowers(totalDisks, startPeg, tempPeg, endPeg);

				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("\n ---------------------\n  r: retry, esc: exit\n");
				Console.ResetColor();

				while (true)
				{
					var key = Console.ReadKey();
					if (key.Key == ConsoleKey.R)
						cont = true;
					else if (key.Key == ConsoleKey.Escape)
					{
						Console.Write(' ');
						return;
					}
					else
					{
						Console.SetCursorPosition(0, Console.CursorTop);
						Console.Write(' ');
						Console.SetCursorPosition(0, Console.CursorTop);
					}
					if (cont)
					{
						cont = false;
						break;
					}
				}
			}
		}
	}
}
