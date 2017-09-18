FROM jenkins/webapi-base-netcoreapp1.1
#FROM microsoft/dotnet:1.0.0-core

#RUN printf "deb http://ftp.us.debian.org/debian jessie main\n" >> /etc/apt/sources.list
#RUN apt-get -qq update && apt-get install -qqy sqlite3 libsqlite3-dev && rm -rf /var/lib/apt/lists/*

COPY bin/Debug/netcoreapp1.1/publish/ /app
COPY appsettings.json /app
WORKDIR /app
EXPOSE 80/tcp
ENV ASPNETCORE_URLS http://*:5000:80
ENTRYPOINT dotnet /app/SolarMonitor.Web.Api.dll 
