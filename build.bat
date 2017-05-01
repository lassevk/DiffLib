@echo off

setlocal

set CONFIGURATION=Debug
if not "%1" == "" set CONFIGURATION=%1

call project.bat

if exist %PROJECT%\bin rd /s /q %PROJECT%\bin
if errorlevel 1 goto error

nuget restore
if errorlevel 1 goto error

msbuild %PROJECT%.sln /target:Clean,Rebuild /p:Configuration=%CONFIGURATION%
if errorlevel 1 goto error

set TESTDLL=%PROJECT%.Tests\bin\%CONFIGURATION%\%PROJECT%.Tests.dll
if exist "%TESTDLL%" nunit3-console "%TESTDLL%"
if errorlevel 1 goto error

exit /B 0

:error
exit /B 1
