import axios from 'axios';

// Base API configuration
const API = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Crypto API endpoints
export const cryptoApi = {
  // Get list of cryptocurrencies with optional pagination
  getList: async (page = 1, limit = 20, sortBy = 'marketCap', sortDirection = 'desc') => {
    const response = await API.get(`/api/crypto/list?page=${page}&limit=${limit}&sortBy=${sortBy}&sortDirection=${sortDirection}`);
    return response.data;
  },
  
  // Get details for a specific cryptocurrency
  getDetails: async (id: string) => {
    const response = await API.get(`/api/crypto/details/${id}`);
    return response.data;
  },
  
  // Get price history for a cryptocurrency
  getPriceHistory: async (id: string, timeframe = '7d') => {
    const response = await API.get(`/api/crypto/price-history/${id}?timeframe=${timeframe}`);
    return response.data;
  },
  
  // Search for cryptocurrencies
  search: async (query: string) => {
    const response = await API.get(`/api/crypto/search?query=${encodeURIComponent(query)}`);
    return response.data;
  },
};

// Market API endpoints
export const marketApi = {
  // Get market overview
  getOverview: async () => {
    const response = await API.get('/api/market/overview');
    return response.data;
  },
  
  // Get trending cryptocurrencies
  getTrending: async () => {
    const response = await API.get('/api/market/trending');
    return response.data;
  },
  
  // Get global market metrics
  getGlobalMetrics: async () => {
    const response = await API.get('/api/market/global-metrics');
    return response.data;
  },
};

// Analysis API endpoints
export const analysisApi = {
  // Get technical analysis for a cryptocurrency
  getTechnicalAnalysis: async (cryptoId: string, timeframe = '24h') => {
    const response = await API.get(`/api/analysis/technical/${cryptoId}?timeframe=${timeframe}`);
    return response.data;
  },
  
  // Get fundamental analysis for a cryptocurrency
  getFundamentalAnalysis: async (cryptoId: string) => {
    const response = await API.get(`/api/analysis/fundamental/${cryptoId}`);
    return response.data;
  },
  
  // Get combined analysis for a cryptocurrency
  getCombinedAnalysis: async (cryptoId: string) => {
    const response = await API.get(`/api/analysis/combined/${cryptoId}`);
    return response.data;
  },
};

// Export all API services
export default {
  crypto: cryptoApi,
  market: marketApi,
  analysis: analysisApi,
};
