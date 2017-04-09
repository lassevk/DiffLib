@echo off

setlocal

if exist DiffLib\bin rd /s /q DiffLib\bin
if errorlevel 1 goto error

nuget restore
if errorlevel 1 goto error

msbuild DiffLib\DiffLib.csproj /target:Clean,Rebuild /p:Configuration=Debug
if errorlevel 1 goto error

exit /B 0

:error
exit /B 1
