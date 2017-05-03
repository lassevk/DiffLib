@echo off

setlocal

for /f "tokens=*" %%i in ('which msbuild.exe') do set MSBUILD_CONSOLE=%%i
for /f "tokens=*" %%i in ('which nunit3-console.exe') do set NUNIT_CONSOLE=%%i
for /f "tokens=*" %%i in ('which dotcover.exe') do set DOTCOVER_CONSOLE=%%i
for /f "tokens=*" %%i in ('which nuget.exe') do set NUGET_CONSOLE=%%i

if "%MSBUILD_CONSOLE%" == "" goto NO_MSBUILD
if "%DOTCOVER_CONSOLE%" == "" goto NO_DOTCOVER
if "%NUNIT_CONSOLE%" == "" goto NO_NUNIT
if "%NUGET_CONSOLE%" == "" goto NO_NUGET


call project.bat
if not "%1" == "" set CONFIGURATION=%1

for /d %%f in (*.*) do (
    if exist "%%f\bin" rd /s /q "%%f\bin"
    if errorlevel 1 goto error
    if exist "%%f\obj" rd /s /q "%%f\obj"
    if errorlevel 1 goto error
)
if errorlevel 1 goto error

"%NUGET_CONSOLE%" restore "%PROJECT%.sln"
if errorlevel 1 goto error

"%MSBUILD_CONSOLE%" "%PROJECT%.sln" /target:Clean,Rebuild /p:Configuration=%CONFIGURATION%
if errorlevel 1 goto error

set TESTDLL=%CD%\%PROJECT%.Tests\bin\%CONFIGURATION%\%PROJECT%.Tests.dll
if exist "%TESTDLL%" "%DOTCOVER_CONSOLE%" analyze /Output="%PROJECT%-CodeCoverage.html" /ReportType="HTML" /TargetExecutable="%NUNIT_CONSOLE%" /TargetArguments="%TESTDLL% --work=""%CD%"""
if errorlevel 1 goto error

exit /B 0

:NO_NUGET
echo Unable to locate 'nuget.exe', is it on the path?
goto error

:NO_DOTCOVER
echo Unable to locate 'dotcover.exe', is it on the path?
goto error

:NO_MSBUILD
echo Unable to locate 'msbuild.exe', is it on the path?
goto error

:NO_NUNIT
echo Unable to locate "nunit3-console.exe", is it on the path?
goto error

:error
exit /B 1
