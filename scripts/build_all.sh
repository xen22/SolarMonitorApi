#!/bin/bash
. $(dirname $0)/lib/logging

#ROOT_DIR=$(echo $(pwd)/$(dirname $0)/..)
ROOT_DIR="$( cd "$(dirname "$0")/.." ; pwd -P )"

loginfo "Checking dotnet command."
echo "dotnet location: $(ls -l `which dotnet`)"
echo "dotnet version: $(dotnet --version)"

loginfo "Restoring dependencies..."
dotnet restore ${ROOT_DIR}/SolarMonitor.sln

loginfo "Building all projects..."
global_return_code=0
dotnet build ${ROOT_DIR}/SolarMonitor.sln
rc=$? 
if [[ $rc != 0 ]]; then 
  global_return_code=$rc
  logerr "Solution failed to build." 
else
  loginfo "Solution built successfully." 
fi

# global_return_code=0
# for dir in `/usr/bin/find src test/unit test/integration tools -mindepth 1 -maxdepth 1 -type d` ; do
#   if [ -f $dir/project.json ]; then 
#     loginfo "Building project in: $dir" 
#     dotnet build $dir
#     rc=$? 
#     if [[ $rc != 0 ]]; then 
#       global_return_code=$rc
#       logerr "Project $(basename $dir) failed to build." 
#     else
#       loginfo "Project $(basename $dir) built successfully." 
#     fi
#   else
#     loginfo "Skipping directory $(basename $dir) since no project.json file found."
#   fi
# done

SOURCE_DIR=${ROOT_DIR}/src/SolarMonitor.Web.Api
BUILD_DIR=${ROOT_DIR}/src/SolarMonitor.Web.Api/bin/Debug/netcoreapp2.0/
PUBLISH_DIR=${BUILD_DIR}/publish

cp ${SOURCE_DIR}/appsettings.*.json ${BUILD_DIR}
rm -rf ${PUBLISH_DIR}
pushd ${SOURCE_DIR}
dotnet publish
popd
cp ${SOURCE_DIR}/appsettings.*.json ${PUBLISH_DIR}


loginfo "Done."
exit $global_return_code
