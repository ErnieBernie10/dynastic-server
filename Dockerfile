#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Install OpenSSH and set the password for root to "Docker!". In this example, "apk add" is the install instruction for an Alpine Linux-based image.
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
RUN apt-get update \
      && apt-get install -y --no-install-recommends dialog \
      && apt-get update \
      && apt-get install -y --no-install-recommends openssh-server \
      && echo "root:Docker!" | chpasswd 

# Copy the sshd_config file to the /etc/ssh/ directory
COPY sshd_config /etc/ssh/

# Copy and configure the ssh_setup file
RUN mkdir -p /tmp
COPY ssh_setup.sh /tmp
RUN chmod +x /tmp/ssh_setup.sh \
    && (sleep 1;/tmp/ssh_setup.sh 2>&1 > /dev/null) 

COPY init.sh /usr/local/bin/

# Open port 2222 for SSH access
EXPOSE 80 2222

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Dynastic.API/Dynastic.API.csproj", "Dynastic.API/"]
RUN dotnet restore "Dynastic.API/Dynastic.API.csproj"
COPY . .
WORKDIR "/src/Dynastic.API"
RUN dotnet build "Dynastic.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dynastic.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["bash","init.sh"]
