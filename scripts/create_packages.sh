#!/bin/bash
. $(dirname $0)/lib/logging

BUILD_CONFIG="Debug"
BUILD_VERSION="UnknownVersion"
PLATFORM="UnknownPlatform"

PROJECT_NAMES="SolarMonitor.Simulator"
ROOT_DIR=$(echo $(pwd)/$(dirname $0)/..)
OUTPUT_DIR="$ROOT_DIR/_output"

if [[ $# -ne 3 ]]; then
  loginfo "Usage: $(basename $0) <BUILD_CONFIG> <BUILD_VERSION> <PLATFORM>"
  exit 1
else
  BUILD_CONFIG=$1
  BUILD_VERSION=$2
  PLATFORM=$3
fi

mkdir -p $OUTPUT_DIR
for proj in $PROJECT_NAMES ; do
  proj_dir=$(/usr/bin/find $ROOT_DIR -type d -name $proj)
  loginfo "Creating archive for project $proj found at \"$proj_dir\"."
  #zip --junk-paths --temp-path $OUTPUT_DIR "$OUTPUT_DIR/${proj}-${BUILD_VERSION}-${BUILD_CONFIG}.zip" \
  #    $proj_dir/bin/$BUILD_CONFIG/netcoreapp1.0/*.dll

  filename=${proj}-${BUILD_VERSION}-${BUILD_CONFIG}-${PLATFORM}
  NETCOREAPP_VERSION=`cat ${ROOT_DIR}/NETCOREAPP_VERSION`
  cd $proj_dir/bin/$BUILD_CONFIG/netcoreapp${NETCOREAPP_VERSION}/
  new_dir=$filename
  mkdir ${new_dir}
  cp *.dll *.deps.json *.runtimeconfig.json ${new_dir}
  tar zcf "$OUTPUT_DIR/$filename.tar.gz" ${new_dir}
  rm -rf ${new_dir}
  cd - 
done
