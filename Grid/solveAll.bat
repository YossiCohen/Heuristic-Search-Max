@echo off
echo Solving all *.grd with .NET x64 implementation 
for %%f in (*.grd) do (
	echo Solving %%~nf
	type "%%~nf.grd"
	echo Running AStar...
	Grid.exe problem=%%~nf.grd alg=astar
	echo Running DFBNB...
	Grid.exe problem=%%~nf.grd alg=dfbnb
	echo -----------------Next GRiD...
)
rem for %%f in (*.text) do (
rem 	echo DrawSolution %%~nf
rem 	DrawSolution.exe "%%~nf.text"
rem )
copy ExpSum.exe RunningLogs
cd RunningLogs
ExpSum
cd ..
