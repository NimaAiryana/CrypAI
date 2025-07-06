import { useState, useEffect } from 'react';
import { cryptoApi, marketApi, analysisApi } from './api';

// Hook for fetching cryptocurrency list
export function useCryptoList(page = 1, limit = 20, sortBy = 'marketCap', sortDirection = 'desc') {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const result = await cryptoApi.getList(page, limit, sortBy, sortDirection);
        setData(result);
        setError(null);
      } catch (err) {
        setError(err.message || 'An error occurred while fetching crypto list');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [page, limit, sortBy, sortDirection]);

  return { data, loading, error };
}

// Hook for fetching cryptocurrency details
export function useCryptoDetails(id) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      if (!id) {
        setLoading(false);
        return;
      }
      
      try {
        setLoading(true);
        const result = await cryptoApi.getDetails(id);
        setData(result);
        setError(null);
      } catch (err) {
        setError(err.message || `An error occurred while fetching details for crypto ${id}`);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  return { data, loading, error };
}

// Hook for fetching price history
export function usePriceHistory(id, timeframe = '7d') {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      if (!id) {
        setLoading(false);
        return;
      }
      
      try {
        setLoading(true);
        const result = await cryptoApi.getPriceHistory(id, timeframe);
        setData(result);
        setError(null);
      } catch (err) {
        setError(err.message || `An error occurred while fetching price history for crypto ${id}`);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id, timeframe]);

  return { data, loading, error };
}

// Hook for fetching market overview
export function useMarketOverview() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const result = await marketApi.getOverview();
        setData(result);
        setError(null);
      } catch (err) {
        setError(err.message || 'An error occurred while fetching market overview');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  return { data, loading, error };
}

// Hook for fetching technical analysis
export function useTechnicalAnalysis(cryptoId, timeframe = '24h') {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      if (!cryptoId) {
        setLoading(false);
        return;
      }
      
      try {
        setLoading(true);
        const result = await analysisApi.getTechnicalAnalysis(cryptoId, timeframe);
        setData(result);
        setError(null);
      } catch (err) {
        setError(err.message || `An error occurred while fetching technical analysis for crypto ${cryptoId}`);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [cryptoId, timeframe]);

  return { data, loading, error };
}

// Hook for fetching fundamental analysis
export function useFundamentalAnalysis(cryptoId) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      if (!cryptoId) {
        setLoading(false);
        return;
      }
      
      try {
        setLoading(true);
        const result = await analysisApi.getFundamentalAnalysis(cryptoId);
        setData(result);
        setError(null);
      } catch (err) {
        setError(err.message || `An error occurred while fetching fundamental analysis for crypto ${cryptoId}`);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [cryptoId]);

  return { data, loading, error };
}

// Hook for fetching combined analysis
export function useCombinedAnalysis(cryptoId) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      if (!cryptoId) {
        setLoading(false);
        return;
      }
      
      try {
        setLoading(true);
        const result = await analysisApi.getCombinedAnalysis(cryptoId);
        setData(result);
        setError(null);
      } catch (err) {
        setError(err.message || `An error occurred while fetching combined analysis for crypto ${cryptoId}`);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [cryptoId]);

  return { data, loading, error };
}
