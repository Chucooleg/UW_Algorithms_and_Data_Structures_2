from typing import List

class InsertionSort:
    @staticmethod
    def insertion_sort(A:List[int], recursive:bool=False):
        if recursive:
            print("Insertion sort recursive")
            InsertionSort.in_sort_recursive(A, len(A) - 1)
        else:
            print("Insertion sort iterative")
            InsertionSort.in_sort_iterative(A, len(A) - 1)

        # prints each element of A with spaces in between
        print(*A)

    @staticmethod
    def in_sort_recursive(A:List[int], N:int):
        if N <= 0:
            return

        InsertionSort.in_sort_recursive(A, N-1)

        for i in range(N, 0, -1):
            if A[i] >= A[i-1]:  # found appropriate position for A[i]
                break
            A[i], A[i-1] = A[i-1], A[i]

    @staticmethod
    def in_sort_iterative(A:List[int], N:int):
        for i in range(1, N+1):
            for j in range(i, 0, -1):
                if A[j] >= A[j-1]:  # found appropriate position for A[j]
                    break
                A[j], A[j-1] = A[j-1], A[j]
