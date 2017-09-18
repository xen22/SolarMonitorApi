#!/bin/bash
. $(dirname $0)/lib/logging

ROOT_DIR=$(echo $(pwd)/$(dirname $0)/..)

if [[ $# -gt 0 ]]; then
  EXPECTED_VERSION=$1
  loginfo "Checking against expected version $EXPECTED_VERSION"
fi

for assembly in `/usr/bin/find $ROOT_DIR/src $ROOT_DIR/test $ROOT_DIR/tools -name 'SolarMonitor.*.dll' -o -name 'SolarMonitor.*.exe'` ; do 
  loginfo "Processing file: $assembly"
  ver=$(/usr/bin/monodis --assembly $assembly | grep Version | sed s/Version:\\s*//)
  fname=`basename $assembly`
  logdbg "File $fname has version: $ver"
  if [[ $# > 0 && $ver != $EXPECTED_VERSION ]] ; then
    logerr "Assembly \"$fname\" version ($ver) does not match the expected value, $EXPECTED_VERSION"
  fi
done

