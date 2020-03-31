FROM mcr.microsoft.com/dotnet/core/sdk:3.1 
WORKDIR /app
COPY EventSourcing.Library/*.csproj EventSourcing.Library/
COPY Contracts/*.csproj Contracts/
COPY Domain/*.csproj Domain/
COPY ApplicationServices/*.csproj ApplicationServices/
COPY ReadModel/*.csproj ReadModel/
COPY OrderApi/*.csproj OrderApi/
RUN dotnet restore OrderApi
COPY EventSourcing.Library EventSourcing.Library
COPY Contracts Contracts
COPY Domain Domain
COPY ApplicationServices ApplicationServices
COPY ReadModel ReadModel
COPY OrderApi OrderApi
CMD dotnet run -p OrderApi