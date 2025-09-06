FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
WORKDIR /src/src/01-Presentation/Kantar.ShoppingBasket.Presentation.WebApi
RUN dotnet publish "Kantar.ShoppingBasket.Presentation.WebApi.csproj" -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .
EXPOSE 5020
ENTRYPOINT ["dotnet", "Kantar.ShoppingBasket.Presentation.WebApi.dll"]