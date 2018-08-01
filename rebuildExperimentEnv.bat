SET Path=C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE;%Path%
devenv HeuristicSearchMax.sln /rebuild Release 
rmdir /S /Q ExperimentEnv
mkdir ExperimentEnv
copy Grid\bin\Release\*.exe ExperimentEnv
copy Grid\bin\Release\*.config ExperimentEnv
copy Grid\bin\Release\*.dll ExperimentEnv
copy ExperimentRunner\bin\Release\*.exe ExperimentEnv
copy ExperimentRunner\bin\Release\*.config ExperimentEnv
copy ExperimentRunner\profile.txt ExperimentEnv
