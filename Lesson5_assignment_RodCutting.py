from typing import Tuple, List

class RodLengthCalculator:
  def __init__(self, rod_length, price_array):
    self.price_array = price_array
    self.rod_length = rod_length
    self.cache = {}

  def find_max_value_top_down_memoization(self) -> Tuple[int, List[int]]:
    self.cache = {0:(self.price_array[0], [0])}
    ans = self.helper_top_down(self.rod_length)
    print(ans)
    return ans

  def helper_top_down(self, rod_length):
    # base case
    if rod_length in self.cache:
      return self.cache[rod_length]
    else:
      max_ = self.price_array[rod_length]
      best_cut = [rod_length]
      for i in range(1, rod_length):
        left_val, left_cut = self.helper_top_down(i)
        right_val, right_cut = self.helper_top_down(rod_length-i)
        cand = left_val + right_val
        if cand > max_:
          max_ = cand
          best_cut = left_cut + right_cut
      self.cache[rod_length] = (max_, best_cut)
    return max_, best_cut
  
  def find_max_value_bottom_up(self) -> Tuple[int, List[int]]:
    self.cache = {0:(self.price_array[0], [0])}
    ans = self.helper_bottom_up(self.rod_length)
    print(ans)
    return ans

  def helper_bottom_up(self, rod_length):
    for i in range(0, rod_length+1): # 0-8
      max_ = self.price_array[i]
      best_cut = [i]
      for j in range(0, i): #i=8, j:1-7
        left_val, left_cut = self.cache[j]
        right_val, right_cut = self.price_array[i-j], [i-j]
        cand = left_val + right_val
        if cand > max_:
          max_ = cand
          best_cut = left_cut + right_cut
      self.cache[i] = (max_, best_cut)
    return max_, best_cut

    
