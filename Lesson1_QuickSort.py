from random import randint
from typing import List

class QuickSort:
    _do_debug = False

    @staticmethod
    def do_quick_sort(A:List[int]):
        print("quicksort")
        QuickSort.quick_sort(A, 0, len(A) - 1)
        print(*A)

    @staticmethod
    def quick_sort(A:List[int], start:int, end:int):
        if start >= end:
            if QuickSort._do_debug:
                print(f"start, end: {start}, {end}")
            return
        pivot_index = QuickSort.pick_pivot(start, end)
        pivot_final_index = QuickSort.partition(A, start, end, pivot_index)

        QuickSort.quick_sort(A, start, pivot_final_index-1)
        QuickSort.quick_sort(A, pivot_final_index+1, end)

    @staticmethod
    def partition(A:List[int], start:int, end:int, pivot_index:int):
        pivot_value = A[pivot_index]
        A[start], A[pivot_index] = A[pivot_index], A[start]

        left_most_bigger_index = start + 1
        for i in range(start+1, end+1):
            if A[i] < pivot_value:
                A[i], A[left_most_bigger_index] = A[left_most_bigger_index], A[i]
                left_most_bigger_index += 1
        pivot_final_index = left_most_bigger_index - 1
        A[start], A[pivot_final_index] = A[pivot_final_index], A[start]

        if QuickSort._do_debug:
            print(f"pivot_final_index: {pivot_final_index}")

        return pivot_final_index

    @staticmethod
    def pick_pivot(start:int, end:int):
        return randint(start, end)  # random integer in [start, end] inclusive
