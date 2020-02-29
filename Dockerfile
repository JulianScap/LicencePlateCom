FROM mcr.microsoft.com/dotnet/core/runtime:3.1

COPY LicencePlateCom.API/bin/Release/netcoreapp3.1/publish/ app/

ENTRYPOINT ["dotnet", "app/LicencePlateCom.API.exe"]
