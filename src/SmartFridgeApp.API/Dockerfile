FROM mcr.microsoft.com/dotnet/aspnet:10.0
ARG SOURCE=obj/docker/publish

RUN apt-get update \
    && apt-get install -y --no-install-recommends libc-dev \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY ${SOURCE} .

RUN useradd --no-create-home --shell /bin/false appuser \
    && chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["sh", "-c", "sleep 10 && ./SmartFridgeApp.API"]