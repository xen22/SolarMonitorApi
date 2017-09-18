#!/bin/bash
. $(dirname $0)/lib/logging

ROOT_DIR=$(echo $(pwd)/$(dirname $0)/..)

NEW_VERSION="0.0.0.1"
if [[ $# -lt 1 ]]; then
  echo "Usage: $0 [<new_version>]"
  loginfo "Attempting to retrieve version from the most recent git tag on the current branch."
  NEW_VERSION=$(git tag -l --points-at HEAD | tail -1)
  if [[ $NEW_VERSION == "" ]]; then 
    logerr "Could not retrieve version."
    exit 1
  fi
else
  NEW_VERSION=$1
fi

loginfo "Updating project.json files with new version $NEW_VERSION"

for proj_file in $(/usr/bin/find $ROOT_DIR -type f -name project.json) ; do
  loginfo "Processing project file $proj_file"
  pattern="^\\s*\"version\":.*"
  
  # Note: '--in-place' does not work on Azure (custom pre-build step - see deploy.cmd in the root dir)
  #/bin/sed --in-place=bak "0,/$pattern/s/$pattern/  \"version\": \"$NEW_VERSION\",/" $proj_file

  /bin/sed "0,/$pattern/s/$pattern/  \"version\": \"$NEW_VERSION\",/" $proj_file > $proj_file.new
  cp $proj_file.new $proj_file
  rm -rf $proj_file.new
done

#find $ROOT_DIR -type f -name project.json -print0 | xargs -0 sed -i "s/^\\s*\"version\":.*/  \"version\": \"$NEW_VERSION\",/"

#for f in $(find $ROOT_DIR -name project.json) ; do 
#  ver=$(/usr/bin/monodis --assembly $f | grep Version | sed s/Version:\\s*//)
#  fname=`basename $f`
#  echo "File $fname has version: $ver"
#  if [[ $# > 0 && $ver != $EXPECTED_VERSION ]] ; then
#    echo "Error: Assembly \"$fname\" version ($ver) does not match the expected value, $EXPECTED_VERSION"
#  fi
#done

