@echo off

setlocal

set PROJECT=DiffLib

if exist %PROJECT%\bin rd /s /q %PROJECT%\bin
if errorlevel 1 goto error

nuget restore
if errorlevel 1 goto error

msbuild %PROJECT%\%PROJECT%.csproj /target:Clean,Rebuild /p:Configuration=Debug
if errorlevel 1 goto error

exit /B 0

:error
exit /B 1
