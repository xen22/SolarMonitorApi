#!/bin/bash
. $(dirname $0)/lib/logging

# Script adapted from: http://stackoverflow.com/questions/36897334/export-mwb-to-working-sql-file-using-command-line

# export diagram stored in mwb file as a PDF file
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
from grt.modules import WbPrinting as printing
d = grt.root.wb.doc.physicalModels[0].diagrams[0]
printing.printToPDFFile(d, os.getenv('OUTPUT'))" \
  --quit-when-done
set -e
