from typing import List
import queue

def get_neighbors_poses(grid, pos):
  # look at neighbors : R, Lower L, Lower C, Lower R
  # include diagonals
  y, x = pos
  neighbors = []
  # R
  if pos[1] < len(grid[0]) - 1:
    if grid[y][x+1]:
      neighbors.append((y, x+1))
  # Lower
  if pos[0] < len(grid) - 1:
    if grid[y+1][x-1]:
      neighbors.append((y+1, x-1))
    if grid[y+1][x]:
      neighbors.append((y+1, x))
    if pos[1] < len(grid[0]) - 1:
      if grid[y+1][x+1]:
        neighbors.append((y+1, x+1))

  return neighbors


def BFS_Traversal(pos, visited, grid):
  # Traversal and accumulate area
  island_area = 0
  q = queue.Queue()
  q.put(pos)
  visited.add(pos)

  while not q.empty():
    curr_pos = q.get()
    island_area += 1
    print(curr_pos, island_area)
    for nei in get_neighbors_poses(grid, curr_pos):
      if not nei in visited:
        q.put(nei)
        visited.add(nei)
  print('island area', island_area)
  return visited, island_area


def get_largest_island(grid: List[List[int]]) -> int:

  max_island = 0
  visited = set()
  for y in range(len(grid)):
    for x in range(len(grid[0])):
      pos = (y, x)
      if pos in visited or grid[y][x] == 0:
        continue
      visited, island_area = BFS_Traversal(pos, visited, grid)
      max_island = max(island_area, max_island)

  return max_island


area = get_largest_island(grid=[[0, 0, 1, 1, 0], [1, 0, 0, 1, 0], [0, 0, 1, 1, 0], [0, 1, 0, 0, 1], [0, 0, 0, 0, 0], [1, 1, 1, 0, 1]])
print('Area=', area)
