FROM mcr.microsoft.com/mssql/server:2019-latest
USER root
RUN apt-get update \
    && apt-get install netcat -y \
    && rm -rf /var/lib/apt/lists/*
USER mssql