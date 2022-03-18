FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
HEALTHCHECK --interval=5s --timeout=3s CMD dotnet DashTransit.App.dll --ready

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/DashTransit.App/DashTransit.App.csproj", "src/DashTransit.App/"]
COPY ["src/DashTransit.Core/DashTransit.Core.csproj", "src/DashTransit.Core/"]
COPY ["src/DashTransit.EntityFramework/DashTransit.EntityFramework.csproj", "src/DashTransit.EntityFramework/"]
RUN dotnet restore "src/DashTransit.App/DashTransit.App.csproj"
COPY . .
WORKDIR "/src/src/DashTransit.App"
RUN dotnet build "DashTransit.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DashTransit.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DashTransit.App.dll"]