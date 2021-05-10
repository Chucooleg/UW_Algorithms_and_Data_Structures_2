# Lab

def get_hash(string):
    if not string:
        return 0

    hash_value = 0
    for ii in range(len(string)):
        hash_value += ord(string[ii]) # simplify this, to create easy collisions for our example

    return hash_value

def get_BFi(value, hash_array, size_int=32, debug=False):
    hash_value = get_hash(value)
    # Bloom Filter Index
    BF_size = len(hash_array) * size_int # python default datatype for integers is 32-bit
    if debug: 
        print(hash_value, BF_size)
    BFi = hash_value % BF_size
    if debug: 
        print(BFi)
    return BFi

def add(value, hash_array, size_int=32, debug=False):
    BFi = get_BFi(value, hash_array, size_int, debug)
    # find position, bit-AND, left shift
    if debug: 
        print(bin(hash_array[BFi // size_int]))
    hash_array[BFi // size_int] = hash_array[BFi // size_int] | ( 1 << BFi % size_int)
    if debug: 
        print(bin(hash_array[BFi // size_int]))
    return hash_array
    
def contains(value, hash_array, size_int=32, debug=False):
    BFi = get_BFi(value, hash_array, size_int, debug)
    # find position, bit-AND, left shift
    if debug: 
        print('contains', bin(hash_array[BFi // size_int]))
    found = hash_array[BFi // size_int] & ( 1 << BFi % size_int)
    if debug: 
        print('contains', bin(hash_array[BFi // size_int]))
        print('contains', found)
    return found > 0
  
##########################################################################
hash_array = [0,0,0]
add('ABC', hash_array, size_int=32)
add('IJK', hash_array, size_int=32)
contains('ABC', hash_array, debug=True)
contains('IJK', hash_array, debug=True)
contains('DEF', hash_array, debug=True)
