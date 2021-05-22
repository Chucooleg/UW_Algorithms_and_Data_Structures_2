from typing import List

def num_ways_for_change(n: int, coins: List[int]) -> int:
    '''
    coins: list e.g. [1,5,10,25]
    '''
  
    if n == 0:
        return 0
    
    cache = {0:{0:1}}
    
    # built up answer values
    for i in range(1, n+1):
        
        # {coin 1 as smallest: xx ways, coin 5 as smallest: yy ways, ...}
        cache_i = {}
        
        # look at coins from small to large
        for c in coins:
            # don't consider coins larger than value
            if c > i:
                break
            # for the remaining, use cache
            remaining = i - c
            # num ways using c as the smallest coin
            sum_ways_c_smallest = 0
            # coin and value are exact matches
            if remaining == 0 :
                sum_ways_c_smallest = 1
            # coin is smaller than value
            else:
                # only consider combination of coins with the smallest coin larger than current coin c
                for c_smallest in cache[remaining]:
                    if c_smallest >= c:
                        sum_ways_c_smallest += cache[remaining][c_smallest]

            cache_i[c] = sum_ways_c_smallest
        print(i, cache_i)
        cache[i] = cache_i
    
    return sum(cache[n].values())
