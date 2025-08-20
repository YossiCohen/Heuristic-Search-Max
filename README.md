# Heuristic Search for Maximization Problems
This solution (HeuristicSearchMax.sln) is compund of multiple projects. They all cover two main domains GRID & MASIB. 
In Grid we search for the longest simple path on 2D grid.
in MASIB we solve the Multi-Agent version variant of Snake in the Box problem.

Projects structure:

https://deepwiki.com/YossiCohen/Heuristic-Search-Max/4-masib-domain-system

MaxSearchAlg - Implementation of search algorithms (A*, DFBnB, Greedy)


Grid related:
- Grid - Longest simple path on 2D grid code
- GridGenerator - tool to generate varaity of grid problems
- ExperimentRunner - tool to run experiments with multiple configuration (algorithm/pruning/heuristic/init-bcc)
- LogUtils\DrawSolution - given an experiment result (in RunningLogs folder) this tool will draw the solution on the grid
- LogUtils\ExperimentSummarizer - given an experiment results files (RunningLogs folder) this tool will create a collection of CSV files that summarize the results in different perspectives
- LogUtils\LogClean - given a verbose log, this tool will remove various lines to make it more readable (for debugging purpose)

Masib related:
- MaSib - Multi agent snake in the box code
- Lab - Python environment that runs multiple MASIB experiments

Test projects:
- CommotTest
- GridTest
- MasibTest

Getting started with grid:
1. generate some problems with the **GridGenerator** project
2. run the batch file: **rebuildExperimentEnv.bat** (maybe you will need to modify paths in it) it will create the folder: **ExperimentEnv**
3. create a folder(s) in it and put the problems you want to solve in that folder(s) - you can create a different folder for every problem set
4. edit **Profile.txt** and define what alg/pruning/heuristics you want to use
5. edit **ExperimentRunner.exe.config** and **Grid.exe.config** - set default values such as time limitation memory limitations etc...
6. run **FoldersExp.bat** it will solve all the problems in the different folders
7. run **SumFolders.bat** it will summarize the results in each folder saparatly and in **ALL_EXP** folder for all experiments united

Getting started with masib: see separate readme in the **MaSib** folder


### Grid Generator
running GridGenerator without arguments will display the following information
```
Grid problems generator
-=-=-=-=-=-=-=-=-=-=-=-
Please Provide arguments to run:
all args should be in the form of: [key]=[value] with space between them
Arguments:
----------
type:                    [basic/rooms/alternate] (mandatory) all other arguments that
                         start with type name are mandatory for that type
num:                     number of problems to generate (default=1)
retries:                 number of retries before stop generation of grid (default=1000)
one-bcc:                 [true/false] one bcc in initial state, Not relevant for Rooms (default=false)
- - - - Type specific args:  All mandatory per type
basic-blocked:           number of blocked locations
basic-width:             number - basic size
basic-hight:             number - basic size
basic-corners:           [true/false] if true start & goal will be on the top left and bottom right
                         corners, otherwise they will be random
- - - -
rooms-num-x:             number of rooms in the X axis (width)
rooms-num-y:             number of rooms in the Y axis (hight)
rooms-size-x:            room size in the X axis (width)
rooms-size-y:            room size in the Y axis (hight)
rooms-door-count-x:      number of doors on the X walls (width)
rooms-door-count-y:      number of doors on the Y walls (hight)
rooms-door-open-prob:    probability for door to be open
rooms-barier-prob:       probability for blocked location inside a room
- - - -
alternate-width:         number - grid size
alternate-hight:         number - grid size
alternate-blocked-odd:   number of blocked odd locations
alternate-blocked-even:  number of blocked even locations
alternate-corners:       [true/false] if true start & goal will be on the top left and bottom right
                         corners, otherwise they will be random
----------
Examples:
Generate 10 basic maps:
GridGenerator type=basic basic-width=7 basic-hight=5 basic-blocked=9 basic-corners=true num=10
Generate 5 alternate maps:
GridGenerator type=alternate alternate-width=5 alternate-hight=5 alternate-blocked-odd=1 alternate-blocked-even=3 alternate-corners=true one-bcc=true num=5
Generate 3 Rooms maps:
GridGenerator type=rooms rooms-num-x=2 rooms-num-y=2 rooms-size-x=2 rooms-size-y=2 rooms-door-count-x=1 rooms-door-count-y=1 rooms-door-open-prob=0.9 rooms-barier-prob=0.3 num=3
-----------------------------[Version:1.1]---------------------------------
```
### Grid Solver
running Grid solver (Grid.exe) without arguments will display the following information
```
Please Provide arguments to run:
all args should be in the form of: [key]=[value] with space between them
Arguments:
----------
problem:     problem filename
time-limit:  limit run time to X minutes (default 120), 0 for no time limit
alg:         [astar/dfbnb/greedy/greedyloops] the solving algorithm
heuristic:   [none/untouched/bcc/alternate/altbcc/sepaltbcc] the heuristic being used
prune:       [none/bsd/rsd/hbsd] pruning technique
bcc-init:    [true/false] remove non-reachable areas from the graph on init
----------
memTest:     if set to true, will not solve nothing, only fill memory
             allocation to check 64bit issue
-----------------------------[Version:2.01]---------------------------------
```

Have fun!
