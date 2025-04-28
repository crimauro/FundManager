# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln ./
COPY FundCoreAPI/FundCoreAPI.csproj FundCoreAPI/
COPY FundCoreAPI.Tests/FundCoreAPI.Tests.csproj FundCoreAPI.Tests/
RUN dotnet restore

# copy everything else and build
COPY . .
WORKDIR /app/FundCoreAPI
RUN dotnet publish -c Release -o /app/publish

# execute the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# copy the build app to the runtime image
COPY --from=build /app/publish .

# expose the port the app runs on
EXPOSE 60721

# command to run the application
ENTRYPOINT ["dotnet", "FundCoreAPI.dll"]
