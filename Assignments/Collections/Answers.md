# S1 Find a student by national ID — thousands of times a day.

--i will use Dictionary
--> because it pair <key,Value> type provides fast lookup about student id in time complixty O(1)

# S2 Keep the tags of a course. The same tag must never be stored twice.

--i will use Hashset
-->Because it prevent any duplication of insertion the same tag of course

# S3 Keep a student's grades in the order they were entered. The same grade can appear more than once.

--i will use List
--> because it provide storing at the order where they entered and and allow duplications.

# S4 A public method returns the course price list. Callers can read prices but must not add or change any.

--i will use IReadOnlyDictionary
-->becaue it allow read data only

# S5 A timetable keyed by session start time. Sessions are added at any moment, and it must always print in time order.

--i will use SortedDictionary
--> becaus it make sessions stored by their keys

# S6 A method returns results that the caller only loops over once — and may stop early.

--i will use IEnumerable
-->because it allowing results to be generated on demand and stopping early