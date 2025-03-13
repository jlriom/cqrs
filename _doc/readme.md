docker network create --attachable -d bridge mydockernetwork

docker run -it -d --name mongo-container -p 27017:27017 --network mydockerneetwork --restart always -v mongodb_data_container:/data/db mongo:latest

docker run -d --name sql-container \
--network mydockernetwork \
--restart always \
-e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=$tr0ngS@P@ssw0rd02' -e 'MSSQL_PID=Express' \
-p 1433:1433 mcr.microsoft.com/mssql/server:2017-latest-ubuntu 