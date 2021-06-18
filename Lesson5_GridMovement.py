import time
from typing import List
from enum import Enum, auto


class GridMovement:
    class SolutionApproach(Enum):
        TopDown = auto()
        TopDownMemoization = auto()
        BottomUp = auto()

    def __init__(self, numRows: int, numCols: int):
        self.debug = True
        self.NumRows = numRows
        self.NumCols = numCols
        self.cost = self.__InitializeGridCost()
        self.DisplayGrid()


    @staticmethod
    def Run():
        numRows = 5
        numCols = 5
        gridm = GridMovement(numRows, numCols)

        for approach in GridMovement.SolutionApproach:
            print(f"{approach.name}:\n"
                    f"*** Diagonal not allowed ***\n"
                    f"Min cost to cell ({numRows-1}, {numCols-1}) = {gridm.FindMinCost(numRows-1, numCols-1, False, approach)}\n"
                    f"*** Diagonal allowed ***\n"
                    f"Min cost to cell ({numRows-1}, {numCols-1}) = {gridm.FindMinCost(numRows-1, numCols-1, True, approach)}\n\n"
                    f"================================================")

    def FindMinCost(self, destinationRow: int, destinationCol: int,
                    diagonalAllowed: bool, approach: SolutionApproach):
        minValue = float("-inf")

        if approach is self.SolutionApproach.TopDown:
            minValue = self.FindMinCostTopDown(destinationRow, destinationCol,
                                                    diagonalAllowed)
        elif approach is self.SolutionApproach.BottomUp:
            minValue = self.FindMinCostBottomUp(destinationRow, destinationCol,
                                                    diagonalAllowed)
        elif approach is self.SolutionApproach.TopDownMemoization:
            pass
        else:
            raise ValueError(f"approach {approach} not supported.")

        return minValue

    def DisplayGrid(self):
        if not self.debug:
            return

        print("\n".join(
                "  ".join(f"{self.cost[row][col]: >4}" for col in range(self.NumCols))
            for row in range(self.NumRows)))

    def FindMinCostTopDown(self, destinationRow: int, destinationCol: int, diagonalAllowed: bool):
        minCost = None  # TODO unused??
        callCounter = [0]
        repetitiveCalculationsCounter = [[0 for _ in range(self.NumCols)] for _ in range(self.NumRows)]

        start = time.time()

        result = self.__Helper_FindMinCostTopDown(destinationRow, destinationCol,
                    diagonalAllowed, callCounter, repetitiveCalculationsCounter)

        end = time.time()
        print(f"Time: {int(1000*(end-start))} ms. Num recursive calls: {callCounter[0]}")
        print("Num calls for each cell in the grid:")
        print("\n".join(
                ", ".join(f"{repetitiveCalculationsCounter[row][col]: >7}" for col in range(self.NumCols))
            for row in range(self.NumRows)))
        print(f"totalCallsCount: {sum(sum(arr) for arr in repetitiveCalculationsCounter)}")

        return result

    def __Helper_FindMinCostTopDown(self, row: int, col: int, diagonalAllowed: bool, callCounter: List[int], repetitiveCalculationsCounter: List[List[int]]):
        if row == 0 and col == 0:
            return self.cost[0][0]
        elif row < 0 or col < 0:
            return float("inf")
        callCounter[0] += 1
        repetitiveCalculationsCounter[row][col] += 1

        comingFromLeft = self.__Helper_FindMinCostTopDown(row, col-1, diagonalAllowed, callCounter, repetitiveCalculationsCounter)
        comingFromAbove = self.__Helper_FindMinCostTopDown(row-1, col, diagonalAllowed, callCounter, repetitiveCalculationsCounter)
        comingDiagonally = float("inf")
        if diagonalAllowed:
            comingDiagonally = self.__Helper_FindMinCostTopDown(row-1, col-1, diagonalAllowed, callCounter, repetitiveCalculationsCounter)
        return self.cost[row][col] + min(min(comingFromLeft, comingFromAbove), comingDiagonally)

    def FindMinCostBottomUp(self, destinationRow: int, destinationCol: int, diagonalAllowed: bool):
        minCost = self.__InitializeMinCost(self.cost)
        start = time.time()
        for row in range(1, destinationRow+1):
            for col in range(1, destinationCol+1):
                minCost[row][col] = self.cost[row][col] + self.__GetMinCostToNeighbor(row, col, minCost, diagonalAllowed)
        end = time.time()
        print(f"Time: {int(1000*(end-start))} ms")
        return minCost[destinationRow][destinationCol]

    def __GetMinCostToNeighbor(self, row: int, col: int, minCost: List[List[int]],
                                diagonalAllowed: bool):
        costToLeftCell = minCost[row][col-1]
        costToAboveCell = minCost[row-1][col]
        minCostToNeighbor = min(costToLeftCell, costToAboveCell)
        if diagonalAllowed:
            minCostToLeftDiagonal = minCost[row-1][col-1]
            minCostToNeighbor = min(minCostToLeftDiagonal, minCostToNeighbor)
        return minCostToNeighbor


    def __InitializeMinCost(self, cost: List[List[int]]):
        minCost = [[0 for _ in range(self.NumCols)] for _ in range(self.NumRows)]
        for col in range(1, self.NumCols):
            minCost[0][col] = minCost[0][col-1] + cost[0][col]
        for row in range(1, self.NumRows):
            minCost[row][0] = minCost[row-1][0] + cost[row][0]
        return minCost

    def __InitializeGridCost(self):
        return [[i + j for j in range(self.NumCols)] for i in range(self.NumRows)]
