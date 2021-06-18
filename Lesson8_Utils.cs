using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex2
{
	class Utils
	{
		static public void Output(List<String> strings, string initialMsg="", string endMsg="", string separator=" ")
		{
			if (initialMsg.Length > 0)
				Console.WriteLine(initialMsg);

			foreach (string s in strings)
			{
				Console.Write(s);
				Console.Write(separator);
			}

			if (endMsg.Length > 0)
				Console.WriteLine(endMsg);
		}

		static public List<String> GenerateStrings1()
		{
			List<String> strs = new List<string>();

			strs.Add("washington");
			strs.Add("washing");				 
			strs.Add("washingmachine");
			strs.Add("university");
			strs.Add("washer");
			strs.Add("web");
			strs.Add("sanitation");
			strs.Add("sanctuary");
			strs.Add("water");

			return strs;
		}

		static public List<string> GenerateStrings2()
		{
			List<string> strs = new List<string>();
			//strList.Add("ant");
			//strList.Add("andy");
			strs.Add("shells");
			strs.Add("by");
			strs.Add("are");

			return strs;
		}

		// swap two integers
		static public void Swap(ref int elementA, ref int elementB)
		{
			int elementACopy = elementA;
			elementA = elementB;
			elementB = elementACopy;
		}

		static public void InitArray(int[] arr, int value)
		{
			// foreach  (ref int x in arr)  need C#8.0
			for (int ii = 0; ii < arr.Length; ++ii)
				arr[ii] = value;
		}

		static public void InitArray(int[,] arr, int value)
		{
			for (int row = 0; row < arr.GetLength(0); ++row)
				for (int col = 0; col < arr.GetLength(1); ++col)
					arr[row, col] = value;
		}

		static public void DisplayGrid(int [,] grid, int padLength)
		{
			for (int row = 0; row < grid.GetLength(0); ++row)
			{
				for (int col = 0; col < grid.GetLength(1); ++col)
				{
					System.Console.Write(grid[row, col].ToString().PadLeft(padLength, ' ') + "  ");
				}
				System.Console.WriteLine("");
			}
		}

		// if value == sentinelValue, then print sentinelPrintReplacement, else print the actual value in grid
		static public void DisplayGrid(int[,] grid, int padLength, int sentinelValue, string sentinelPrintReplacement)
		{
			for (int row = 0; row < grid.GetLength(0); ++row)
			{
				for (int col = 0; col < grid.GetLength(1); ++col)
				{
					string printValue = grid[row, col] == sentinelValue ? sentinelPrintReplacement : grid[row, col].ToString();
					System.Console.Write(printValue.PadLeft(padLength, ' ') + "  ");
				}
				System.Console.WriteLine("");
			}
		}

		static public void DisplayGrid(string[,] grid, int padLength)
		{
			for (int row = 0; row < grid.GetLength(0); ++row)
			{
				for (int col = 0; col < grid.GetLength(1); ++col)
				{
					System.Console.Write(grid[row, col].PadLeft(padLength, ' ') + "  ");
				}
				System.Console.WriteLine("");
			}
		}

		/// <summary>
		/// Assumes input is sorted.
		/// </summary>
		/// <param name="input">sorted array of integers</param>
		/// <param name="k">value being searched for</param>
		/// <returns>index of element found</returns>
		static public int BinarySearch(int[] input, int k, out bool found)
		{
			int start = 0;
			int end = input.Length - 1;
			int mid = -1;
			found = false;

			while (start <= end)
			{
				mid = start + (end - start) / 2;

				if (k == input[mid])
				{
					found = true;
					break;
				}
				else if (k > input[mid])
					start = mid + 1;
				else
					end = mid - 1;
			}

			return mid;
		}

		static public void TestBinarySearch()
		{
			int[] input = new int[] { 0, 5, 10, 15, 20, 25, 30, 35 };

			for (int ii = 1; ii < 15; ii += 5)
			{
				//bool found;
				int foundIndex = BinarySearch(input, ii, out bool found);

				Console.WriteLine(" number: " + ii + " found: " + found +  " Foundindex: " + foundIndex);
			}
		}
	}
}
