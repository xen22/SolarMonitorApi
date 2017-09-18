#!/bin/bash
. $(dirname $0)/lib/logging

# This is a simple script that performs basic "smoke tests" on the output binaries.
# For example, it runs the simulator tool and verifies that it outputs the expected version number.
# Note: this is just a rough draft for now, it will likely be implemented differently.  

BUILD_CONFIG="Debug"
BUILD_VERSION="UnknownVersion"

ROOT_DIR=$(echo $(pwd)/$(dirname $0)/..)
OUTPUT_DIR="$ROOT_DIR/_output"

if [[ $# -ne 2 ]]; then
  echo "Usage: $(basename $0) <BUILD_CONFIG> <BUILD_VERSION>"
  exit 1
else
  BUILD_CONFIG=$1
  BUILD_VERSION=$2
fi

loginfo "Running smoke tests..."

loginfo "[Test]: Validating SolarMonitor.Simulator tool."

NETCOREAPP_VERSION=`cat ${ROOT_DIR}/NETCOREAPP_VERSION`
SIMULATOR_BINARY="./tools/SolarMonitor.Simulator/bin/${BUILD_CONFIG}/netcoreapp${NETCOREAPP_VERSION}/SolarMonitor.Simulator.dll"
if [ ! -f $SIMULATOR_BINARY ]; then
  logerr "cannot find SolarMonitor.Simulator at $SIMULATOR_BINARY"
  exit 1
else
  out=$(dotnet $SIMULATOR_BINARY)
  if [[ $out =~ "${BUILD_VERSION}" ]]; then
    loginfo "Output string ($out) contains expected version string (${BUILD_VERSION})"
    loginfo "Test PASSED."
  else
    logwarn "Output string ($out) does not contain expected version string (${BUILD_VERSION})"
    logwarn "Test FAILED."
    exit 1
  fi
fi

loginfo "Done."
