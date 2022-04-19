#!/usr/bin/env bash

HOST=localhost

mvn -pl avro-models clean install
mvn -pl translation-service,document-service clean install dockerfile:build

# Both the document and translation services need to be running. Open two different terminal windows
# and run the appropriate module commands below in the respective windows if you want them to be run manually:
#
# Module 3:
#   mvn -pl document-service spring-boot:run -Dspring-boot.run.profiles=m03
#   mvn -pl translation-service spring-boot:run -Dspring-boot.run.profiles=m03
#
# Module 4:
#   Functional only when run with docker-compose
#
# Module 5:
#   Functional only when run with docker-compose

curl -v --user translator:password \
        -H 'Content-Type: application/json' \
        -X PUT \
        http://${HOST}:8083/register \
        -d '{"languages": [{"source": "English", "target": "French"}]}'

echo "This is an example" > /tmp/example.txt
curl -v --user user:password http://${HOST}:8082/documents | jq
curl -v --user user:password -X PUT -F 'file=@/tmp/example.txt' http://${HOST}:8082/documents
curl -v --user user:password http://${HOST}:8082/documents | jq

curl -v --user user:password \
        -H 'Content-Type: application/json' \
        http://${HOST}:8083/submissions \
        -d '{"name": "example.txt", "etag": "cf778431631ef17d1e8bb1b84e13d25a", "source": "English", "target": "French", "completionDate": 4099769220000}'
curl -v --user user:password http://${HOST}:8083/submissions | jq

curl -v --user user:password -X DELETE http://${HOST}:8083/submissions/ea881a983c9fc1de058c87bb363d0ad376052a30f3597aeeca74c1f98df4f6c9

curl -v --user user:password http://${HOST}:8083/submissions | jq

echo "This is an example." > /tmp/example.txt
curl -v --user user:password -X PUT -F 'file=@/tmp/example.txt' http://${HOST}:8082/documents
curl -v --user user:password http://${HOST}:8082/documents | jq

curl -v --user user:password \
        -H 'Content-Type: application/json' \
        http://${HOST}:8083/submissions \
        -d '{"name": "example.txt", "etag": "46edc6541babd006bb52223c664b29a3", "source": "English", "target": "French", "completionDate": 4099769220000}'
curl -v --user user:password http://${HOST}:8083/submissions | jq

curl -v --user translator:password -X POST http://${HOST}:8083/submissions/a63afcdaa0ffc93690d544e94263bbc1c124c64da55c2cd8ab3c0aab60733ef6/accept

curl -v --user user:password http://${HOST}:8083/submissions | jq
