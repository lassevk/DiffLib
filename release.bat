rem @echo off

setlocal

if "%SIGNINGKEYS%" == "" goto setup

set year=%date:~6,4%
set month=%date:~3,2%
set day=%date:~0,2%
set tm=%time:~0,2%%time:~3,2%

copy "%SIGNINGKEYS%\Lasse V. Karlsen Private.snk" "DiffLib\Lasse V. Karlsen.snk"
if errorlevel 1 goto error

if exist DiffLib\bin rd /s /q DiffLib\bin
if errorlevel 1 goto error

nuget restore
if errorlevel 1 goto error

msbuild DiffLib\DiffLib.csproj /target:Clean,Rebuild /p:Configuration=Release /p:Version=%year%.%month%.%day%.%tm%
if errorlevel 1 goto error

copy DiffLib\bin\Release\DiffLib*.nupkg .\
if errorlevel 1 goto error

exit /B 0

:error
exit /B 1

:setup
echo Requires SIGNINGKEYS environment variable to be set
exit /B 1
