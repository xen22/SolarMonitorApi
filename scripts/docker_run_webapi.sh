#!/bin/bash
. $(dirname $0)/lib/logging

USAGE="$(basename $0) <version>"

if [[ $# -lt 1 ]]; then
  echo $USAGE
  exit 1
fi

version=$1

addrN2=$(echo ${version} | cut -d'.' -f 2)
addrN4=$(echo ${version} | cut -d'.' -f 4)

testSubnet=172.18.${addrN2}.0
testSubnetName=testnet3
apiIpAddr=172.18.${addrN2}.${addrN4}

loginfo "Creating network ${testSubnet}/24 ${testSubnetName}"
docker network create --subnet=${testSubnet}/24 ${testSubnetName}

docker network ls

loginfo "Opening new browser window."
/usr/bin/google-chrome --new-window http://${apiIpAddr}/api/SolarMonitor/version

loginfo "Running docker container webapi-${version}"
docker run -it --rm --net ${testSubnetName} --ip ${apiIpAddr} solarmonitor/webapi-${version}

loginfo "Destroying network ${testSubnetName}"
docker network rm ${testSubnetName}
