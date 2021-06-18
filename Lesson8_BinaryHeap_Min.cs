using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex2
{
	internal class BinaryHeap_Min
	{
		static void DoHeapAdd(BinaryHeap_Min minHeap)
		{
			Console.WriteLine("");
			Console.WriteLine("Doing Adds now:");

			Console.WriteLine("Adding 100");

			minHeap.Add(100, new object());
			minHeap.Output("After add 100: ");

			minHeap.Add(1, new object());
			minHeap.Output("After add 1:   ");
		}

		static void DoMinOperations(BinaryHeap_Min minHeap)
		{
			Console.WriteLine("");
			Console.WriteLine("Doing GetMin and DeleteMin operations now:");

			Element e = (Element)minHeap.GetMin();
			Console.WriteLine("GetMin returned: " + e.value);

			uint numDeletes = minHeap.NumElements() - 1;
			for (int ii = 0; ii < numDeletes; ++ii)
			{
				minHeap.DeleteMin();
				Console.WriteLine("After DeleteMin, GetMin returned: " + ((Element)minHeap.GetMin()).value);
			}
		}

		static public void Run()
		{
			Element[] someValues = GenerateSomeValues1();
			//Console.WriteLine("Starting values: " + string.Join(", ", someValues));

			BinaryHeap_Min minHeap1 = new BinaryHeap_Min(someValues);

			DoHeapAdd(minHeap1);
			DoMinOperations(minHeap1);

			// Run this code in ur own time and step thru it.
			// Add another method, say,  GenerateSomeValues2() that will return different values for u to 
			// play around with the min heap

			// Write code for max heap. You can use this code as a starting point, or start from scratch
			// and come back to this code if u want to refer to something
		}
		public uint NumElements()
		{
			return mNumElements;
		}
		public Element GetMin()
		{
			return mHeap[1];        // first or root element. 0th index is not used in this implementation.
		}

		public void Add(int value, Object obj)
		{
			// To keep this code simple, I will just return if no space left in my array
			if (mNumElements == mHeap.Length-1)
			{
				Console.WriteLine("Ran out of space.");
				throw new Exception("Heap array ran out of space.");
			}

			// 1. Append the value, ie, add to end
			mHeap[++mNumElements] = new Element(value, obj);

			// 2. HeapifyUp
			HeapifyUp();
		}

		public void DeleteMin()
		{
			// 1. Replace root with last element.
			mHeap[1] = mHeap[mNumElements--];      // post decrement mNumElements

			// 2. HeapifyDown
			HeapifyDown();
		}

		public void Output(string msg)
		{
			// Console.WriteLine(msg + " " + string.Join(", ", mHeap, 0, mNumElements));  // didnt work. TBD
			//Console.WriteLine(msg + " " + string.Join(", ", mHeap));
		}

		public BinaryHeap_Min()
		{
			const int DEFAULT_CAPACITY = 2000;
			mNumElements = 0;
			mHeap = new Element[DEFAULT_CAPACITY];
		}
		public BinaryHeap_Min(Element[] elements)
		{
			mNumElements = (uint)elements.Length - 1;        // since we dont use element at index 0, we do Length -1 
			mHeap = new Element[mNumElements * 2];              // allocate extra in order to be able to call Add()
			Array.Copy(elements, mHeap, elements.Length);

			BuildHeap();
		}


		uint GetParentIndex(uint index)
		{
			return index / 2;
		}

		uint GetLeftChildIndex(uint index)
		{
			return index << 1;      // same as index * 2;
		}

		uint GetRightChildIndex(uint index)
		{
			return (index << 1) + 1;        // same as index * 2 + 1;
		}

		bool HasLeftChild(uint index)
		{
			return GetLeftChildIndex(index) <= mNumElements;
		}
		bool HasRightChild(uint index)
		{
			return GetRightChildIndex(index) <= mNumElements;
		}

		private void BuildHeap()
		{
			// Remember, there is nothing to be done at the leaf level.
			// We will start at leafLevel - 1
			// So, we start from the parent of the last element
			uint childIndex = mNumElements;
			uint currentIndex = GetParentIndex(mNumElements);
			while (currentIndex > 0)
			{
				HeapifyDown(currentIndex);
				--currentIndex;
				Output("After processing index " + (currentIndex + 1) + ":  ");
			}
		}

		private void HeapifyDown()
		{
			HeapifyDown(1);  // start with root.
		}

		private void HeapifyDown(uint startingIndex)
		{
			uint currentIndex = startingIndex;
			uint smallerChildIndex = GetSmallerChildIndex(currentIndex);

			while ((smallerChildIndex > 0) && (mHeap[smallerChildIndex].value < mHeap[currentIndex].value))
			{
				SwapValues(currentIndex, smallerChildIndex);    // Since child is smaller, swap it with its parent
				currentIndex = smallerChildIndex;
				smallerChildIndex = GetSmallerChildIndex(currentIndex);
			}
		}

		
		private void HeapifyUp()
		{
			// 1. Start with the parent of the last element we just added.
			uint currentIndex = mNumElements;
			uint parentIndex = GetParentIndex(currentIndex);

			while ((parentIndex > 0) && (mHeap[parentIndex].value > mHeap[currentIndex].value))
			{
				SwapValues(currentIndex, parentIndex);    // Since child is smaller, swap it with its parent

				currentIndex = parentIndex;
				parentIndex = GetParentIndex(currentIndex);
			}
		}

		private uint GetSmallerChildIndex(uint parentIndex)
		{
			uint smallerChildIndex = 0;

			if ((parentIndex > 0) && HasRightChild(parentIndex))
			{
				// Take the smaller of left or right.
				// NOTE: I didnt check if HasLeftChild(parentIndex) is true... Is that ok ?
				if (mHeap[GetRightChildIndex(parentIndex)].value < mHeap[GetLeftChildIndex(parentIndex)].value)
				{
					smallerChildIndex = GetRightChildIndex(parentIndex);
				}
				else
				{
					smallerChildIndex = GetLeftChildIndex(parentIndex);
				}
			}
			else if ((parentIndex > 0) && HasLeftChild(parentIndex))
				smallerChildIndex = GetLeftChildIndex(parentIndex);

			return smallerChildIndex;
		}

		private void SwapValues(uint indexA, uint indexB)
		{
			Element valAtB = mHeap[indexB];
			mHeap[indexB] = mHeap[indexA];
			mHeap[indexA] = valAtB;
		}

		static private Element[] GenerateSomeValues1()
		{
			Object oo = new Object();
			Element[] values = new Element[7] 
			{ 
				new Element(0, new Object()), 
				new Element(11, new Object()),
				new Element(1, new Object()),
				new Element(22, new Object()),
				new Element(52, new Object()),
				new Element(72, new Object()),
				new Element(25, new Object())
			};   // we wont use index 0

			return values;
		}

		internal class Element
		{
			public Element(int val, Object obj)
			{
				value = val;
				this.obj = obj;
			}

			internal int value;
			internal Object obj;
		}

		private Element[] mHeap;
		private uint mNumElements;
	}
}
