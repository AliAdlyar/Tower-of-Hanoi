using System;
using System.Runtime.InteropServices;

namespace Console_UI
{
	public static class Program
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern int FreeConsole();

		private static int counter = 1;

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

		static void ConsoleMode()
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
					if (totalDisks < 1)
						throw new Exception();
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
						break;

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

                }
            }
		}


		static void UIMode()
		{
			new App().Run();
		}

		[STAThread]
		static void Main()
		{
			Console.SetWindowSize(120, 30);
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.Write("\n  Please choose mode:\n\n");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("  1: Console     ");
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.Write("2: With UI\n  ");
			Console.CursorVisible = false;

			while (true)
			{
				var key = Console.ReadKey();
				if (key.Key == ConsoleKey.D1)
				{
					Console.Clear();
					ConsoleMode();
					break;
				}
				else if (key.Key == ConsoleKey.D2)
				{
					FreeConsole();
					UIMode();
					break;
				}
				else
				{
					Console.SetCursorPosition(2, Console.CursorTop);
					Console.Write(' ');
					Console.SetCursorPosition(2, Console.CursorTop);
				}
			}
		}
	}
}