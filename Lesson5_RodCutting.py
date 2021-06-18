from typing import List
from enum import Enum, auto


class RodCutting:
    class SolutionApproach(Enum):
        TopDown = auto()
        TopDownMemoization = auto()
        BottomUp = auto()

    def __init__(self, prices: List[int]):
        self.mPrices = prices.copy()
        self.callCount = 0
        self.computeCount = 0

    @staticmethod
    def RodCuttingDriver():
        RodCutting.Run()

    @staticmethod
    def Run():
        prices = [0, 1, 6, 2, 4, 3, 3, 2, 8]
        rc = RodCutting(prices)
        N = 8
        for approach in RodCutting.SolutionApproach:
            print(f"{approach.name}:\n"
                    f"Length: {N} MaxValue: {rc.FindMaxValue(N, approach)}\n"
                    f"Call count: {rc.callCount} compute count: {rc.computeCount}\n")

    def FindMaxValue(self, N: int, approach: SolutionApproach):
        maxValue = float("-inf")
        self.callCount = self.computeCount = 0

        if approach is self.SolutionApproach.TopDown:
            maxValue = self.__FindMaxValueTopDown(N)
        elif approach is self.SolutionApproach.BottomUp:
            cache = [-1 for _ in range(N+1)]
            maxValue = self.__FindMaxValueBottomUp(N, cache)
        elif approach is self.SolutionApproach.TopDownMemoization:
            cache = [-1 for _ in range(N+1)]
            maxValue = self.__FindMaxValueTopDownMemoization(N, cache)
        else:
            raise ValueError(f"approach {approach} not supported.")

        return maxValue

    def __FindMaxValueTopDown(self, N: int):
        if N <= 0:
            return 0

        self.callCount += 1
        self.computeCount += 1
        maxValueSoFar = -1
        for cutRod in range(1, N+1):
            maxValueSoFar = max(self.mPrices[cutRod] + self.__FindMaxValueTopDown(N-cutRod),
                                maxValueSoFar)
        return maxValueSoFar

    def __FindMaxValueTopDownMemoization(self, N: int, cache: List[int]):
        if N <= 0:
            return 0

        self.callCount += 1

        if cache[N] >= 0:
            return cache[N]

        self.computeCount += 1

        maxValueSoFar = -1
        for cutRod in range(1, N+1):
            maxValueSoFar = max(self.mPrices[cutRod] + self.__FindMaxValueTopDownMemoization(N-cutRod, cache),
                                maxValueSoFar)
        cache[N] = maxValueSoFar
        return maxValueSoFar

    def __FindMaxValueBottomUp(self, N: int, cache: List[int]):
        cache[0] = 0
        for rodLen in range(1, N+1):
            cache[rodLen] = self.__FindMaxValueForLen(rodLen, cache)

        return cache[N]

    def __FindMaxValueForLen(self, N: int, cache: List[int]):
        maxValue = -1
        for cut in range(1, N+1):
            maxValue = max(self.mPrices[cut] + cache[N-cut],
                            maxValue)
        return maxValue
