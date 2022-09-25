FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY app ./
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "FireplaceApi.Api.dll"]
