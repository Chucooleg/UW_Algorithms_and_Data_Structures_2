using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// TBD Encapsulate cell access inside a function CellXY()?
// Move test methods to a separate class (GraphTest or something)
// chage name "distanceFromSource" to something appropriate since for A* algo I add dist to dest
namespace Ex2
{
	internal class Graph
	{
		private bool debugPrints = false;
		private bool printPathVertices = false;
		static bool generateRandomWeights = false;

		private int maxXCoord;
		private int maxYCoord;
		private int[,] grid;
		private bool[,] visited;

		private bool allowDiagonalMovement = false;
		private bool addEstimatedDistanceToDestination = false;		// AStar algorithm uses this, so we default to false

		private const int minWeight = 5;
		private const int maxWeight = 25;
		private const int WALL_WEIGHT = 5000000;     // some high weight
		private const string WALL_WEIGHT_PrintStr = "WW";
		
		public static void Run()
		{
			DoDijkstraRuns(addEstimatedDistanceToDestination:false);
			DoAStarRuns();
			BFSTest(usePriorityQueueWithEqualWeights:false);
			//BFSTest(usePriorityQueueWithEqualWeights:true);
		}

		public static void BFSTest(bool usePriorityQueueWithEqualWeights )
		{
			Console.WriteLine("BFSTest using " + (usePriorityQueueWithEqualWeights ? "priority queue with equal weights" : "using regular queue, not a priority queue"));

			int maxXCoord = 8;
			int maxYCoord = 8;
			Graph graph = new Graph(maxXCoord, maxYCoord, equalWeights: true);       // BFS, so we want edge weights to be the same (or not have weights at all)

			GraphUtils.Vertex source = new GraphUtils.Vertex(0, 0);
			GraphUtils.Vertex destination = new GraphUtils.Vertex(graph.maxXCoord - 1, graph.maxYCoord - 1);
			Dictionary<GraphUtils.Vertex, GraphUtils.Vertex> parentVertices = new Dictionary<GraphUtils.Vertex, GraphUtils.Vertex>();

			if (usePriorityQueueWithEqualWeights)
			{
				graph.Display();
				graph.BFSPathPriorityQueueEqualWeights(source, destination, parentVertices);
			}
			else
			{
				graph.BFSPathRegularQueue(source, destination, parentVertices);
			}

			graph.DisplayPath(source, destination, parentVertices);

			// TBD: Check if calling BFSPathPQueueEqualWeights (uses heap with equal edge weights) gives same results as calling BFSPath (uses regular queue)

			Console.WriteLine("");
		}

		public static void DoAStarRuns()
		{
			DoDijkstraRuns(addEstimatedDistanceToDestination: true);
		}

		public static void DoDijkstraRuns(bool addEstimatedDistanceToDestination)
		{
			bool addWall = true;
			GraphUtils.Wall rectwall = null;
			if (addWall)
				rectwall = GraphUtils.GenerateWall_Rectangle(new GraphUtils.Vertex(3,1), 4, 6);
			
			DijkstraTest1(addEstimatedDistanceToDestination, null, null, rectwall);

			/*
			// test with no walls
			DijkstraTest1(addEstimatedDistanceToDestination);

			// now, add some walls
			GraphUtils.Wall hwall = GraphUtils.GenerateWall_Horizontal();
			DijkstraTest1(addEstimatedDistanceToDestination, null, null, hwall);

			GraphUtils.Wall vwall = GraphUtils.GenerateWall_Vertical();
			hwall.theWall.AddRange(vwall.theWall);
			DijkstraTest1(addEstimatedDistanceToDestination, null, null, hwall);

			GraphUtils.Wall dwall = GraphUtils.GenerateWall_Diagonal();
			DijkstraTest1(addEstimatedDistanceToDestination, null, null, dwall);
			*/
		}

		public static void DijkstraTest1(bool addEstimatedDistanceToDestination = false, GraphUtils.Vertex start = null, GraphUtils.Vertex end = null, GraphUtils.Wall walls = null)
		{
			int maxXCoord = 8;
			int maxYCoord = 8;

			// Test with no wall
			Console.WriteLine(addEstimatedDistanceToDestination ? "AStar Test" : "Dijkstra Test");

			Graph graph = new Graph(maxXCoord, maxYCoord, equalWeights:false, addEstimatedDistanceToDestination, walls);       // Dijkstra, so we want edge weights to be different
			graph.Display();

			GraphUtils.Vertex source = start == null ? new GraphUtils.Vertex(0, 0) : start;
			GraphUtils.Vertex destination = end == null ? new GraphUtils.Vertex(graph.maxXCoord - 1, graph.maxYCoord - 1) : end;
			Dictionary<GraphUtils.Vertex, GraphUtils.Vertex> parentVertices = new Dictionary<GraphUtils.Vertex, GraphUtils.Vertex>();

			graph.DijkstraPath(source, destination, parentVertices);
			graph.DisplayPath(source, destination, parentVertices);

			Console.WriteLine("");
		}

		private Dictionary<GraphUtils.Vertex, int> InitializeDistanceFromSource(GraphUtils.Vertex source, int sourceWeight)
		{
			Dictionary<GraphUtils.Vertex, int> dist = new System.Collections.Generic.Dictionary<GraphUtils.Vertex, int>();
			dist.Add(source, sourceWeight);
			
			return dist;
		}

		private void DijkstraPath(GraphUtils.Vertex source, GraphUtils.Vertex destination, Dictionary<GraphUtils.Vertex, GraphUtils.Vertex> parentVertices)
		{
			BinaryHeap_Min q = new BinaryHeap_Min();
			Dictionary<GraphUtils.Vertex, int> distanceFromSource = InitializeDistanceFromSource(source, 0);
			q.Add(0,source);

			while (q.NumElements() > 0)
			{
				GraphUtils.Vertex vertex = (GraphUtils.Vertex)q.GetMin().obj;
				q.DeleteMin();

				visited[vertex.yCoord, vertex.xCoord] = true;
				if (vertex.Equals( destination))
				{
					//Console.Write("Reached destination: ");
					DisplayVertex(destination);
					break;
				}
				AddUnvisitedNeighbors(q, vertex, visited, parentVertices, distanceFromSource, destination);
			}
		}

		private void BFSPathPriorityQueueEqualWeights(GraphUtils.Vertex source, GraphUtils.Vertex destination, Dictionary<GraphUtils.Vertex, GraphUtils.Vertex> parentVertices)
		{
			BinaryHeap_Min q = new BinaryHeap_Min();
			Dictionary<GraphUtils.Vertex, int> distanceFromSource = InitializeDistanceFromSource(source, 0);
			q.Add(0, source);

			while (q.NumElements() > 0)
			{
				GraphUtils.Vertex vertex = (GraphUtils.Vertex)q.GetMin().obj;
				q.DeleteMin();

				visited[vertex.yCoord, vertex.xCoord] = true;
				if (vertex.Equals(destination))
				{
					//Console.Write("Reached destination: ");
					DisplayVertex(destination);
					break;
				}
				AddUnvisitedNeighbors(q, vertex, visited, parentVertices, distanceFromSource, destination);
			}
		}

		private List<GraphUtils.Vertex> GetNeighbors1(GraphUtils.Vertex vertex, bool getDiagonalNeighbors)
		{
			List<GraphUtils.Vertex> neighbors = new List<GraphUtils.Vertex>();

			for (int startX = vertex.xCoord - 1; startX <= vertex.xCoord + 1; ++startX)
			{
				for (int startY = vertex.yCoord - 1; startY <= vertex.yCoord + 1; ++startY)
				{
					if ((startX != vertex.xCoord) && (startY != vertex.yCoord) && !getDiagonalNeighbors)		// if no diagonal movement allowed, continue
						continue;

					if (!IsValidCoord(startX, startY))
						continue;

					neighbors.Add(new GraphUtils.Vertex(startX, startY));
				}
			}

			return neighbors;
		}

		// runs the for loop starting from Item1 + 1 down to Item1 - 1 (unlike GetNeighbors1)
		private List<GraphUtils.Vertex> GetNeighbors2(GraphUtils.Vertex vertex, bool getDiagonalNeighbors)
		{
			List<GraphUtils.Vertex> neighbors = new List<GraphUtils.Vertex>();

			for (int startX = vertex.xCoord + 1; startX >= vertex.xCoord - 1; --startX)
			{
				for (int startY = vertex.yCoord + 1; startY >= vertex.yCoord - 1; --startY)
				{
					if ((startX != vertex.xCoord) && (startY != vertex.yCoord) && !getDiagonalNeighbors)      // if no diagonal movement allowed, continue
						continue;

					if (!IsValidCoord(startX, startY))
						continue;

					neighbors.Add(new GraphUtils.Vertex(startX, startY));
				}
			}

			return neighbors;
		}


		private void BFSPathRegularQueue(GraphUtils.Vertex source, GraphUtils.Vertex destination, Dictionary<GraphUtils.Vertex, GraphUtils.Vertex> parentVertices)
		{
			Queue<GraphUtils.Vertex> q = new Queue<GraphUtils.Vertex>();
			q.Enqueue(source);
			
			while (q.Count() > 0)
			{
				GraphUtils.Vertex vertex = q.Dequeue();
				visited[vertex.yCoord, vertex.xCoord] = true;
				if (vertex.Equals(destination))
				{
					//Console.Write("Reached destination: ");
					DisplayVertex(destination);
					break;
				}
				AddUnvisitedNeighbors_UsesRegularQueue(q, vertex, visited, parentVertices);
			}
		}

		private void AddUnvisitedNeighbors(BinaryHeap_Min q, GraphUtils.Vertex vertex, bool[,] visited, Dictionary<GraphUtils.Vertex, GraphUtils.Vertex> parentVertices,
			Dictionary<GraphUtils.Vertex, int> distanceFromSource, GraphUtils.Vertex destination)
		{
			if (debugPrints)
			{
				Console.Write(" Unvisited neighbors of ");
				DisplayVertex(vertex);
			}

			List<GraphUtils.Vertex> neighbors = GetNeighbors1(vertex, allowDiagonalMovement);

			foreach (GraphUtils.Vertex neighbor in neighbors)
			{
				int newDistanceFromSource = distanceFromSource[vertex] + grid[neighbor.yCoord, neighbor.xCoord];

				if ( ! distanceFromSource.ContainsKey(neighbor) || (newDistanceFromSource < distanceFromSource[neighbor]) )
				{
					int priorityWeight = newDistanceFromSource;
					if (addEstimatedDistanceToDestination)
					{
						priorityWeight += EstimateDistanceToDestination(neighbor, destination);
					}
					q.Add(priorityWeight, neighbor);
					distanceFromSource[neighbor] = newDistanceFromSource;
					parentVertices[neighbor] = vertex;
					if (debugPrints)
						DisplayVertex(neighbor);
				}
			}
		}

		private void AddUnvisitedNeighbors_UsesRegularQueue(Queue<GraphUtils.Vertex> q, GraphUtils.Vertex vertex, bool[,] visited, Dictionary<GraphUtils.Vertex, GraphUtils.Vertex> parentVertices)
		{
			if (debugPrints)
			{
				Console.Write(" Unvisited neighbors of ");
				DisplayVertex(vertex);
			}

			List<GraphUtils.Vertex> neighbors = GetNeighbors2(vertex, allowDiagonalMovement);
	
			foreach (GraphUtils.Vertex neighbor in neighbors)
			{
				if (!visited[neighbor.yCoord, neighbor.xCoord])		// we use unvisited check because here we use a regular queue (and not a priority queue with weights)
				{
					q.Enqueue(neighbor);
					parentVertices[neighbor] = vertex;
					if (debugPrints)
						DisplayVertex(neighbor);
				}
			}
		}

		private int EstimateDistanceToDestination(GraphUtils.Vertex current, GraphUtils.Vertex destination)
		{
			// determine the left vertex (lower xCoord)
			GraphUtils.Vertex leftVertex = current.xCoord < destination.xCoord ? current : destination;
			GraphUtils.Vertex rightVertex = current.xCoord > destination.xCoord ? current : destination;
			leftVertex  = new GraphUtils.Vertex(leftVertex.xCoord, leftVertex.yCoord);
			rightVertex = new GraphUtils.Vertex(rightVertex.xCoord, rightVertex.yCoord);

			// Figure out the direction of y movement (up or down)
			int yIncrement = leftVertex.yCoord < rightVertex.yCoord ? 1 : -1;
			int xIncrement = 1;
			int estimatedDistance = 0;

			while ((leftVertex.xCoord != rightVertex.xCoord) && (leftVertex.yCoord != rightVertex.yCoord))
			{
				estimatedDistance += grid[leftVertex.yCoord, leftVertex.xCoord];

				// if we reached rightVertex in the horizontal direction, then stop incrementing X
				if (leftVertex.xCoord == rightVertex.xCoord)
					xIncrement = 0;

				// if we reached rightVertex in the vertical direction, then stop incrementing Y
				if (leftVertex.yCoord == rightVertex.yCoord)
					yIncrement = 0;

				leftVertex.xCoord += xIncrement;
				leftVertex.yCoord += yIncrement;
			}

			return estimatedDistance;
		}

		private void DisplayVertex(GraphUtils.Vertex neighbor)
		{
			if (printPathVertices)
			{
				Console.Write("(x, y): " + neighbor.yCoord + ", " + neighbor.xCoord + "    ");
			}
		}

		
		public Graph(int MaxXCoord, int MaxYCoord,  bool equalWeights, bool addEstimatedDistanceToDestination = false, GraphUtils.Wall walls = null)
		{
			maxXCoord = MaxXCoord;
			maxYCoord = MaxYCoord;
			this.addEstimatedDistanceToDestination = addEstimatedDistanceToDestination;
			grid = new int[MaxYCoord, MaxXCoord];
			visited = new bool[grid.GetLength(0), grid.GetLength(1)];       // is initialized to false.

			// pass it min and max weights. If we want equal weights for all edges, then pass max weight the same as min weight
			InitializeWeights(minWeight, equalWeights ? minWeight:maxWeight, generateRandomWeights);
			
			AddWalls(walls);
		}

		public void AddHorizontalWall(GraphUtils.Vertex start, GraphUtils.Vertex end)
		{
			if (!IsHorizontal(start, end))
				throw (new ArgumentException("Called AddHorizontalWall with bad values."));

			int startX = Math.Min(start.xCoord, end.xCoord);
			int endX = Math.Max(start.xCoord, end.xCoord);
			int yCoord = start.yCoord;		// same as end.yCoord

			for (; startX <= endX; ++startX)
			{
				if (!IsValidCoord(startX, yCoord))			// if we are out of bounds of the grid, we r done
					break;

				grid[yCoord, startX] = WALL_WEIGHT;
			}
		}

		public void AddVerticalWall(GraphUtils.Vertex start, GraphUtils.Vertex end)
		{
			if (!IsVertical(start, end))
				throw (new ArgumentException("Called AddVerticalWall with bad values."));

			int startY = Math.Min(start.yCoord, end.yCoord);
			int endY = Math.Max(start.yCoord, end.yCoord);
			int xCoord = start.xCoord;      // same as end.xCoord

			for (; startY <= endY; ++startY)
			{
				if (!IsValidCoord(xCoord, startY))          // if we are out of bounds of the grid, we r done
					break;

				grid[startY, xCoord] = WALL_WEIGHT;
			}
		}

		public void AddDiagonalWall(GraphUtils.Vertex start, GraphUtils.Vertex end)
		{
			if (IsVertical(start, end) || IsHorizontal(start, end))
				throw (new ArgumentException("Called AddDiagonalWall with bad values."));

			// determine the left vertex (lower xCoord)
			GraphUtils.Vertex leftVertex  = start.xCoord < end.xCoord ? start : end;
			GraphUtils.Vertex rightVertex = start.xCoord > end.xCoord ? start : end;

			// we will add wall weights from leftVertex to rightVertex, but first figure out the direction of y movement (up or down)
			int yIncrement = leftVertex.yCoord < rightVertex.yCoord ? 1 : -1;
			int xIncrement = 1;

			while ( (leftVertex.xCoord != rightVertex.xCoord) && (leftVertex.yCoord != rightVertex.yCoord))
			{
				grid[leftVertex.yCoord, leftVertex.xCoord] = WALL_WEIGHT;

				// if we reached rightVertex in the horizontal direction, then stop incrementing X
				if (leftVertex.xCoord == rightVertex.xCoord)
					xIncrement = 0;

				// if we reached rightVertex in the vertical direction, then stop incrementing Y
				if (leftVertex.yCoord == rightVertex.yCoord)
					yIncrement = 0;

				leftVertex.xCoord += xIncrement;
				leftVertex.yCoord += yIncrement;
			}
		}

		private void AddWalls(GraphUtils.Wall walls)
		{
			if (walls == null)
				return;

			foreach (Tuple < GraphUtils.Vertex, GraphUtils.Vertex> t in walls.theWall)
			{
				AddOneWall(t.Item1, t.Item2);
			}
		}

		private void AddOneWall(GraphUtils.Vertex start, GraphUtils.Vertex end)
		{
			if (IsHorizontal(start, end))
				AddHorizontalWall(start, end);
			else if (IsVertical(start, end))
				AddVerticalWall(start, end);
			else
				AddDiagonalWall(start, end);
		}


		private bool IsHorizontal(GraphUtils.Vertex point1, GraphUtils.Vertex point2)
		{
			return point1.yCoord == point2.yCoord;
		}

		private bool IsVertical(GraphUtils.Vertex point1, GraphUtils.Vertex point2)
		{
			return point1.xCoord == point2.xCoord;
		}

		private bool IsValidCoord(int xCoord, int yCoord)
		{
			return xCoord >= 0
				&& yCoord >= 0
				&& xCoord <= grid.GetUpperBound(1)
				&& yCoord <= grid.GetUpperBound(0);
		}
		private void InitializeWeights(int minWeight, int maxWeight, bool generateRandomWeights)
		{
			if (generateRandomWeights)
				InitializeRandomWeights(minWeight, maxWeight);
			else
			{
				for (int xCoord = 0; xCoord < maxXCoord; ++xCoord)
				{
					for (int yCoord = 0; yCoord < maxYCoord; ++yCoord)
					{
						if (minWeight == maxWeight)
							grid[yCoord, xCoord] = 1;
						else
							grid[yCoord, xCoord] = (xCoord + yCoord) % (maxWeight - minWeight) + minWeight;
					}
				}
			}
		}

		private void InitializeRandomWeights(int minWeight, int maxWeight)
		{
			Random rnd = new Random();

			for (int xCoord = 0; xCoord < maxXCoord; ++xCoord)
			{
				for (int yCoord = 0; yCoord < maxYCoord; ++yCoord)
				{
					grid[yCoord, xCoord] = rnd.Next(minWeight, maxWeight + 1);
				}
			}
		}

		public void DisplayPath(GraphUtils.Vertex source, GraphUtils.Vertex destination, Dictionary<GraphUtils.Vertex, GraphUtils.Vertex> parentVertices)
		{
			// create and initialize a pathgrid to show the path from source to destination
			string[,] pathGrid = new string[grid.GetLength(0), grid.GetLength(1)];

			/*
			for (int xCoord = 0; xCoord < maxXCoord; ++xCoord)
			{
				for (int yCoord = 0; yCoord < maxYCoord; ++yCoord)
				{
					pathGrid[yCoord, xCoord] = "-";
				}
			}*/

			// Now mark visited vertices as V
			for (int xCoord = 0; xCoord < maxXCoord; ++xCoord)
			{
				for (int yCoord = 0; yCoord < maxYCoord; ++yCoord)
				{
					pathGrid[yCoord, xCoord] = visited[yCoord, xCoord] ? ".." : " ";
				}
			}

			Console.WriteLine("Path is:");

			int totalPathCost = 0;
			GraphUtils.Vertex vertex = destination;
			DisplayVertex(destination);
			pathGrid[destination.yCoord, destination.xCoord] = "DD";
			totalPathCost += grid[destination.yCoord, destination.xCoord];

			while (vertex != source)
			{
				vertex = parentVertices[vertex];
				DisplayVertex(vertex);
				pathGrid[vertex.yCoord, vertex.xCoord] = "P";
				
				totalPathCost += grid[vertex.yCoord, vertex.xCoord];
			}
			pathGrid[source.yCoord, source.xCoord] = "SS";
			Console.WriteLine("Total path cost " + totalPathCost);
			
			// also show it as a grid
			int padLength = 10;
			Utils.DisplayGrid(pathGrid, padLength);
			Console.WriteLine("////////////////////////////////////////////////////////////////////////////////////////////");
			Console.WriteLine();
		}

		public void Display()
		{
			int padLength = 10;
			Utils.DisplayGrid(grid, padLength, WALL_WEIGHT, WALL_WEIGHT_PrintStr);
			Console.WriteLine();
			Console.WriteLine();
		}
	}
}
