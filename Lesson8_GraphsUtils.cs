using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex2
{
	internal class GraphUtils
	{
		internal class Vertex
		{
			public int xCoord;
			public int yCoord;

			public Vertex(int x, int y)
			{
				xCoord = x;
				yCoord = y;
			}

			public override bool Equals(Object obj)
			{
				Vertex other = obj as Vertex;
				return (xCoord == other.xCoord) && (yCoord == other.yCoord);
			}

			public override int GetHashCode()
			{
				return xCoord.GetHashCode() + yCoord.GetHashCode();
			}
		}

		internal class Wall
		{
			public List<Tuple<Vertex, Vertex>> theWall;

			public Wall()
			{
				theWall = new List<Tuple<Vertex, Vertex>>();
			}
		}

		static public Wall GenerateWall_LShape(Vertex start = null, Vertex end = null)
		{
			Wall wall = new Wall();

			Vertex startWall1 = start == null ? new Vertex(2, 2) : start;           // TBD: remove use of hard coded numbers, need to know grid bounds, so pass that in.
			Vertex endWall1 = end == null ? new Vertex(5, 2) : end;
			wall.theWall.Add(new Tuple<Vertex, Vertex>(startWall1, endWall1));

			Vertex startWall2 = endWall1;
			Vertex endWall2 = new Vertex(5, 5);
			wall.theWall.Add(new Tuple<Vertex, Vertex>(startWall2, endWall2));

			return wall;
		}
		static public Wall GenerateWall_Horizontal(Vertex start = null, Vertex end = null)
		{
			Wall wall = new Wall();

			Vertex startWall1 = start == null ? new Vertex(2, 2) : start;           // TBD: remove use of hard coded numbers, need to know grid bounds, so pass that in.
			Vertex endWall1 = end == null ? new Vertex(5, 2) : end;
			wall.theWall.Add(new Tuple<Vertex, Vertex>(startWall1, endWall1));

			return wall;
		}

		static public Wall GenerateWall_Vertical(Vertex start = null, Vertex end = null)
		{
			Wall wall = new Wall();

			Vertex startWall1 = start == null ? new Vertex(2, 2) : start;           // TBD: remove use of hard coded numbers, need to know grid bounds, so pass that in.
			Vertex endWall1 = end == null ? new Vertex(2, 5) : end;
			wall.theWall.Add(new Tuple<Vertex, Vertex>(startWall1, endWall1));

			return wall;
		}

		static public Wall GenerateWall_Diagonal(Vertex start = null, Vertex end = null)
		{
			Wall wall = new Wall();

			Vertex startWall1 = start == null ? new Vertex(2, 2) : start;           // TBD: remove use of hard coded numbers, need to know grid bounds, so pass that in.
			Vertex endWall1 = end == null ? new Vertex(5, 5) : end;
			wall.theWall.Add(new Tuple<Vertex, Vertex>(startWall1, endWall1));

			return wall;
		}
		static public Wall GenerateWall_Rectangle(Vertex start = null, int width = 0, int length = 0)
		{
			const int DEFAULT_WIDTH = 4;
			const int DEFAULT_LENGTH = 4;

			Vertex startWall = start == null ? new Vertex(2, 2) : start;           // TBD: remove use of hard coded numbers, need to know grid bounds, so pass that in.
			int wallWidth = width == 0 ? DEFAULT_WIDTH : width;
			int wallLength = length == 0 ? DEFAULT_LENGTH : length;

			// Get the 4 corners of rectangle
			Vertex topLeft = startWall;
			Vertex topRight = new Vertex(startWall.xCoord + wallWidth - 1, startWall.yCoord);
			Vertex bottomLeft = new Vertex(startWall.xCoord, startWall.yCoord + wallLength - 1);
			Vertex bottomRight = new Vertex(startWall.xCoord + wallWidth - 1, startWall.yCoord + wallLength - 1);

			// Generate all 4 walls of rectangle
			Wall topWall = GenerateWall_Horizontal(topLeft, topRight);
			Wall leftWall = GenerateWall_Vertical(topLeft, bottomLeft);
			Wall bottomWall = GenerateWall_Horizontal(bottomLeft, bottomRight);
			Wall rightWall = GenerateWall_Vertical(topRight, bottomRight);

			// combine all walls into topWall and return that
			topWall.theWall.AddRange(leftWall.theWall);
			topWall.theWall.AddRange(bottomWall.theWall);
			topWall.theWall.AddRange(rightWall.theWall);

			return topWall;
		}
	}
}
