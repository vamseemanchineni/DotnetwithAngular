FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
EXPOSE 8080

#Copy csproj and restore as distinct files
COPY *.csproj ./
RUN dotnet restore


#Copy everything and build
COPY . ./
RUN dotnet publish -c Release -o out

#build run time image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out/ .
ENTRYPOINT ["dotnet", "API.dll"]