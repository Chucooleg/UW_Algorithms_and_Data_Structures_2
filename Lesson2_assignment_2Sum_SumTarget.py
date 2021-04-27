from typing import Tuple

def merge_sort(array):
  '''merge sort'''
  if len(array) <= 1:
    return array

  mid = len(array)//2
  sortedL = merge_sort(array[:mid])
  sortedR = merge_sort(array[mid:])

  sortedLR = []
  idxR, idxL = 0, 0
  while idxL < len(sortedL) and idxR < len(sortedR):
    if sortedL[idxL] < sortedR[idxR]:
      sortedLR.append(sortedL[idxL])
      idxL += 1
    else:
      sortedLR.append(sortedR[idxR])
      idxR += 1
  if idxL < len(sortedL):
    sortedLR += sortedL[idxL:]
  elif idxR < len(sortedR):
    sortedLR += sortedR[idxR:]
  return sortedLR

# merge_sort(array=[1,3,2,4,5])


def find_intersection(array_one, array_two) -> list: 
  array_one = merge_sort(array_one)
  array_two = merge_sort(array_two)
  print(array_one, array_two)
  ans = []
  p1 = 0
  p2 = 0
  while p1 < len(array_one) and p2 < len(array_two):
    if array_one[p1] == array_two[p2]:
      if not (len(ans) > 0 and array_one[p1] == ans[-1]):
        ans.append(array_one[p1])
      p1 += 1
      p2 += 1
    elif array_one[p1] < array_two[p2]:
      p1 += 1
    else:
      p2 += 1
  return ans

# print(find_intersection([1,3,6,7,9],[2,0,15,1,6]))

def two_sum(twoSumSource, target) -> Tuple[int, int]:
  if len(twoSumSource) == 0:
    return (-1, -1)
  # O(N), Space O(N)
  lookup = {e:i for i, e in enumerate(twoSumSource)}
  # O(NlogN)
  twoSumSource = merge_sort(twoSumSource)
  # O(N)
  pL, pR = 0, len(twoSumSource)-1
  while pL < pR:
    sum_ = twoSumSource[pL] + twoSumSource[pR]
    if sum_ < target:
      pL += 1
    elif sum_ > target:
      pR -= 1
    else:
      return (
        min(lookup[twoSumSource[pL]], lookup[twoSumSource[pR]]),
        max(lookup[twoSumSource[pL]], lookup[twoSumSource[pR]])
        )
  return (-1, -1)

print(two_sum([1,7,6,3,9], 9))
