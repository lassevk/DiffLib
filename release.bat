@echo off

setlocal

for /f "tokens=*" %%i in ('which msbuild.exe') do set MSBUILD_CONSOLE=%%i
for /f "tokens=*" %%i in ('which nunit3-console.exe') do set NUNIT_CONSOLE=%%i
for /f "tokens=*" %%i in ('which dotcover.exe') do set DOTCOVER_CONSOLE=%%i
for /f "tokens=*" %%i in ('which nuget.exe') do set NUGET_CONSOLE=%%i
for /f "tokens=*" %%i in ('which git.exe') do set GIT_CONSOLE=%%i

if "%MSBUILD_CONSOLE%" == "" goto NO_MSBUILD
if "%DOTCOVER_CONSOLE%" == "" goto NO_DOTCOVER
if "%NUNIT_CONSOLE%" == "" goto NO_NUNIT
if "%NUGET_CONSOLE%" == "" goto NO_NUGET
if "%GIT_CONSOLE%" == "" goto NO_GIT

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

for /d %%f in (*.*) do (
    if exist "%%f\bin" rd /s /q "%%f\bin"
    if errorlevel 1 goto error
    if exist "%%f\obj" rd /s /q "%%f\obj"
    if errorlevel 1 goto error
)
if errorlevel 1 goto error

"%NUGET_CONSOLE%" restore
if errorlevel 1 goto error

set VERSION=%year%.%month%.%day%.%tm%
"%MSBUILD_CONSOLE%" "%PROJECT%.sln" /target:Clean,Rebuild /p:Configuration=%CONFIGURATION% /p:Version=%VERSION%%SUFFIX% /p:AssemblyVersion=%VERSION% /p:FileVersion=%VERSION% /p:DefineConstants="%CONFIGURATION%;USE_RELEASE_KEY"
if errorlevel 1 goto error

set TESTDLL=%CD%\%PROJECT%.Tests\bin\%CONFIGURATION%\%PROJECT%.Tests.dll
if exist "%TESTDLL%" "%DOTCOVER_CONSOLE%" analyze /Output="%PROJECT%-CodeCoverage.html" /ReportType="HTML" /TargetExecutable="%NUNIT_CONSOLE%" /TargetArguments="%TESTDLL% --work=""%CD%"""
if errorlevel 1 goto error

copy %PROJECT%\bin\%CONFIGURATION%\%PROJECT%*.nupkg .\
if errorlevel 1 goto error

"%GIT_CONSOLE%" checkout "%PROJECT%\Lasse V. Karlsen.snk"

echo=
echo================================================
set /P PUSHYESNO=Push package to nuget? [y/N]
if "%PUSHYESNO%" == "Y" GOTO PUSH
if "%PUSHYESNO%" == "y" GOTO PUSH
exit /B 0

:PUSH
"%NUGET_CONSOLE%" push %PROJECT%.%VERSION%%SUFFIX%.nupkg -Source https://www.nuget.org/api/v2/package
if errorlevel 1 goto error
"%GIT_CONSOLE%" tag version/%VERSION%%SUFFIX%
if errorlevel 1 goto error
start "" "https://www.nuget.org/packages/%PROJECT%/"
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

:NO_GIT
echo Unable to locate "git.exe", is it on the path?
goto error

:error
goto exitwitherror

:setup
echo Requires SIGNINGKEYS environment variable to be set
goto exitwitherror

:exitwitherror
"%GIT_CONSOLE%" checkout "%PROJECT%\Lasse V. Karlsen.snk"
exit /B 1
