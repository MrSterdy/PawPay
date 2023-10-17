FROM mcr.microsoft.com/dotnet/sdk:7.0.402-alpine3.18 AS build

WORKDIR /app

COPY . .

RUN dotnet publish ./src/PawPay.Web -r alpine-x64 -c release -o ./build --sc true /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/aspnet:7.0.12-alpine3.18 as runtime

WORKDIR /app

COPY --from=build /app/build .

EXPOSE 80

ENTRYPOINT ["./PawPay.Web"]