FROM mcr.microsoft.com/dotnet/sdk:10.0

WORKDIR /app

COPY . ./src

CMD ["dotnet", "run", "./src/PortfolioSelenium.cs", "--report-trx"]
