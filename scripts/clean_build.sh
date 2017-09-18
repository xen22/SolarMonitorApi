#!/bin/bash
. $(dirname $0)/lib/logging

ROOT_DIR=$(echo $(pwd)/$(dirname $0)/..)
OUTPUT_DIR="$ROOT_DIR/_output"
SOURCE_PATHS="src test tools"


loginfo "Deleting output directory: $OUTPUT_DIR"
rm -rf $OUTPUT_DIR
for dir in $SOURCE_PATHS; do
  loginfo "Cleaning binaries from source directory \"$dir\""
  for binary_file in `/usr/bin/find $dir -type f \( -name SolarMonitor.*.dll -o -name SolarMonitor.*.exe \)` ; do 
     logdbg "Deleting file \"$binary_file\"" 
     rm $binary_file
  done

  loginfo "Cleaning project lock files from source directory \"$dir\""
  for proj_lock_file in `/usr/bin/find $dir -type f \( -name *.lock.json \)` ; do
     logdbg "Deleting file \"$proj_lock_file\"" 
     rm $proj_lock_file
  done
done
