FROM mcr.microsoft.com/dotnet/aspnet:10.0
ARG SOURCE

RUN apt update && apt install -y libc-dev

WORKDIR /app

COPY ${SOURCE:-obj/docker/publish/net10.0/linux-x64} .

ENTRYPOINT ["./SmartFridgeApp.API"]