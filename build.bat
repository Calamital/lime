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
    echo Beginning to compile '%ToBuild%'
    dotnet run --project "./src/" --no-build -- "%ToBuild%" "clean"
) else (
    echo Cannot find %ToBuild% to compile.
    exit
)

echo Completed
