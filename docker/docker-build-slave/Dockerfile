FROM microsoft/dotnet:1.1.0-sdk-projectjson
MAINTAINER Ciprian Toader

#VOLUME  ["/var/lib/jenkins/.nuget:/var/lib/jenkins/.nuget", 
#         "/var/lib/jenkins/.local/share/NuGet:/var/lib/jenkins/.local/share/NuGet"]
#VOLUME  ["/var/lib/jenkins/.nuget", "/var/lib/jenkins/workspace/SolarMonitor"]
#WORKDIR "/var/lib/jenkins/workspace/SolarMonitor"
COPY usr/bin/monodis /usr/bin/
RUN /bin/bash -c 'mkdir -p /usr/lib/mono/4.5/'
COPY usr/lib/mono/4.5/mscorlib.dll /usr/lib/mono/4.5/

RUN /bin/bash -c 'mkdir /var/lib/jenkins ; chown 136.148 /var/lib/jenkins'

# Fixing some incorrect permissions in the base image
RUN /bin/bash -c 'find /usr/share/dotnet -type f -exec chmod o+r {} \;'
RUN /bin/bash -c 'find /usr/share/dotnet -type d -exec chmod o+rx {} \;'

ENV HOME /var/lib/jenkins


