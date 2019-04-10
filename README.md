# Heuristic Search for Maximization Problems
This solution (HeuristicSearchMax.sln) is compund of multiple projects. They all cover two main domains GRID & MASIB. 
In Grid we search for the longest simple path on 2D grid.
in MASIB we solve the Multi-Agent version variant of Snake in the Box problem.

Projects structure:
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

Have fun!
