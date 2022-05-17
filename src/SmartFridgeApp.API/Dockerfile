FROM mcr.microsoft.com/dotnet/aspnet:6.0
ARG SOURCE

RUN apt update && apt install -y libc-dev

WORKDIR /app

COPY ${SOURCE:-obj/docker/publish/net6.0/linus-x64} .

ENTRYPOINT ["./SmartFridgeApp.API"]