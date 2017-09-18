#!/bin/bash
. $(dirname $0)/lib/logging

CONTAINER_NAME=docker-build-slave-1

docker exec -ti $CONTAINER_NAME /bin/bash
