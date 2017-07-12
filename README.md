# MA-SIB
Multi Agent Snake In the Box

Solver project for the SIB problem with heuristic search

Implemented in C# you can find here:

Algorithms:

A* & DFBnB - Maximal varient

Problems:

Snake - Single snake in the box

Box - Multiple snakes with cartesian product of all snakes when expending a node

Box-od - Instead of cartesian product, we are using operator decomposition


The project runs as an console application that gets arguments and write the output solution (and some other stuff) to a file in the folder "RunningLogs" that will be created in the process path.

Running the project without arguments will generate the folowing message:
```
Please Provide arguments to run:
all args should be in the form of: [key]=[value] with space between them
Arguments:
----------
problem:     [snake/box/box-od] snake is single agent & box is multi-agent
             if you choose box you must provide snakes initial locations
             with arguments Sx=location
Sx:          starting location of snake number x, counting from 0
             when using snake you can have only 1 Sx argument
snakeH:      [none/legal/reachable] the snake heuristic
boxH:        [none/legal/reachable] the box heuristic
alg:         [astar/dfbnb] the solving algorithm
dim:         the number of dimentions for the problem (N)
snakeSpread: the intra-snake spread (sK)
boxSpread:   the inter-snake spread (bK)
memTest:     if set to true, will not solve nothing, only fill memory
             allocation to check 64bit issue

Start examples:
---------------
this is how to solve single snake problem with A* (head at 0=(00000))
when the dimention is set to 5 and snake spread is 2:
MaSiB problem=snake S0=0 alg=astar dim=5 snakeSpread=2

this is how to solve multiple snake problem with A*-OD
when the dimention is set to 7, intra-snake spread is 2
and inter-snake spread is 3, the starting locations are 0-(0000000)
and 127-(1111111) so we have 2 snakes
MaSiB problem=box-od s0=0 s1=127 alg=dfbnb dim=7 snakeSpread=2 boxSpread=3 boxh=snakes-sum snakeh=reachable
```
Build:

The release is configured to 64bit since the problem can really grow huge with the right parameters

Results:

output example:
```
MaSiB problem=snake S0=0 alg=astar dim=5 snakeSpread=2
[12/07/2017 22:43:08|Off    ]Log started.
[12/07/2017 22:43:08|Warning]boxSpread not found, setting it to:2
[12/07/2017 22:43:08|Info   ]Solviong snakes in the box problem:
[12/07/2017 22:43:08|Info   ][[Algorithm:AStarMax]]
[12/07/2017 22:43:08|Info   ][[Problem:snake]]
[12/07/2017 22:43:08|Info   ][[WorldDimentions:5]]
[12/07/2017 22:43:08|Info   ][[SnakeSpread:2]]
[12/07/2017 22:43:08|Info   ][[BoxSpread:2]]
[12/07/2017 22:43:08|Info   ][[SnakeHeuristics:SnakeNoneHeuristic]]
[12/07/2017 22:43:08|Info   ][[BoxHeuristics:BoxNoneHeuristic]]
[12/07/2017 22:43:08|Off    ][[TotalTime(MS):9.0006]]
[12/07/2017 22:43:08|Off    ][[Expended:347]]
[12/07/2017 22:43:08|Off    ][[Generated:346]]
[12/07/2017 22:43:08|Off    ][[Pruned:0]]
[12/07/2017 22:43:08|Off    ][[G-Value:14]]
[12/07/2017 22:43:08|Off    ][[GoalBits:00000-00001-00011-00111-01111-01110-01100-11100-11101-11001-11011-11010-10010-10110]]
[12/07/2017 22:43:08|Off    ][[Goal:0-1-3-7-15-14-12-28-29-25-27-26-18-22]]
[12/07/2017 22:43:08|Info   ][[S0:0]]

MaSiB problem=box-od s0=0 s1=127 alg=dfbnb dim=7 snakeSpread=2 boxSpread=3
[12/07/2017 22:44:33|Off    ]Log started.
[12/07/2017 22:44:33|Info   ]Solviong snakes in the box problem:
[12/07/2017 22:44:33|Info   ][[Algorithm:DfBnbMax]]
[12/07/2017 22:44:33|Info   ][[Problem:box-od]]
[12/07/2017 22:44:33|Info   ][[WorldDimentions:7]]
[12/07/2017 22:44:33|Info   ][[SnakeSpread:2]]
[12/07/2017 22:44:33|Info   ][[BoxSpread:3]]
[12/07/2017 22:44:33|Info   ][[SnakeHeuristics:SnakeNoneHeuristic]]
[12/07/2017 22:44:33|Info   ][[BoxHeuristics:BoxNoneHeuristic]]
[12/07/2017 22:44:34|Off    ][[TotalTime(MS):283.0162]]
[12/07/2017 22:44:34|Off    ][[Expended:139238]]
[12/07/2017 22:44:34|Off    ][[Generated:139237]]
[12/07/2017 22:44:34|Off    ][[Pruned:0]]
[12/07/2017 22:44:34|Off    ][[G-Value:30]]
[12/07/2017 22:44:34|Off    ][[GoalBits:|0000000-0000001-0000011-0000111-0001111-0001101-0001100-0011100-0010100-0110100-0100100-0100101-1100101-1000101-1000100|1111111-1111110-1101110-1101010-0101010-0111010-0111011-0111001-1111001-1011001-1011011-1011010-1010010-1110010-1110011|]]
[12/07/2017 22:44:34|Off    ][[Goal:|0-1-3-7-15-13-12-28-20-52-36-37-101-69-68|127-126-110-106-42-58-59-57-121-89-91-90-82-114-115|]]
[12/07/2017 22:44:34|Info   ][[S0:0]]
[12/07/2017 22:44:34|Info   ][[S1:127]]

```


Ref:

'Finding Optimal Solutions to Cooperative Pathfinding Problems.' by Trevor Standley, 2010 - https://pdfs.semanticscholar.org/2529/f40c4f79ef24165dbb1a8327770e37cced2d.pdf

'Solving the Snake in the Box Problem with Heuristic Search - First Results' by Palombo et al. - http://www.bgu.ac.il/~felner/2015/SoCS/solving-snake-box.pdf

Have fun!
