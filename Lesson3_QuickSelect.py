from quick_sort import QuickSort
from typing import List

class QuickSelect:
    @staticmethod
    def do_quick_select(A:List[int], index_of_interest:int):
        return QuickSelect.__quick_select(A, 0, len(A)-1, index_of_interest)

    @staticmethod
    def __quick_select(A:List[int], start:int, end:int, index_of_interest:int):
        if start >= end:
            if start != index_of_interest:
                print("Error: start != index_of_interest: " +
                        f"{start}, {index_of_interest}")
                return -1
            else:
                return A[start]
        pivot_index = QuickSort.pick_pivot(start, end)
        pivot_final_index = QuickSort.partition(A, start, end, pivot_index)

        if pivot_final_index == index_of_interest:
            return A[index_of_interest]
        elif index_of_interest < pivot_final_index:
            return QuickSelect.__quick_select(A, start, pivot_final_index-1,
                                index_of_interest)
        else:
            return QuickSelect.__quick_select(A, pivot_final_index+1, end, index_of_interest)
