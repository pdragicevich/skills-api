#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SkillsAPI.Albemuth.Net/SkillsAPI.Albemuth.Net.csproj", "SkillsAPI.Albemuth.Net/"]
RUN dotnet restore "SkillsAPI.Albemuth.Net/SkillsAPI.Albemuth.Net.csproj"
COPY . .
WORKDIR "/src/SkillsAPI.Albemuth.Net"
RUN dotnet build "SkillsAPI.Albemuth.Net.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SkillsAPI.Albemuth.Net.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkillsAPI.Albemuth.Net.dll"]
