#!/bin/bash
. $(dirname $0)/lib/logging

#ROOT_DIR=$(realpath $(pwd)/$(dirname $0)/..)
ROOT_DIR="$( cd "$(dirname "$0")/.." ; pwd -P )"
ENV="production"

if [[ $# > 0 ]]; then
  ENV=$1
fi

BASE_DIR=${ROOT_DIR}/src/SolarMonitor.Web.Api
PUBLISH_DIR=${BASE_DIR}/bin/Debug/netcoreapp1.1/publish

# publish first
loginfo "Publishing Web API..."
pushd ${BASE_DIR}
dotnet publish
popd

loginfo "Copying configuration files manually..."
cp ${BASE_DIR}/appsettings.*.json ${PUBLISH_DIR}

# upload to linode
loginfo "Syncing publish dir with linode..."
rsync -avz --progress ${PUBLISH_DIR}/* linode:webapi/test

loginfo "Restarting kestrel-solarmonitor service on linode..."
ssh linode sudo systemctl restart kestrel-solarmonitor.service
