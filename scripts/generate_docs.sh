#!/bin/bash
. $(dirname $0)/lib/logging

loginfo "Generating docs with docfx"
docfx doc/docfx_project/docfx.json --serve --port 9090
