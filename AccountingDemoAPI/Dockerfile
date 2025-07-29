FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AccountingDemoAPI/AccountingDemoAPI.csproj", "AccountingDemoAPI/"]
COPY ["BLLAccountingDemo/BLLAccountingDemo.csproj", "BLLAccountingDemo/"]
COPY ["DTO/DTO.csproj", "DTO/"]
COPY ["EFAccounting/EFAccounting.csproj", "EFAccounting/"]
RUN dotnet restore "AccountingDemoAPI/AccountingDemoAPI.csproj"
# Copy all source files
COPY . .
WORKDIR "/src/AccountingDemoAPI"
RUN dotnet build "AccountingDemoAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccountingDemoAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccountingDemoAPI.dll"] 