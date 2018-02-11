@echo off
echo Solving all *.grd with .NET x64 implementation 
for %%f in (*.grd) do (
	echo Solving %%~nf
	type "%%~nf.grd"
	echo Running AStar - No Prunning...
	Grid.exe problem=%%~nf.grd alg=astar prune=none
	echo Running DFBNB - No Prunning...
	Grid.exe problem=%%~nf.grd alg=dfbnb prune=none
	echo Running AStar - BasicSymmetryDetection Prunning...
	Grid.exe problem=%%~nf.grd alg=astar prune=bsd
	echo Running DFBNB - BasicSymmetryDetection Prunning...
	Grid.exe problem=%%~nf.grd alg=dfbnb prune=bsd
	echo Running AStar - ReachableSymmetryDetection Prunning...
	Grid.exe problem=%%~nf.grd alg=astar prune=rsd
	echo -----------------Next GRiD...
)
rem for %%f in (*.text) do (
rem 	echo DrawSolution %%~nf
rem 	DrawSolution.exe "%%~nf.text"
rem )
copy ExpSum.exe RunningLogs
copy LogClean.exe RunningLogs
copy DrawSolution.exe RunningLogs
cd RunningLogs
ExpSum
LogClean
DrawSolution
cd ..
