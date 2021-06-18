from typing import List
from enum import Enum, auto


class EditD:
    class SolutionApproach(Enum):
        TopDown = auto()
        TopDownMemoization = auto()
        BottomUp = auto()

    def __init__(self):
        # privates
        self.callCount = 0
        self.computeCount = 0
        self.debug = True

    def __DisplayArray(self, arr: List[int], a: str, b: str):
        if not self.debug:
            return
        for row in range(len(arr)):
            if row == 0:
                print("\t\t", end='')
                print(f"\t".join(b))
            if row > 0:
                print(f"{a[row-1]}\t", end='')
            else:
                print("\t", end='')
            print(f"\t".join(str(arr[row][col]) for col in range(len(arr[0]))))
        print()

    @staticmethod
    def Run():
        e1 = EditD()

        # spend vs spendy: first char processed is different
        # abate vs bate:   last  char processed is different. Call count is lot less than when only first chat to be processed was different (bcos all initial chars processed are the same, so nothing to be done).
        # spar  vs spear:  one char in between is different.
        # abcde vs ghcpt:  one char in between is same.
        # abcdefghi vs mnopqrstu: ALL chars are different
        # abcdefghij vs mnopqrstuv: ALL chars are different. Length is one more than previous strings
        # abcdefghiz vs mnopqrstuz: ALL chars EXCEPT last char are different.
        # abzzcd vs abzzcd: ALL chars SAME. edit distance should be 0.
        # abzzcd vs abzzcd: ALL chars SAME. edit distance should be 0.

        aStrings = ["spend",  "abate", "spar",  "abcde", "sunday",   "appreciate", "abcdefghi", "abcdefghij", "abcdefghiz", "abzzcd", "abzzcd"]
        bStrings = ["spendy", "bate",  "spear", "ghcpt", "saturday", "exaggerate", "mnopqrstu", "mnopqrstuv", "mnopqrstuz", "abzzcd", "aczzbf"]
        for strIndex in range(len(aStrings)):
            for approach in EditD.SolutionApproach:
                print(f"{approach.name}:")
                print(f"a: {aStrings[strIndex]} b: {bStrings[strIndex]} Edit distance: {e1.GetEditDistance(aStrings[strIndex], bStrings[strIndex], approach)}")
                print(f"Call count: {e1.callCount} compute count: {e1.computeCount}\n")
            print("================================================")

    def GetEditDistance(self, a: str, b: str, approach: SolutionApproach):
        editDistance = -1
        self.callCount = self.computeCount = 0
        if approach is EditD.SolutionApproach.TopDown:
            editDistance = self.__EditDistanceTopDown(a, len(a), b, len(b))
        elif approach is EditD.SolutionApproach.BottomUp:
            cache = [[float('inf') for _ in range(len(b)+1)] for _ in range(len(a)+1)]
            self.__DisplayArray(cache, a, b)
            editDistance = self.__EditDistanceBottomUp(a, b, cache)
            print("At end:")
            savdebug = self.debug
            debug = True
            self.__DisplayArray(cache, a, b)
            self.debug = savdebug
        elif approach is EditD.SolutionApproach.TopDownMemoization:
            cache = [[-1 for _ in range(len(b))] for _ in range(len(a))]
            editDistance = self.__EditDistanceTopDownMemoization(a, len(a), b, len(b), cache)
        else:
            raise ValueError(f"approach {approach} not supported.")
        return editDistance

    def __EditDistanceBottomUp(self, a: str, b: str, cache: List[int]):
        for col in range(len(cache[0])):
            cache[0][col] = col
        for row in range(len(cache)):
            cache[row][0] = row
        self.__DisplayArray(cache, a, b)

        for row in range(1, len(cache)):
            for col in range(1, len(cache[0])):
                aChar = a[row-1]
                bChar = b[col-1]
                if aChar == bChar:
                    cache[row][col] = cache[row-1][col-1]
                else:
                    cache[row][col] = 1 + min(
                        cache[row][col-1],  # insert case
                        cache[row-1][col-1],  # replace case
                        cache[row-1][col]  # delete case
                    )
                self.__DisplayArray(cache, a, b)
            self.__DisplayArray(cache, a, b)
        return cache[len(a)][len(b)]

    def __EditDistanceTopDownMemoization(self, a: str, aLength: int, b: str, bLength: int, cache: List[int]):
        if aLength == 0:
            return bLength
        elif bLength == 0:
            return aLength

        self.callCount += 1

        if cache[aLength-1][bLength-1] >= 0:
            return cache[aLength-1][bLength-1]

        aChar = a[aLength-1]
        bChar = b[bLength-1]

        # if aChar and bChar are the same, there is no action to be done... we ignore them and move on.
        if aChar == bChar:
            return self.__EditDistanceTopDownMemoization(a, aLength-1, b, bLength-1, cache)

        # if we are here, it means last chars of a and b are NOT the same,
        # so we have a choice of 3 actions (replace, insert, delete) and we want the min of those 3
        replaceOption = 1 + self.__EditDistanceTopDownMemoization(a, aLength-1, b, bLength-1, cache)  # We replace char a[ aLength-1 ], we have processed that char, so we move left on both a and b.
        insertOption = 1 + self.__EditDistanceTopDownMemoization(a, aLength, b, bLength - 1, cache)  # We insert b[ bLength-1 ] into a, so we processed b's last char, move that to left, but we still need to process aLength chars of a
        deleteOption = 1 + self.__EditDistanceTopDownMemoization(a, aLength - 1, b, bLength, cache)  # We delete char a[ aLength-1 ], and move left on a, but not on b since b[ bIndex ] still needs to be taken care of

        cache[aLength-1][bLength-1] = min(replaceOption, insertOption, deleteOption)
        return cache[aLength-1][bLength-1]

    def __EditDistanceTopDown(self, a: str, aLength: int, b: str, bLength: int):
        if aLength == 0:
            return bLength
        elif bLength == 0:
            return aLength

        self.callCount += 1

        aChar = a[aLength-1]
        bChar = b[bLength-1]

        # if aChar and bChar are the same, there is no action to be done... we ignore them and move on.
        if aChar == bChar:
            return self.__EditDistanceTopDown(a, aLength-1, b, bLength-1)

        # if we are here, it means last chars of a and b are NOT the same,
        # so we have a choice of 3 actions (replace, insert, delete) and we want the min of those 3
        replaceOption = 1 + self.__EditDistanceTopDown(a, aLength - 1, b, bLength - 1)	# We replace char a[ aLength-1 ], we have processed that char, so we move left on both a and b.
        insertOption  = 1 + self.__EditDistanceTopDown(a, aLength,     b, bLength - 1)	# We insert b[ bLength-1 ] into a, so we processed b's last char, move that to left, but we still need to process aLength chars of a
        deleteOption  = 1 + self.__EditDistanceTopDown(a, aLength - 1, b, bLength)     # We delete char a[ aLength-1 ], and move left on a, but not on b since b[ bIndex ] still needs to be taken care of

        return min(replaceOption, insertOption, deleteOption)


if __name__ == '__main__':
    arr = [[0, 0, 0] for _ in range(3)]
    a = "fatty"
    b = "dino"
    EditD().DisplayArray(arr, a, b)
