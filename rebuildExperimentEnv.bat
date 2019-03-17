rem Define VISUAL_STUDIO_PATH_DEVENV env. variable in your system, example:
rem SET VISUAL_STUDIO_PATH_DEVENV=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe

"%VISUAL_STUDIO_PATH_DEVENV%" HeuristicSearchMax.sln /rebuild Release 
rmdir /S /Q ExperimentEnv
mkdir ExperimentEnv
copy Grid\bin\Release\*.exe ExperimentEnv
copy Grid\bin\Release\*.config ExperimentEnv
copy Grid\bin\Release\*.dll ExperimentEnv
copy ExperimentRunner\bin\Release\*.exe ExperimentEnv
copy ExperimentRunner\bin\Release\*.config ExperimentEnv
copy ExperimentRunner\profile*.txt ExperimentEnv
copy LogUtils\ExperimentSummarizer\bin\Release\*.exe ExperimentEnv
copy LogUtils\DrawSolution\bin\Release\*.exe ExperimentEnv
copy FoldersExp.bat ExperimentEnv
copy SumFolders.bat ExperimentEnv
