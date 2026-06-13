@REM @echo off

SETLOCAL EnableDelayedExpansion

for /f "delims=" %%i in ('powershell -Command "(Get-Content -Raw './buildsettings.json' | ConvertFrom-Json).'buildFile'"') do (
    set "ToBuild=%%i"
)

cd "src"

echo beginning compilation.
dotnet clean
dotnet build

cd ..

if EXIST !ToBuild! (
    echo Cleaning '%ToBuild%'
    dotnet run --project "./src/" --no-build -- "clean" "%ToBuild%"
    echo Building '%ToBuild%'
    dotnet run --project "./src/" --no-build -- "build" "%ToBuild%"
    echo Cleaning '%ToBuild%'
    dotnet run --project "./src/" --no-build -- "Clean" "%ToBuild%"
) else (
    echo Cannot find %ToBuild% to compile.
    exit
)

echo Completed
