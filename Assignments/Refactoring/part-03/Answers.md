# Part 03 — answers

---

## BlockedUsers

- Time complexity before: O(N*M)
- Time (ms) before: 54m
- What did you change?
 - I optimized the algorithm by using a hash set to store blocked users instead of iterating through the list of blocked users for each user.
    This reduced the time complexity from O(N*M) to O(N)
- Time complexity after: O(N)
- Time (ms) after: 5m

---

## Students

- What was the problem?
- the problem was that all students were being added to the list of studentd even user may not use all of them 
- but use anumber of them only which was causing memory allocations overhead and performance issues.

- What did you change?
- i changed the code to only add students to the list when they are actually used, instead of adding all students at once.
- by using lazy loading, we can reduce memory usage and improve performance.
- by yeild keyword and Ienumrable.
