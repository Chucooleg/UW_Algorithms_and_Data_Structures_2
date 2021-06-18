using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex2
{
	class AlienDict
	{
		public static void Run()
		{
			string[] sortedWordsA1 = new string[] { "zb", "za" };       // zba OR baz OR bza
			Test(sortedWordsA1);
			
			string[] sortedWordsA2 = new string[] { "zb", "za", "bz" }; // zba
			Test(sortedWordsA2);

			// move "bz" to first position
			string[] sortedWordsA3 = new string[] { "bz", "zb", "za" };	// bza  OR  baz
			Test(sortedWordsA3);

			string[] sortedWordsA4 = new string[] { "bz", "zb", "za", "ab" }; // bza
			Test(sortedWordsA4);

			string[] sortedWords0 = new string[] { "m", "m" };
			Test(sortedWords0);

			string[] sortedWords1 = new string[] { "xhu", "xhe", "ch", "cuu", "heuu" };  //xchue
			Test(sortedWords1);

			string[] sortedWords2 = new string[] { "z", "x" };
			Test(sortedWords2);

			string[] sortedWords3 = new string[] { "z", "x", "z" };
			Test(sortedWords3);

			////////////////////////////////////////////////////////////////////////////////////////
			/////// Below example is based on the Enlish alphabet ordering, so u dont have to stand on ur head to understand  :-)))  ////////////////
			////////////////////////////////////////////////////////////////////////////////////////
			/// Lets look at this in steps and see how we can add more info to the given sorted list of words to finally get the complete order of letters.
			/// Here, our goal is to get "abdez".
			/// But u will see that we didnt get that initially, 
			/// but then I keep adding more words to the given sorted list of words to get to my goal (because more words help with making more inferences)
			/*
			Initial list of words (and the inferences we can draw from that)
			"abc"
			"aef"	// b --> e
			"bbc"	// a --> b
			"bdc"	// b --> d
			"bzz"
			// above gives order as "abedz". But we  really want "abdez". So we add the following word 
			"daz"	<-- add this and one below to disambiguate 'e' and 'd' order 
			"eff"	// d --> e
			// above gives order as "abdze". We need one more word:
			"zzz"	// e --> z
			// Now we get the order as "abdez"  << This is correct.
			*/

			string[] sortedWords4 = new string[] { "abc", "aef", "bbc", "bdc", "bzz" };
			Test(sortedWords4);

			// add "daz", "eff"
			string[] sortedWords5 = new string[] { "abc", "aef", "bbc", "bdc", "bzz", "daz", "eff" };
			Test(sortedWords5);

			// add "zzz"
			string[] sortedWords6 = new string[] { "abc", "aef", "bbc", "bdc", "bzz", "daz", "eff", "zzz" };
			Test(sortedWords6);
		}

		public static string Test(string[] sortedWords)
		{
			string order = GetAlienOrder(sortedWords);
			Console.WriteLine("order is " + order);
			Console.WriteLine();

			return order;
		}

		/// <summary>
		/// 1. use the sortedWords given to build an adjacency list for a directed graph where:
		//		- vertices represent the alphabet letters
		//		- edges represent the order of the letters as inferred from the sorted words given to us
		//  2. Start with the vertex that has an in-degree of 0
		//		- if complete info is given, then there should only be one (and not more, not less) such vertex.
		//			However, that may not be the case and u may have > 1 such vertex, and if so, just use any ordering of these characters.
		//			Eg: 'a' is the only char with indegree of 0 here, then its obvious that comes first
		//				But if u have 'a' and 'b' as the chars with indegree of 0, then "ab" or "ba" would both be valid choices (since u cannot do any better and determine the relative ordering between 'a' and 'b')
		//		- this is the first letter(s) of the alphabet (E.g.:  'a' or "ab" or "ba" in our example)
		//		- Now, go to the vertex (vertices) that has an incoming edge from this vertex.
		//			- This edge signifies the letter(s) that comes next.
		//			- If all info is given, there should be only one such letter (ie, out-degree of the first letter should be exactly 1...out-degree for any letter should be exactly 1, except the last one)
		//				Again, this may not be the case and u may have more than one
		//			- remove this edge (which means, we will reduce this vertex's in-degree count, which will now become 0)
		//		- Get the letter with an in-degree of 0, which should be the letter above (and that should be the only one with in-degree 0 at this point)
		//		- This is the 2nd letter.
		//      - Repeat the process until no more vertices are left.
		public static string GetAlienOrder(string[] sortedWords)
		{
			string order = String.Empty;
			Dictionary<char, List<char>> adjList = new Dictionary<char, List<char>>();
			Dictionary<char, int> inDegrees = new Dictionary<char, int>();

			BuildAdjacencyList(sortedWords, adjList, inDegrees);

			// Now, we need to know what vertices have an indegree of 0.
			// That will be the letters that have nothing before them (as far as we can infer from the information given).
			// If info given to u is complete, then u will see that in the beginning, only one letter will have indegree of 0  (eg. letter 'a' in English alphabet).
			string zeroIndegreeChars = FindZeroIndegreeChars(inDegrees);
			if (zeroIndegreeChars.Length == 0)
			{
				Console.WriteLine("No order exists");
				return String.Empty;
			}
			order += zeroIndegreeChars;

			// Now, we want to remove the edge going out of the chars in zeroIndegreeChars.
			// The way we will do that (and will suffice for this problem) is to reduce the indegree of the chars in their adjacency lists.

			while (zeroIndegreeChars.Length > 0)
			{
				zeroIndegreeChars = RemoveOutgoingEdges(adjList, inDegrees, zeroIndegreeChars);
				order += zeroIndegreeChars;
			}

			return AllIndegreesZero(inDegrees) ? order : "";
		}

		private static bool AllIndegreesZero(Dictionary<char, int> inDegrees)
		{
			foreach (var entry in inDegrees)
			{
				if (entry.Value > 0)
					return false;
			}

			return true;
		}

		private static string RemoveOutgoingEdges(Dictionary<char, List<char>> adjList, Dictionary<char, int> inDegrees, string zeroIndegreeChars)
		{
			string newZeroIndegreeChars = String.Empty;	// this is the next set of chars with 0 indegree (ideally, this will be only one char).

			foreach ( char ch in zeroIndegreeChars)
			{
				if (!adjList.ContainsKey(ch))
					continue;

				List<char> oneAdjList = adjList[ch];

				foreach (char neighbor in oneAdjList)		// for each neighbor char of 'ch'
				{
					--inDegrees[neighbor];

					if (inDegrees[neighbor] == 0)           // We will check right here, otherwise we will need to call FindZeroIndegreeChars evey time (expensive. O ( V ) )
						newZeroIndegreeChars += neighbor;
				}
			}

			return newZeroIndegreeChars;
		}

		private static void BuildAdjacencyList(string[] sortedWords, Dictionary<char, List<char>> adjList, Dictionary<char, int> inDegrees)
		{
			for (int ii=1; ii < sortedWords.Length; ++ ii)
			{
				string prevWord = sortedWords[ii - 1];
				string currentWord = sortedWords[ii];
				char ch = prevWord[0];
				if (!inDegrees.ContainsKey(ch))
					inDegrees[ch] = 0;

				int firstDifferentCharIndex = FindFirstDifferenceIndex(prevWord, currentWord);
				if (firstDifferentCharIndex < 0)
					continue;                   // because u cant infer dependency (i.e., order) from such words. Eg: "her" and "her"  OR  "her" and "here"

				// Now, we can infer that prevWord[ firstDifferentCharIndex ] comes before currentWord[ firstDifferentCharIndex ]
				// Add this relationship (edge) to adjacency list
				char earlierChar = prevWord[firstDifferentCharIndex];
				char laterChar = currentWord[firstDifferentCharIndex];
				if (!adjList.ContainsKey(earlierChar))
					adjList[earlierChar] = new List<char>();
				adjList[earlierChar].Add(laterChar);

				// Insert earlierChar in inDegree and increment indegree count for laterChar.
				// if either of them dont exist in inDegrees, we add  with a value of 0
				if (!inDegrees.ContainsKey(earlierChar))
					inDegrees[earlierChar] = 0;
				if (!inDegrees.ContainsKey(laterChar))
					inDegrees[laterChar] = 0;
				++inDegrees[laterChar];
			}
		}

		/// <summary>
		///  return index of first different character. If no difference, returns -1
		///  Assumption: case is the same (does not check for case differences)
		private static int FindFirstDifferenceIndex(string wordA, string wordB)
		{
			int lengthToCheck = Math.Min(wordA.Length, wordB.Length);
			for (int index = 0; index < lengthToCheck; ++index)
			{
				if (wordA[index] != wordB[index])
					return index;
			}

			return -1;          
			// if we r here, it means the first 'lengthToCheck' letters were the same
			// (ie, EITHER the words were exactly the same, OR the shorter word is a prefix of the longer word)
		}

		// If complete info was given, then only one character should have indegree of 0
		private static string  FindZeroIndegreeChars(Dictionary<char, int> inDegrees)
		{
			string zeroIndegreeChars = String.Empty;

			foreach (var entry in inDegrees)
			{
				if (entry.Value == 0)			// if in-degree of a character is zero, we r interested in it.
					zeroIndegreeChars += entry.Key;
			}

			return zeroIndegreeChars;
		}

	}
}
