# MA-SIB
Multi Agent Snake In the Box

Solver project for the SIB problem with heuristic search

Implemented in C# you can find here:
Algorithms:
A* & DFBnB - Maximal varient

Snake - single snake
Box - Multiple snakes with cartesian product of all snakes when expending a node
Box-od - Instead of cartesian product, we are using operator decomposition - as offered by Trevor Standley in 2010 paper 'Finding Optimal Solutions to Cooperative Pathfinding Problems.' https://pdfs.semanticscholar.org/2529/f40c4f79ef24165dbb1a8327770e37cced2d.pdf

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

Have fun!
