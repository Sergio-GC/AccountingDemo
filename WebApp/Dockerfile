FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebApp/WebApp.csproj", "WebApp/"]
COPY ["BLLAccountingDemo/BLLAccountingDemo.csproj", "BLLAccountingDemo/"]
COPY ["DTO/DTO.csproj", "DTO/"]
COPY ["EFAccounting/EFAccounting.csproj", "EFAccounting/"]
RUN dotnet restore "WebApp/WebApp.csproj"
# Copy only the necessary source files, avoiding appsettings.json conflicts
COPY ["WebApp/", "WebApp/"]
COPY ["BLLAccountingDemo/", "BLLAccountingDemo/"]
COPY ["DTO/", "DTO/"]
COPY ["EFAccounting/Entities/", "EFAccounting/Entities/"]
COPY ["EFAccounting/Context.cs", "EFAccounting/"]
COPY ["EFAccounting/ContextFactory.cs", "EFAccounting/"]
WORKDIR "/src/WebApp"
RUN dotnet build "WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp.dll"] 