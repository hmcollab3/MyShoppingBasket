FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish "Kantar.DatabaseScriptRunner.csproj" -c Release -o /out

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .
COPY Scripts ./Scripts
ENTRYPOINT ["dotnet", "Kantar.DatabaseScriptRunner.dll"]