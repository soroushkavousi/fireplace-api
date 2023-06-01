FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY app ./
ENV FIREPLACE_API_ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "FireplaceApi.Application.dll"]
