import random
import time
import copy

def PickAPivotIndex1(start, end):
  return start

def PickAPivotIndex2(start, end):
  # return random.randint(a=start, b=end)
  return (start+end)//2

def Partition(A, start, end, pivotIndex):
  A[start], A[pivotIndex] = A[pivotIndex], A[start]
  new_start = start + 1
  LeftMostBiggest = new_start

  for i in range(new_start, end+1):
    if A[i] <= A[start]:
      A[i], A[LeftMostBiggest] = A[LeftMostBiggest], A[i]
      LeftMostBiggest += 1

  finalPivotIndex = LeftMostBiggest-1
  A[start], A[finalPivotIndex] = A[finalPivotIndex], A[start]

  return finalPivotIndex

def QSort(A, start, end, pickPivot_fn):
  if start >= end:
    return

  pivotIndex = pickPivot_fn(start, end)
  finalPivotIndex = Partition(A, start, end, pivotIndex)

  QSort(A, start, finalPivotIndex-1, pickPivot_fn)
  QSort(A, finalPivotIndex+1, end, pickPivot_fn)

# A = [10, 6, 1, 9]
# QSort(A, start=0, end=3, pickPivot_fn=PickAPivotIndex1)
# print(A)
# A = [10, 6, 1, 9]
# QSort(A, start=0, end=3, pickPivot_fn=PickAPivotIndex2)
# print(A)

N = 2000_000
sorted_array = [i for i in range(N)]
unsorted_array = [random.randint(0,N-1) for _ in range(N)]
sorted_array_c = sorted_array.copy()
unsorted_array_c = unsorted_array.copy()

start_time = time.time()
QSort(sorted_array, start=0, end=3, pickPivot_fn=PickAPivotIndex1)
print('PickAPivotIndex1 run time for sorted array =', time.time() - start_time)

start_time = time.time()
QSort(unsorted_array, start=0, end=3, pickPivot_fn=PickAPivotIndex1)
print('PickAPivotIndex1 run time for unsorted array =', time.time() - start_time)

start_time = time.time()
QSort(sorted_array_c, start=0, end=3, pickPivot_fn=PickAPivotIndex2)
print('PickAPivotIndex2 run time for sorted array =', time.time() - start_time)

start_time = time.time()
QSort(unsorted_array_c, start=0, end=3, pickPivot_fn=PickAPivotIndex2)
print('PickAPivotIndex2 run time for unsorted array =', time.time() - start_time)

### Results
# PickAPivotIndex1 run time for sorted array = 3.981590270996094e-05
# PickAPivotIndex1 run time for unsorted array = 1.1444091796875e-05
# PickAPivotIndex2 run time for sorted array = 1.3113021850585938e-05
# PickAPivotIndex2 run time for unsorted array = 1.0013580322265625e-05

### Observations:
# On the unsorted array, the two schemes perform about this same.
# On the sorteed array, scheme 1 (pick the start) is much slower than scheme 2 (pick the middle). Scheme 1, by always choosing the smallest element in the current list, doesn't really divide up the list well for conquering.
