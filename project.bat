set PROJECT=DiffLib
set GITBRANCH=
for /f %%f in ('git rev-parse --abbrev-ref HEAD') do set GITBRANCH=%%f

if "%GITBRANCH%" == "master" (
    set SUFFIX=
    set CONFIGURATION=Release
    echo Building RELEASE build
    exit /B 0
)

echo Building BETA build
set SUFFIX=-beta
set CONFIGURATION=Debug
