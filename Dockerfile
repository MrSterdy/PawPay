FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

COPY . .

RUN dotnet publish -c release -o ./build

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime

WORKDIR /app

COPY --from=build /app/build .

EXPOSE 80

ENTRYPOINT ["dotnet", "PawPay.Web.dll"]