rem Define VISUAL_STUDIO_PATH_DEVENV env. variable in your system, example:
rem SET VISUAL_STUDIO_PATH_DEVENV=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe

"%VISUAL_STUDIO_PATH_DEVENV%" HeuristicSearchMax.sln /rebuild Debug 
rmdir /S /Q ExperimentEnv
mkdir ExperimentEnv
copy Grid\bin\Debug\*.exe ExperimentEnv
copy Grid\bin\Debug\*.config ExperimentEnv
copy Grid\bin\Debug\*.dll ExperimentEnv
copy ExperimentRunner\bin\Debug\*.exe ExperimentEnv
copy ExperimentRunner\bin\Debug\*.config ExperimentEnv
copy ExperimentRunner\profile.txt ExperimentEnv
copy LogUtils\ExperimentSummarizer\bin\Debug\*.exe ExperimentEnv
copy LogUtils\DrawSolution\bin\Debug\*.exe ExperimentEnv
copy FoldersExp.bat ExperimentEnv
