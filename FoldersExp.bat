@echo off
for /d %%d in (*.*) do (
    echo Folder: %%d
	copy *.exe %%d
	copy *.config %%d
	copy *.dll %%d
	copy profile.txt %%d
	cd %%d
	ExperimentRunner.exe
	del *.exe
	del *.config
	del *.dll
	del profile.txt
	cd ..
)