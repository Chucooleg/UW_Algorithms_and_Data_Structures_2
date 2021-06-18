import heapq
import math

class StreamingMedianCalculatorWithHeapPackage:
  # Using heapq package

  def __init__(self, initial_values):
    self.max_heap_left = []
    self.min_heap_right = []

    for val in initial_values:
      _ = self.add_number_and_return_median(val)

  def add_number_and_return_median(self, new_element: int) -> float:
    # Init
    if len(self.max_heap_left) == 0 and len(self.min_heap_right) == 0:
        heapq.heappush(self.min_heap_right, new_element)
        # return self.max_heap_left, self.min_heap_right, self.min_heap_right[0]
        return self.min_heap_right[0]
    
    # compare and put into one of the two heaps
    if new_element >= self.min_heap_right[0]:
        heapq.heappush(self.min_heap_right, new_element)
    else:
        heapq.heappush(self.max_heap_left, -new_element)
        
    # balance the two heaps
    if len(self.min_heap_right) - len(self.max_heap_left) > 1:
        move_element = heapq.heappop(self.min_heap_right)
        heapq.heappush(self.max_heap_left, -move_element)
    elif len(self.max_heap_left) - len(self.min_heap_right) > 1:
        move_element = -heapq.heappop(self.max_heap_left)
        heapq.heappush(self.min_heap_right, move_element)
        
    # find median
    right_root = self.min_heap_right[0]
    left_root = -self.max_heap_left[0] if len(self.max_heap_left) else 0
    if len(self.min_heap_right) == len(self.max_heap_left):
        median = (right_root + left_root)/2.0
    elif len(self.min_heap_right) > len(self.max_heap_left):
        median = right_root
    else:
        median = left_root
    
    # return self.max_heap_left, self.min_heap_right, median
    return median



class StreamingMedianCalculator:
  # Using my own heap implementation

  def __init__(self, initial_values):
    self.max_heap_left = []
    self.min_heap_right = []

    for val in initial_values:
      _ = self.add_number_and_return_median(val)

  def add_number_and_return_median(self, new_element: int) -> float:
    # Init
    if len(self.max_heap_left) == 0 and len(self.min_heap_right) == 0:
        myHeap.push(self.min_heap_right, new_element)
        # return self.max_heap_left, self.min_heap_right, self.min_heap_right[0]
        return self.min_heap_right[0]
    
    # compare and put into one of the two heaps
    if new_element >= self.min_heap_right[0]:
        myHeap.push(self.min_heap_right, new_element)
    else:
        myHeap.push(self.max_heap_left, -new_element)
        
    # balance the two heaps
    if len(self.min_heap_right) - len(self.max_heap_left) > 1:
        move_element = myHeap.pop(self.min_heap_right)
        myHeap.push(self.max_heap_left, -move_element)
    elif len(self.max_heap_left) - len(self.min_heap_right) > 1:
        move_element = -myHeap.pop(self.max_heap_left)
        myHeap.push(self.min_heap_right, move_element)
        
    # find median
    right_root = self.min_heap_right[0]
    left_root = -self.max_heap_left[0] if len(self.max_heap_left) else 0
    if len(self.min_heap_right) == len(self.max_heap_left):
        median = (right_root + left_root)/2.0
    elif len(self.min_heap_right) > len(self.max_heap_left):
        median = right_root
    else:
        median = left_root
    
    # return self.max_heap_left, self.min_heap_right, median
    return median



class myHeap:
    
    def __init__(self, init_vals=None):
        self.data = []
        if init_vals:
            self.data = self.heapify(init_vals)
    
    @staticmethod
    def get_left_child_idx(idx):
        return idx*2 + 1
    
    @staticmethod
    def get_right_child_idx(idx):
        return idx*2 + 2
    
    @staticmethod
    def get_right_child_val(array, idx):
        right_child_idx = myHeap.get_right_child_idx(idx)
        if right_child_idx >= len(array):
            return 1e8
        else:
            return array[right_child_idx]
    
    @staticmethod
    def get_left_child_val(array, idx):
        left_child_idx = myHeap.get_left_child_idx(idx)
        if left_child_idx >= len(array):
            return 1e8
        else:
            return array[left_child_idx]
        
    @staticmethod
    def get_parent_idx(array, idx):
        return max(0, (idx + 1) // 2 - 1)
    
    @staticmethod
    def get_level(idx):
        return int(math.log2(idx + 1))
    
    @staticmethod
    def heapify(array):
        # O(N)
        
        # work on nodes from pre-terminal level up to root
        last_idx = len(array) - 1
        last_preterminal_idx = 2**(myHeap.get_level(last_idx)) - 2
        
        # heapify down for each node
        for node_idx in reversed(range(last_preterminal_idx+1)):
            myHeap.heapify_down(array, node_idx)
        
        return array
    
    @staticmethod
    def heapify_up(array, idx):
        # O(log(N))
        curr_idx = idx
        parent_idx = myHeap.get_parent_idx(array, curr_idx)
        while array[parent_idx] > array[curr_idx]:
            array[parent_idx], array[curr_idx] = \
                array[curr_idx], array[parent_idx]
            curr_idx = parent_idx
            parent_idx = myHeap.get_parent_idx(array, curr_idx)
    
    @staticmethod
    def heapify_down(array, idx):
        # O(log(N))
        curr_idx = idx
        while min(myHeap.get_left_child_val(array, curr_idx), myHeap.get_right_child_val(array, curr_idx)) < array[curr_idx]:
            if myHeap.get_left_child_val(array, curr_idx) < myHeap.get_right_child_val(array, curr_idx):
                min_child_idx = myHeap.get_left_child_idx(curr_idx)
            else:
                min_child_idx = myHeap.get_right_child_idx(curr_idx)
            try:
                array[curr_idx], array[min_child_idx] = array[min_child_idx], array[curr_idx]
            except:
                breakpoint()
            curr_idx = min_child_idx
    
    @staticmethod
    def look_root(array):
        return array[0]
    
    @staticmethod
    def pop(array):
        if len(array) == 0:
            return
        elif len(array) == 1:
            last_e = array.pop()
            return last_e
        else:
            last_e = array.pop()
            root = array[0]
            array[0] = last_e
            # heapify down at the root
            myHeap.heapify_down(array=array, idx=0)
            return root
        
    @staticmethod
    def push(array, e):
        array.append(e)
        # heapify up at the end of array
        myHeap.heapify_up(array=array, idx=len(array)-1)
