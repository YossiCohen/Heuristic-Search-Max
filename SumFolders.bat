@echo off
echo Summerizing all the results in the folders

IF not exist %~dp0ExpSum.exe (
echo ExpSum.exe is missing - I cant work like that!
goto exit
)

IF EXIST %~dp0ALL_EXP (
echo Folder ALL_EXP already exists - Halting - Check what's in it or remove it if you want this batch to work
goto exit
) ELSE (
mkdir %~dp0ALL_EXP
)
for /d %%d in (*.*) do (
    echo Folder: %%d
	IF "%%d" EQU "ALL_EXP" (
		echo Skipping All_EXP Folder
	) else (
		copy ExpSum.exe %%d\RunningLogs
		cd %%d\RunningLogs
		ExpSum.exe
		copy *.txt ..\..\ALL_EXP
		del ExpSum.exe
		cd ..\..
	)
)
copy ExpSum.exe ALL_EXP
cd ALL_EXP
ExpSum.exe
del ExpSum.exe
cd ..
:exit