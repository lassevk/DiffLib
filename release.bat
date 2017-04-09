@echo off

setlocal

call project.bat

if exist *.nupkg del *.nupkg
if errorlevel 1 goto error

if "%SIGNINGKEYS%" == "" goto setup

set /A year=%date:~6,4%
set /A month=%date:~3,2%
set /A day=%date:~0,2%
set /A tm=%time:~0,2%%time:~3,2%

copy "%SIGNINGKEYS%\Lasse V. Karlsen Private.snk" "%PROJECT%\Lasse V. Karlsen.snk"
if errorlevel 1 goto error

if exist %PROJECT%\bin rd /s /q %PROJECT%\bin
if errorlevel 1 goto error

nuget restore
if errorlevel 1 goto error

set VERSION=%year%.%month%.%day%.%tm%
msbuild %PROJECT%\%PROJECT%.csproj /target:Clean,Rebuild /p:Configuration=Release /p:Version=%VERSION%
if errorlevel 1 goto error

copy %PROJECT%\bin\Release\%PROJECT%*.nupkg .\
if errorlevel 1 goto error

git checkout "%PROJECT%\Lasse V. Karlsen.snk"

echo=
echo================================================
set /P PUSHYESNO=Push package to nuget? [y/N]
if "%PUSHYESNO%" == "Y" GOTO PUSH
exit /B 0

:PUSH
nuget push %PROJECT%.%VERSION%.nupkg -Source https://www.nuget.org/api/v2/package
if errorlevel 1 goto error
git tag version/%VERSION%
if errorlevel 1 goto error
exit /B 0

:error
goto exitwitherror

:setup
echo Requires SIGNINGKEYS environment variable to be set
goto exitwitherror

:exitwitherror
git checkout "%PROJECT%\Lasse V. Karlsen.snk"
exit /B 1