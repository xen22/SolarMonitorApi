#!/bin/bash
. $(dirname $0)/lib/logging

loginfo "Running integration tests..."

for dir in `/usr/bin/find test/integration -mindepth 1 -maxdepth 1 -type d` ; do 
  loginfo "Running unit tests in: $dir" 
  cd $dir
  dotnet test
  cd -
done
loginfo "Done."
