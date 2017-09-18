#!/bin/bash
. $(dirname $0)/lib/logging

# Script adapted from: http://stackoverflow.com/questions/36897334/export-mwb-to-working-sql-file-using-command-line

# export diagram stored in mwb file as a PNG image
# usage: sh mwb2sql.sh {mwb file} {output file}
# prepare: set env MYSQL_WORKBENCH

if [ "$MYSQL_WORKBENCH" = "" ]; then
  export MYSQL_WORKBENCH="/usr/bin/mysql-workbench"
fi

export INPUT=$(cd $(dirname $1);pwd)/$(basename $1)
export OUTPUT=$(cd $(dirname $2);pwd)/$(basename $2)


"$MYSQL_WORKBENCH" \
  --model $INPUT \
  --run-python "
import os
import grt
from grt.modules import Workbench as wb

#wb.input('test')

# This should work (it does if I enable the line above) but it fails because it gets to 
# the line calling wb.exportPNG() before the document/model is fully loaded.
# We need to find a way to wait for the GRNModelOpened notification before exporting to PNG.

wb.goToMarker(\"1\")
wb.exportPNG(os.getenv('OUTPUT'))

" \
  --quit-when-done
set -e
