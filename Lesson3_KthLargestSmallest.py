from quick_select import QuickSelect
from typing import List

class KthLargestSmallest:
    @staticmethod
    def get_kth_largest(A:List[int], K:int):
        index_of_interest = len(A) - K
        if index_of_interest < 0 or index_of_interest >= len(A):
            return -1  # error case
        return QuickSelect.do_quick_select(A, index_of_interest)

    @staticmethod
    def get_kth_smallest(A:List[int], K:int):
        index_of_interest = K-1
        if index_of_interest < 0:
            return -1  # error case
        return QuickSelect.do_quick_select(A, index_of_interest)
