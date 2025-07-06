# CrypAI - AI-Powered Cryptocurrency Analysis Platform

CrypAI is an advanced cryptocurrency analysis platform that combines real-time market data with AI-powered insights to help investors make informed decisions. The platform uses Google Gemini API to provide technical and fundamental analyses for cryptocurrencies.

## Project Structure

The project consists of two main components:

### Backend (.NET 8)

- **API Endpoints**: Provides cryptocurrency data, price history, and AI-powered analyses.
- **AI Integration**: Uses Google Gemini API for generating insights.
- **Data Sources**: Integrates with CoinMarketCap and other crypto data providers.

### Frontend (Next.js)

- **Modern UI**: Built with Next.js, React, and Tailwind CSS.
- **Real-time Data**: Displays cryptocurrency information with automatic updates.
- **Interactive Analyses**: Shows technical, fundamental, and combined analyses.

## Features

- **Cryptocurrency Listing**: View a comprehensive list of cryptocurrencies with key metrics.
- **Detailed Analysis**: Get in-depth information about specific cryptocurrencies.
- **Technical Analysis**: AI-generated technical analysis based on price patterns and indicators.
- **Fundamental Analysis**: AI-generated fundamental analysis based on news, market conditions, and project developments.
- **Combined Analysis**: Holistic view combining both technical and fundamental factors.
- **User-Friendly Interface**: Clean, responsive design for both desktop and mobile devices.

## Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js and npm
- API keys for CoinMarketCap and Google Gemini

### Backend Setup

1. Navigate to the backend directory:
```bash
cd back
```

2. Create an `.env` file based on `.env.example`:
```bash
cp .env.example .env
```

3. Fill in your API keys in the `.env` file.

4. Run the backend:
```bash
dotnet run
```

The backend will start on http://localhost:5000.

### Frontend Setup

1. Navigate to the frontend directory:
```bash
cd front
```

2. Install dependencies:
```bash
npm install
```

3. Create an `.env.local` file based on `.env.local.example`:
```bash
cp .env.local.example .env.local
```

4. Run the frontend:
```bash
npm run dev
```

The frontend will start on http://localhost:3000.

## API Documentation

When running the backend, Swagger documentation is available at http://localhost:5000/swagger.

## Technology Stack

- **Backend**:
  - .NET 8
  - ASP.NET Core Web API
  - Google Gemini API
  - Memory Caching
  - Response Compression

- **Frontend**:
  - Next.js 14
  - React
  - Tailwind CSS
  - Axios

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
