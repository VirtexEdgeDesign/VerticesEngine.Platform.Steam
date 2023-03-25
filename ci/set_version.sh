#!/bin/bash 
echo "Set Assembly Version"
VERSION=0.2.0
PROJECT_DIR="./src"
CSPROJECT_DLL="VerticesEngine.Platforms.Steam.csproj"
#DOX_PATH="./docs/Doxyfile"


echo "Setting Assembly and Nuget Pacakge versions to: "$VERSION.$@

# Set DLL Version
sed -E -i "s/0.1.0/$VERSION.$@/g" $PROJECT_DIR/$CSPROJECT_DLL

# Set Doc Version
#sed -E -i "s/v1.0.0/v$VERSION.$@/g" ${DOX_PATH}

# Set Assembly Version
#sed -E -i "s/$VERSION.999/$VERSION.$@/g" "${PROJECT_DIR}/Properties/AssemblyInfo.cs"


echo "Done"