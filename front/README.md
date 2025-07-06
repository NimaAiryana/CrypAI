# CrypAI Frontend

This is the frontend for CrypAI, a free AI-powered cryptocurrency analysis platform. The frontend is built with Next.js, React, and TailwindCSS.

## Features

- **Cryptocurrency Data Display**: View detailed information about cryptocurrencies including price, market cap, volume, etc.
- **Market Overview**: Dashboard with global market metrics and trending coins.
- **AI-Powered Analysis**: Get technical, fundamental, and combined analysis for cryptocurrencies.
- **Price Charts**: Interactive charts for price history visualization.
- **Responsive Design**: Mobile-friendly interface.

## Prerequisites

- Node.js 18.0.0 or later
- npm or yarn

## Getting Started

1. Clone the repository
2. Install dependencies:

```bash
cd /Users/nima/Works/CrypAI/front
npm install
# or
yarn install
```

3. Create a `.env.local` file in the frontend directory with your backend API URL:

```
NEXT_PUBLIC_API_URL=http://localhost:5000
```

4. Run the development server:

```bash
npm run dev
# or
yarn dev
```

The frontend will be available at `http://localhost:3000`.

## Connecting to the Backend

The frontend is configured to connect to the backend API running at the URL specified in your `.env.local` file. Make sure your backend is running before starting the frontend to ensure proper data fetching.

## Building for Production

To build the application for production:

```bash
npm run build
npm run start
# or
yarn build
yarn start
```

## Technologies Used

- **Next.js**: React framework for production
- **React**: JavaScript library for building user interfaces
- **TailwindCSS**: Utility-first CSS framework
- **Axios**: HTTP client for API requests
- **Chart.js**: JavaScript charting library
- **Framer Motion**: Animation library for React
