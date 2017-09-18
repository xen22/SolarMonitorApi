#!/bin/bash

docker build -t jenkins/webapi-base-image ./docker-webapi-base
docker build -t jenkins/unit-test-slave-image ./docker-unit-test-slave
docker build -t jenkins/integration-test-slave-image ./docker-integration-test-slave
docker build -t jenkins/smoke-test-slave-image ./docker-smoke-test-slave

