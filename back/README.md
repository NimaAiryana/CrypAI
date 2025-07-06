# CrypAI Backend

This is the backend API for CrypAI, a free AI-powered cryptocurrency analysis platform. The backend is built with ASP.NET Core and provides endpoints for cryptocurrency data, market information, and AI-powered analysis.

## Features

- **Cryptocurrency Data**: Get detailed information about cryptocurrencies including price, market cap, volume, etc.
- **Market Data**: Get global market metrics, trending coins, and market overview.
- **AI-Powered Analysis**: Generate technical, fundamental, and combined analysis for cryptocurrencies using AI.
- **Caching**: Implements intelligent caching to reduce API calls and improve performance.
- **CORS Support**: Configured for cross-origin requests from the frontend.
- **Swagger Documentation**: API endpoints are documented with Swagger/OpenAPI.

## Prerequisites

- .NET 8.0 SDK or later
- CoinMarketCap API key
- OpenAI API key

## Getting Started

1. Clone the repository
2. Copy `.env.example` to `.env` and fill in your API keys
3. Run the application:

```bash
dotnet run
```

The API will be available at `http://localhost:5000` and Swagger documentation at `http://localhost:5000/swagger`.

## Configuration

API keys and other settings can be configured in the following ways:

1. Environment variables
2. User secrets (recommended for development)
3. appsettings.json (not recommended for secrets)

## API Endpoints

### Cryptocurrency

- `GET /api/crypto/list`: Get a paginated list of cryptocurrencies
- `GET /api/crypto/details/{id}`: Get detailed information about a specific cryptocurrency
- `GET /api/crypto/price-history/{id}`: Get price history for a cryptocurrency
- `GET /api/crypto/search`: Search for cryptocurrencies by name or symbol

### Market

- `GET /api/market/overview`: Get a general market overview
- `GET /api/market/trending`: Get trending cryptocurrencies
- `GET /api/market/global-metrics`: Get global market metrics

### Analysis

- `GET /api/analysis/technical/{cryptoId}`: Get AI-powered technical analysis for a cryptocurrency
- `GET /api/analysis/fundamental/{cryptoId}`: Get AI-powered fundamental analysis for a cryptocurrency
- `GET /api/analysis/combined/{cryptoId}`: Get combined technical and fundamental analysis

## License

This project is licensed under the MIT License.
