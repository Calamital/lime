#!/bin/bash

# finds the filepath of the lime file given in buildsettings.json/buildFile
buildFile=$(jq -r '.buildFile' buildsettings.json)

# enters the csproject directory to run dotnet
cd "/workspaces/lime/src/Lime" || exit

echo "interpreting $buildFile..."
# interpreter code goes here

# cleans up the directory
dotnet clean

# builds the interpreter code
dotnet build

# runs the interpreter to create interpreted.cs
# arg0 is the filepath of the lime script and arg1 is whether or not to delete interpreted.cs before interpretation
dotnet run --no-build -- "$buildFile" "clean"

echo "complete!"
echo "executing..."
# execution

# executes the interpreted file using dotnet
# when the interpreter code has been made uncomment the next line
# dotnet run --file interpreted.cs

echo "complete!"
