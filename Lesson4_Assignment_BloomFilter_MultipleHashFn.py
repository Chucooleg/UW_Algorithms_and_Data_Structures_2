class BloomFilterSingleArray:
  '''Multiple Hash functions; Single data array'''
  
  def __init__(self, hash_array_size, hash_functions, debug=False):
    '''
    hash_array_size: int
    hash_functions: a list of hash functions.
    '''
    self.hash_array_size = hash_array_size
    self.hash_array = [0] * self.hash_array_size
    self.size_int = 32
    self.BF_size = self.hash_array_size * self.size_int
    self.hash_functions = hash_functions
    self.debug = debug

  def get_BFi(self, url, hash_fn):
    hash_val = hash_fn(url)
    if self.debug:
      print('get_BFi', hash_val, self.BF_size)
    BFi = hash_val % self.BF_size
    if self.debug:
      print('get_BFi', BFi)
    return BFi

  def add(self, url: str):
    print('adding url: ', url)

    # find BFi for each hash_function
    BFi_list = [self.get_BFi(url, hash_fn) for hash_fn in self.hash_functions]

    if self.debug:
      print('BFi list:', BFi_list)
      print('before-add hash array positions:', [bin(self.hash_array[BFi // self.size_int]) for BFi in BFi_list])

    for BFi in BFi_list:
      # find position, bit-AND, left shift
      self.hash_array[BFi // self.size_int] = self.hash_array[BFi // self.size_int] | (1 << BFi % self.size_int)

    if self.debug:
      print('post-add hash array positions:', [bin(self.hash_array[BFi // self.size_int]) for BFi in BFi_list])

    return self.hash_array

  def contains(self, url: str) -> bool:
    print('checking url: ', url)

    # find BFi for each hash_function
    BFi_list = [self.get_BFi(url, hash_fn) for hash_fn in self.hash_functions]

    if self.debug: 
      print('BFi list:', BFi_list)
      print('contains at hash array positions:', [bin(self.hash_array[BFi // self.size_int]) for BFi in BFi_list])

    founds = [self.hash_array[BFi // self.size_int] & ( 1 << BFi % self.size_int) > 0 for BFi in BFi_list]

    if self.debug:
      print('founds:', founds)

    found = all(founds)
    print(found)
    return found
    

## referenced from course 1
def get_hash_1(string):
    if not string:
        return 0

    hash_value = 0
    for ii in range(len(string)):
        hash_value += ord(string[ii]) # simplify this, to create easy collisions for our example

    return hash_value

## referenced from course 1
def get_hash_2(string):
    if not string:
        return 0

    hash_value = 0
    for ii in range(len(string)):
        hash_value += ((hash_value << 4) + hash_value) + (ord(string[ii]) * ii)
        #hash_value = hash_value * 17 + str[ii] * ii;

    return hash_value

def get_hash_3(string):
    if not string:
        return 0

    hash_value = 0
    for ii in range(len(string)):
        hash_value += (ord(string[ii]) * (2**ii))

    return hash_value

# Test
print('---------One data array for all hash functions-----------')
BF = BloomFilterSingleArray(hash_array_size=2, hash_functions=[get_hash_1, get_hash_2, get_hash_3], debug=False)
BF.add(url='https://replit.com/@UWPCE/SDDE-320-Assignment-4-Python-chucoolegUW2#main.py')
BF.add(url='https://ai.googleblog.com/2021/05/google-at-iclr-2021.html')

BF.contains(url='https://replit.com/@UWPCE/SDDE-320-Assignment-4-Python-chucoolegUW2#main.py')
BF.contains(url='https://ai.googleblog.com/2021/05/google-at-iclr-2021.html')
BF.contains(url='https://mathai-iclr.github.io/papers/posters/MATHAI_29_poster.png')


class BloomFilter:
  '''Multiple Hash functions; Multiple data arrays'''
  
  def __init__(self, hash_array_size, hash_functions, debug=False):
    '''
    hash_array_size: int
    hash_functions: a list of hash functions.
    '''
    self.hash_array_size = hash_array_size
    self.hash_arrays = [[0] * self.hash_array_size for _ in range(len(hash_functions))]
    self.size_int = 32
    self.BF_size = self.hash_array_size * self.size_int
    self.hash_functions = hash_functions
    self.debug = debug

  def get_BFi(self, url, hash_fn):
    hash_val = hash_fn(url)
    if self.debug:
      print('get_BFi', hash_val, self.BF_size)
    BFi = hash_val % self.BF_size
    if self.debug:
      print('get_BFi', BFi)
    return BFi

  def add(self, url: str):
    print('adding url: ', url)
    # find BFi for each hash_function
    BFi_list = [self.get_BFi(url, hash_fn) for hash_fn in self.hash_functions]

    if self.debug:
      print('BFi list:', BFi_list)
      print('before-add hash array positions:', [bin(self.hash_arrays[h_i][BFi // self.size_int]) for h_i, BFi in enumerate(BFi_list)])

    for h_i, BFi in enumerate(BFi_list):
      # find position, bit-AND, left shift
      self.hash_arrays[h_i][BFi // self.size_int] = self.hash_arrays[h_i][BFi // self.size_int] | (1 << BFi % self.size_int)

    if self.debug:
      print('post-add hash array positions:', [bin(self.hash_arrays[h_i][BFi // self.size_int]) for h_i, BFi in enumerate(BFi_list)])

    return self.hash_arrays

  def contains(self, url: str) -> bool:
    print('checking url: ', url)

    # find BFi for each hash_function
    BFi_list = [self.get_BFi(url, hash_fn) for hash_fn in self.hash_functions]

    if self.debug: 
      print('BFi list:', BFi_list)
      print('contains at hash array positions:', [bin(self.hash_array[h_i][BFi // self.size_int]) for h_i, BFi in enumerate(BFi_list)])

    founds = [self.hash_arrays[h_i][BFi // self.size_int] & ( 1 << BFi % self.size_int) > 0 for h_i, BFi in enumerate(BFi_list)]

    if self.debug:
      print('founds:', founds)

    found = all(founds)
    print(found)
    return found

# Test
print('---------One data array per hash function-----------')
BF = BloomFilter(hash_array_size=2, hash_functions=[get_hash_1, get_hash_2, get_hash_3], debug=False)
BF.add(url='https://replit.com/@UWPCE/SDDE-320-Assignment-4-Python-chucoolegUW2#main.py')
BF.add(url='https://ai.googleblog.com/2021/05/google-at-iclr-2021.html')

BF.contains(url='https://replit.com/@UWPCE/SDDE-320-Assignment-4-Python-chucoolegUW2#main.py')
BF.contains(url='https://ai.googleblog.com/2021/05/google-at-iclr-2021.html')
BF.contains(url='https://mathai-iclr.github.io/papers/posters/MATHAI_29_poster.png')
