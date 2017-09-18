#!/bin/bash
. $(dirname $0)/lib/logging

if [[ $# -lt 1 ]]; then
  echo "Usage: $(basename $0) <ASSEMBLY_FILENAME>"
  exit 1
fi

ASSEMBLY=$1

if [[ ! -f $ASSEMBLY ]]; then
  logerr "file does not exit, $ASSEMBLY"
  exit 1
fi

ver=$(/usr/bin/monodis --assembly $ASSEMBLY | grep Version | sed s/Version:\\s*//)
logdbg "$ver"
