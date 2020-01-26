#!/bin/bash
set -ev

docker build -t dariusz151/smartfridgeapp:dev .

#docker login -u="$DOCKER_USERNAME" -p="$DOCKER_PASSWORD"
#docker push repository/project:$TAG
#docker push repository/project:latest