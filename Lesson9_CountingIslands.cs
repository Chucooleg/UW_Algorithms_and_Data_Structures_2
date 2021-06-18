using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex2
{
	class CountingIslands
	{
		private static bool generateRandomGrid = false;
		private static bool debugPrint = true;
		private const string water = "W";
		private const string land  = "L";

		public static void Run()
		{
			Console.WriteLine("---------------------");
			Console.WriteLine("Doing BFS");
			Console.WriteLine("---------------------");
			
			bool doBFS = true;
			Test1(6, 5, doBFS);

			Console.WriteLine("");
			Console.WriteLine("---------------------");
			Console.WriteLine("Doing DFS");
			Console.WriteLine("---------------------");

			doBFS = false;
			Test1(6, 5, doBFS);
		}

		public static void Test1(int numRows, int numCols, bool doBFS)
		{
			// we will use a 2D array to represent the grid ( or graph )

			string[,] grid = InitializeGrid(numRows, numCols);
			Utils.DisplayGrid(grid, 1);

			int numIslands = CountNumberOfIslands(grid, doBFS);
			Console.WriteLine("\nNum islands is: " + numIslands);
			Console.WriteLine("");

			Utils.DisplayGrid(grid, 1);
		}

		private static int CountNumberOfIslands(string[,] grid, bool doBFS)
		{
			int numIslands = 0;

			for (int row = 0; row < grid.GetLength(0); ++row)
			{
				for (int col = 0; col < grid.GetLength(1); ++col)
				{
					if (grid[row, col] != land)
						continue;

					++numIslands;
					TraverseOneIsland(grid, row, col, doBFS, numIslands);
				}
			}

			return numIslands;
		}

		private static void TraverseOneIsland(string[,] grid, int startRow, int startCol, bool doBFS, int numIslands)
		{
			if (doBFS)
				BFS(grid, startRow, startCol, numIslands);
			else
				DFS(grid, startRow, startCol, numIslands);
		}

		private static void BFS(string[,] grid, int startRow, int startCol, int numIslands)
		{
			// we want a queue for holding the nodes (aka cells of the grid)
			Queue<GraphUtils.Vertex> q = new Queue<GraphUtils.Vertex>();
			
			// add starting node to queue
			grid[startRow, startCol] = numIslands.ToString();
			q.Enqueue( new GraphUtils.Vertex(startRow, startCol));

			while (q.Count() > 0)
			{
				if (debugPrint)
				{
					Console.WriteLine("---------------------");
					Utils.DisplayGrid(grid, 1);
				}

				GraphUtils.Vertex node = q.Dequeue();
				AddUnvisitedNeighborsToQueue(grid, node, q, numIslands);
			}
		}

		private static void AddUnvisitedNeighborsToQueue(string[,] grid, GraphUtils.Vertex node, Queue<GraphUtils.Vertex> q, int numIslands)
		{
			// get all neighbors of the cell (node.xCoord, node.yCoord)
			for (int row = node.xCoord - 1; row <= node.xCoord + 1; ++ row)
			{
				if ((row < 0) || (row >= grid.GetLength(0)))					// nothing to do if row out of bounds
					continue;

				for (int col = node.yCoord - 1; col <= node.yCoord + 1; ++ col)
				{
					if ((col < 0) || (col >= grid.GetLength(1)))				// nothing to do if col out of bounds
						continue;

					if ((row != node.xCoord) && (col != node.yCoord))			// no diagonal movement allowed. If u want to allow that, make a change in this line.
						continue;

					if (grid[row, col] == land)
					{
						grid[row, col] = numIslands.ToString();					// we will show the island number so that when we display grid, it tells us we identified an island.
						q.Enqueue(new GraphUtils.Vertex(row, col));
					}
				}
			}
		}

		private static void DFS(string[,] grid, int startRow, int startCol, int numIslands)
		{
			// if u were to keep a separate visited 2D array, then u would do something like this (after having first initialized visited to 0s).
			// Example, if u r not allowed to modify the grid.
			// visited[ startRow, startCol ] = 1;		


			// we will show the island number so that when we display grid, it tells us we identified an island.
			grid[startRow, startCol] = numIslands.ToString();

			if (debugPrint)
			{
				Console.WriteLine("---------------------");
				Utils.DisplayGrid(grid, 1);
			}

			for (int row = startRow - 1; row <= startRow + 1; ++row)
			{
				if ((row < 0) || (row >= grid.GetLength(0)))            // nothing to do if row out of bounds
					continue;

				for (int col = startCol - 1; col <= startCol + 1; ++col)
				{
					if ((col < 0) || (col >= grid.GetLength(1)))        // nothing to do if col out of bounds
						continue;
					if ((row != startRow) && (col != startCol))			// no diagonal movement allowed. If u want to allow that, make a change in this line.
						continue;

					if (grid[row, col] == land)
						DFS(grid, row, col, numIslands);
				}
			}
		}

		private static string[,] InitializeGrid(int numRows, int numCols)
		{
			if (generateRandomGrid)
				return InitializeRandomGrid(numRows, numCols);

			return InitializeGrid1(numRows, numCols);

			// call below will create one big island
			//return InitializeGrid2(numRows, numCols);
		}


		// InitializeGrid1 returns the grid u saw in our content material
		private static string[,] InitializeGrid1(int numRows, int numCols)
		{
			string[,] grid = new string[numRows, numCols];

			// NOTE: Caution:  The index values below are hardcoded since this is a debug function to initialize the 6 X 5 grid
			grid[0, 0] = water;
			grid[0, 1] = water;
			grid[0, 2] = land;
			grid[0, 3] = land;
			grid[0, 4] = water;

			grid[1, 0] = land;
			grid[1, 1] = water;
			grid[1, 2] = water;
			grid[1, 3] = land;
			grid[1, 4] = water;

			grid[2, 0] = water;
			grid[2, 1] = water;
			grid[2, 2] = land;
			grid[2, 3] = land;
			grid[2, 4] = water;

			grid[3, 0] = water;
			grid[3, 1] = land;
			grid[3, 2] = water;
			grid[3, 3] = water;
			grid[3, 4] = land;

			grid[4, 0] = water;
			grid[4, 1] = water;
			grid[4, 2] = water;
			grid[4, 3] = water;
			grid[4, 4] = water;

			grid[5, 0] = land;
			grid[5, 1] = land;
			grid[5, 2] = land;
			grid[5, 3] = water;
			grid[5, 4] = land;

			return grid;
		}

		// InitializeGrid2 returns a grid with a huge island in the center
		private static string[,] InitializeGrid2(int numRows, int numCols)
		{
			string[,] grid = new string[numRows, numCols];

			// NOTE: Caution:  The index values below are hardcoded since this is a debug function to initialize the 6 X 5 grid

			// only first column of this grid is water. So, grid[ *, 0 ] is water
			grid[0, 0] = water;
			grid[0, 1] = land;
			grid[0, 2] = land;
			grid[0, 3] = land;
			grid[0, 4] = land;

			grid[1, 0] = water;
			grid[1, 1] = land;
			grid[1, 2] = land;
			grid[1, 3] = land;
			grid[1, 4] = land;

			grid[2, 0] = water;
			grid[2, 1] = land;
			grid[2, 2] = land;
			grid[2, 3] = land;
			grid[2, 4] = land;

			grid[3, 0] = water;
			grid[3, 1] = land;
			grid[3, 2] = land;
			grid[3, 3] = land;
			grid[3, 4] = land;

			grid[4, 0] = water;
			grid[4, 1] = land;
			grid[4, 2] = land;
			grid[4, 3] = land;
			grid[4, 4] = land;

			grid[5, 0] = water;
			grid[5, 1] = land;
			grid[5, 2] = land;
			grid[5, 3] = land;
			grid[5, 4] = land;

			return grid;
		}
		private static string[,] InitializeRandomGrid(int numRows, int numCols)
		{
			Random rnd = new Random();
			string[,] grid = new string[numRows, numCols];

			for (int row = 0; row < grid.GetLength(0); ++row)
				for (int col = 0; col < grid.GetLength(1); ++col)
					grid[row, col] = rnd.Next(0, 2) == 0 ? water : land;

			return grid;
		}

	}
}
