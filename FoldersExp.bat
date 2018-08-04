@echo off
for /d %%d in (*.*) do (
    echo Folder: %%d
	copy . %%d
	cd %%d
	ExperimentRunner.exe
	cd ..
)