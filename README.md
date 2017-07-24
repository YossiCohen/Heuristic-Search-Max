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
boxH:        [none/snakes-sum/legal/reachable] the box heuristic
alg:         [astar/dfbnb] the solving algorithm
dim:         the number of dimentions for the problem (N)
snakeSpread: the intra-snake spread (sK)
boxSpread:   the inter-snake spread (bK)
timeLimit:   limit run time to X minutes (default 120), 0 for no time limit
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
[24/07/2017 10:24:28|Off    ]Log started.
[24/07/2017 10:24:28|Warning]boxSpread not found, setting it to:2
[24/07/2017 10:24:28|Info   ]Solviong snakes in the box problem:
[24/07/2017 10:24:28|Info   ][[Algorithm:AStarMax]]
[24/07/2017 10:24:28|Info   ][[Problem:snake]]
[24/07/2017 10:24:28|Info   ][[WorldDimentions:5]]
[24/07/2017 10:24:28|Info   ][[SnakeSpread:2]]
[24/07/2017 10:24:28|Info   ][[BoxSpread:2]]
[24/07/2017 10:24:28|Info   ][[SnakeHeuristics:SnakeNoneHeuristic]]
[24/07/2017 10:24:28|Info   ][[BoxHeuristics:BoxNoneHeuristic]]
[24/07/2017 10:24:28|Off    ][[TotalTime(MS):10.0006]]
[24/07/2017 10:24:28|Off    ][[Expended:347]]
[24/07/2017 10:24:28|Off    ][[Generated:346]]
[24/07/2017 10:24:28|Off    ][[Pruned:0]]
[24/07/2017 10:24:28|Off    ][[G-Value:13]]
[24/07/2017 10:24:28|Off    ][[GoalBits:00000-00001-00011-00111-01111-01110-01100-11100-11101-11001-11011-11010-10010-10110]]
[24/07/2017 10:24:28|Off    ][[Goal:0-1-3-7-15-14-12-28-29-25-27-26-18-22]]
[24/07/2017 10:24:28|Off    ][[HowEnded:Ended]]
[24/07/2017 10:24:28|Off    ][[SnakeSpreadFreeSpotsCount:0]]
[24/07/2017 10:24:28|Off    ][[SnakeSpreadFreeSpotsPlaces:]]
[24/07/2017 10:24:28|Info   ][[S0:0]]

MaSiB problem=box-od s0=0 s1=127 alg=dfbnb dim=7 snakeSpread=2 boxSpread=3
[24/07/2017 10:25:51|Off    ]Log started.
[24/07/2017 10:25:51|Info   ]Solviong snakes in the box problem:
[24/07/2017 10:25:51|Info   ][[Algorithm:DfBnbMax]]
[24/07/2017 10:25:51|Info   ][[Problem:box-od]]
[24/07/2017 10:25:51|Info   ][[WorldDimentions:7]]
[24/07/2017 10:25:51|Info   ][[SnakeSpread:2]]
[24/07/2017 10:25:51|Info   ][[BoxSpread:3]]
[24/07/2017 10:25:51|Info   ][[SnakeHeuristics:SnakeReachableHeuristic]]
[24/07/2017 10:25:51|Info   ][[BoxHeuristics:BoxSnakesSumHeuristic]]
[24/07/2017 10:26:04|Off    ][[TotalTime(MS):12922.7392]]
[24/07/2017 10:26:04|Off    ][[Expended:138434]]
[24/07/2017 10:26:04|Off    ][[Generated:139237]]
[24/07/2017 10:26:04|Off    ][[Pruned:804]]
[24/07/2017 10:26:04|Off    ][[G-Value:28]]
[24/07/2017 10:26:04|Off    ][[GoalBits:|0000000-0000001-0000011-0000111-0001111-0001101-0001100-0011100-0010100-0110100-0100100-0100101-1100101-1000101-1000100|1111111-1111110-1101110-1101010-0101010-0111010-0111011-0111001-1111001-1011001-1011011-1011010-1010010-1110010-1110011|]]
[24/07/2017 10:26:04|Off    ][[Goal:|0-1-3-7-15-13-12-28-20-52-36-37-101-69-68|127-126-110-106-42-58-59-57-121-89-91-90-82-114-115|]]
[24/07/2017 10:26:04|Off    ][[HowEnded:Ended]]
[24/07/2017 10:26:04|Off    ][[SnakeSpreadFreeSpotsCount:4]]
[24/07/2017 10:26:04|Off    ][[SnakeSpreadFreeSpotsPlaces:55-72-87-96]]
[24/07/2017 10:26:04|Off    ][[BoxSpreadFreeSpotsCount:0]]
[24/07/2017 10:26:04|Off    ][[BoxSpreadFreeSpotsPlaces:]]
[24/07/2017 10:26:04|Info   ][[S0:0]]
[24/07/2017 10:26:04|Info   ][[S1:127]]

```

you can validate your results at:
https://yossicohen.github.io/MA-SIB/


*Lab:*

Python project that runs MASIB with many different combination of arguments - this is just a simple example, feel free to modify and create your own experiment

if you want to summerize multiple runs into single CSV file just run ExpSum.exe in the outputs folder (the source for this EXE is in the project ExperimentSummarizer)

Ref:

'Finding Optimal Solutions to Cooperative Pathfinding Problems.' by Trevor Standley, 2010 - https://pdfs.semanticscholar.org/2529/f40c4f79ef24165dbb1a8327770e37cced2d.pdf

'Solving the Snake in the Box Problem with Heuristic Search - First Results' by Palombo et al. - http://www.bgu.ac.il/~felner/2015/SoCS/solving-snake-box.pdf

Have fun!
