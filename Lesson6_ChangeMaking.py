from typing import List
from enum import Enum, auto


class ChangeMaking:
    class SolutionApproach(Enum):
        TopDown = auto()
        TopDownMemoization = auto()
        BottomUp = auto()

    def __init__(self, coins):
        """ Assumption: coins is not null and has data. Code does not check or guard against null or empty array. """
        self.mCoins = list(coins)  # copy of coins list
        self.callCount = 0
        self.computeCount = 0

    @staticmethod
    def Run():
        coins = [1, 2, 5]
        cm = ChangeMaking(coins)

        for N in range(10, 21):  # 10 through 20, inclusive
            for approach in ChangeMaking.SolutionApproach:
                print(f"{approach.name}:")
                print(f"N:{N} min coins:{cm.GetMinCoins(N, approach)}")
                print(f"Call count: {cm.callCount} compute count: {cm.computeCount}\n")

    def GetMinCoins(self, N: int, approach: SolutionApproach):
        minValue = float('inf')
        self.callCount = self.computeCount = 0

        if approach is ChangeMaking.SolutionApproach.TopDown:
            minValue = self.__GetMinCoinsTopDown(N)
        elif approach is ChangeMaking.SolutionApproach.BottomUp:
            cache = [-1 for _ in range(N+1)]
            minValue = self.__GetMinCoinsBottomUp(N, cache)
        elif approach is ChangeMaking.SolutionApproach.TopDownMemoization:
            cache = [-1 for _ in range(N+1)]
            minValue = self.__GetMinCoinsTopDownMemoization(N, cache)
        else:
            raise ValueError(f"approach {approach} not supported.")

        return minValue

    def __GetMinCoinsTopDown(self, N: int):
        if N <= 0:
            return 0

        self.callCount += 1
        minCoins = float('inf')
        for coin in self.mCoins:
            if coin > N:
                continue
            minCoins = min(1 + self.__GetMinCoinsTopDown(N-coin), minCoins)
        return minCoins

    def __GetMinCoinsTopDownMemoization(self, N: int, cache: List[int]):
        if N <= 0:
            return 0

        self.callCount += 1

        if cache[N] >= 0:
            return cache[N]

        self.computeCount += 1
        minCoins = float('inf')
        for coin in self.mCoins:
            if coin > N:
                continue

            minCoins = min(1 + self.__GetMinCoinsTopDownMemoization(N-coin, cache), minCoins)

        cache[N] = minCoins
        return cache[N]

    def __GetMinCoinsBottomUp(self, N: int, cache: List[int]):
        cache[0] = 0
        for value in range(1, N+1):  # from 1 to N inclusive
            minCoins = float('inf')

            for coin in self.mCoins:
                if coin > value:
                    continue
                minCoins = min(1 + cache[value-coin], minCoins)

            cache[value] = minCoins
        return cache[N]
