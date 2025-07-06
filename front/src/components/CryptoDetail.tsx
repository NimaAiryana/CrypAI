"use client";

import React, { useState } from 'react';
import { useCryptoDetails, usePriceHistory, useTechnicalAnalysis, useFundamentalAnalysis, useCombinedAnalysis } from '../lib/hooks';

type CryptoDetailProps = {
  cryptoId: string;
};

const CryptoDetail: React.FC<CryptoDetailProps> = ({ cryptoId }) => {
  const [timeframe, setTimeframe] = useState('24h');
  const [analysisType, setAnalysisType] = useState<'technical' | 'fundamental' | 'combined'>('technical');
  
  const { data: cryptoData, loading: cryptoLoading, error: cryptoError } = useCryptoDetails(cryptoId);
  const { data: priceHistory, loading: priceLoading } = usePriceHistory(cryptoId, '7d');
  const { data: technicalAnalysis, loading: techLoading } = useTechnicalAnalysis(cryptoId, timeframe);
  const { data: fundamentalAnalysis, loading: fundLoading } = useFundamentalAnalysis(cryptoId);
  const { data: combinedAnalysis, loading: combinedLoading } = useCombinedAnalysis(cryptoId);
  
  if (cryptoLoading) {
    return <div className="flex justify-center items-center p-8">Loading cryptocurrency details...</div>;
  }
  
  if (cryptoError) {
    return <div className="text-red-500 p-4">Error: {cryptoError}</div>;
  }
  
  if (!cryptoData?.data) {
    return <div className="text-red-500 p-4">Cryptocurrency not found</div>;
  }
  
  const crypto = cryptoData.data;
  
  // Format currency
  const formatCurrency = (num: number, decimals = 2) => {
    if (num === undefined || num === null) return 'N/A';
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: decimals,
      maximumFractionDigits: decimals
    }).format(num);
  };
  
  // Render appropriate analysis based on selected type
  const renderAnalysis = () => {
    if (analysisType === 'technical') {
      if (techLoading) return <div className="p-4">Loading technical analysis...</div>;
      if (!technicalAnalysis?.data) return <div className="p-4">No technical analysis available</div>;
      
      const analysis = technicalAnalysis.data;
      
      return (
        <div className="space-y-4">
          <h3 className="text-xl font-semibold">Technical Analysis - {analysis.timeframe}</h3>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
              <h4 className="font-medium text-gray-700 dark:text-gray-300">Trend Direction</h4>
              <p className={`text-lg font-bold ${
                analysis.trendDirection === 'Bullish' ? 'text-green-600' : 
                analysis.trendDirection === 'Bearish' ? 'text-red-600' : 'text-yellow-600'
              }`}>{analysis.trendDirection}</p>
            </div>
            <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
              <h4 className="font-medium text-gray-700 dark:text-gray-300">RSI</h4>
              <p className="text-lg font-bold">{analysis.rsi}</p>
            </div>
            <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
              <h4 className="font-medium text-gray-700 dark:text-gray-300">Recommendation</h4>
              <p className={`text-lg font-bold ${
                analysis.recommendation.includes('Buy') ? 'text-green-600' : 
                analysis.recommendation.includes('Sell') ? 'text-red-600' : 'text-yellow-600'
              }`}>{analysis.recommendation}</p>
            </div>
          </div>
          
          <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
            <h4 className="font-medium text-gray-700 dark:text-gray-300 mb-2">Support & Resistance Levels</h4>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <h5 className="text-sm text-gray-500 dark:text-gray-400">Support</h5>
                {Object.entries(analysis.supportLevels).map(([key, value]) => (
                  <p key={key} className="text-green-600 font-medium">{key}: {formatCurrency(value)}</p>
                ))}
              </div>
              <div>
                <h5 className="text-sm text-gray-500 dark:text-gray-400">Resistance</h5>
                {Object.entries(analysis.resistanceLevels).map(([key, value]) => (
                  <p key={key} className="text-red-600 font-medium">{key}: {formatCurrency(value)}</p>
                ))}
              </div>
            </div>
          </div>
          
          <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
            <h4 className="font-medium text-gray-700 dark:text-gray-300 mb-2">Summary</h4>
            <p className="text-gray-800 dark:text-gray-200 whitespace-pre-wrap">{analysis.summary}</p>
          </div>
        </div>
      );
    } else if (analysisType === 'fundamental') {
      if (fundLoading) return <div className="p-4">Loading fundamental analysis...</div>;
      if (!fundamentalAnalysis?.data) return <div className="p-4">No fundamental analysis available</div>;
      
      const analysis = fundamentalAnalysis.data;
      
      return (
        <div className="space-y-4">
          <h3 className="text-xl font-semibold">Fundamental Analysis</h3>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
              <h4 className="font-medium text-gray-700 dark:text-gray-300">Market Sentiment</h4>
              <p className={`text-lg font-bold ${
                analysis.marketSentiment.includes('Positive') ? 'text-green-600' : 
                analysis.marketSentiment.includes('Negative') ? 'text-red-600' : 'text-yellow-600'
              }`}>{analysis.marketSentiment}</p>
            </div>
            <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
              <h4 className="font-medium text-gray-700 dark:text-gray-300">Recommendation</h4>
              <p className={`text-lg font-bold ${
                analysis.recommendation.includes('Buy') ? 'text-green-600' : 
                analysis.recommendation.includes('Sell') ? 'text-red-600' : 'text-yellow-600'
              }`}>{analysis.recommendation}</p>
            </div>
          </div>
          
          <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
            <h4 className="font-medium text-gray-700 dark:text-gray-300 mb-2">Team Assessment</h4>
            <p className="text-gray-800 dark:text-gray-200 whitespace-pre-wrap">{analysis.teamAssessment}</p>
          </div>
          
          <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
            <h4 className="font-medium text-gray-700 dark:text-gray-300 mb-2">Technology Assessment</h4>
            <p className="text-gray-800 dark:text-gray-200 whitespace-pre-wrap">{analysis.technologyAssessment}</p>
          </div>
          
          <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
            <h4 className="font-medium text-gray-700 dark:text-gray-300 mb-2">Community Assessment</h4>
            <p className="text-gray-800 dark:text-gray-200 whitespace-pre-wrap">{analysis.communityAssessment}</p>
          </div>
          
          <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
            <h4 className="font-medium text-gray-700 dark:text-gray-300 mb-2">Competitive Analysis</h4>
            <p className="text-gray-800 dark:text-gray-200 whitespace-pre-wrap">{analysis.competitiveAnalysis}</p>
          </div>
          
          <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
            <h4 className="font-medium text-gray-700 dark:text-gray-300 mb-2">Summary</h4>
            <p className="text-gray-800 dark:text-gray-200 whitespace-pre-wrap">{analysis.summary}</p>
          </div>
        </div>
      );
    } else {
      if (combinedLoading) return <div className="p-4">Loading combined analysis...</div>;
      if (!combinedAnalysis?.data) return <div className="p-4">No combined analysis available</div>;
      
      const analysis = combinedAnalysis.data;
      
      return (
        <div className="space-y-4">
          <h3 className="text-xl font-semibold">Combined Analysis</h3>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
              <h4 className="font-medium text-gray-700 dark:text-gray-300">Overall Score</h4>
              <p className={`text-lg font-bold ${
                analysis.overallScore >= 70 ? 'text-green-600' : 
                analysis.overallScore <= 30 ? 'text-red-600' : 'text-yellow-600'
              }`}>{analysis.overallScore}/100</p>
            </div>
            <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
              <h4 className="font-medium text-gray-700 dark:text-gray-300">Recommendation</h4>
              <p className={`text-lg font-bold ${
                analysis.recommendation.includes('Buy') ? 'text-green-600' : 
                analysis.recommendation.includes('Sell') ? 'text-red-600' : 'text-yellow-600'
              }`}>{analysis.recommendation}</p>
            </div>
          </div>
          
          <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
            <h4 className="font-medium text-gray-700 dark:text-gray-300 mb-2">Integrated Outlook</h4>
            <p className="text-gray-800 dark:text-gray-200 whitespace-pre-wrap">{analysis.integratedOutlook}</p>
          </div>
          
          <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
            <h4 className="font-medium text-gray-700 dark:text-gray-300 mb-2">Summary</h4>
            <p className="text-gray-800 dark:text-gray-200 whitespace-pre-wrap">{analysis.summary}</p>
          </div>
        </div>
      );
    }
  };
  
  return (
    <div className="space-y-6">
      {/* Crypto Header */}
      <div className="flex flex-col md:flex-row md:justify-between md:items-center">
        <div>
          <h2 className="text-2xl font-bold">{crypto.name} ({crypto.symbol})</h2>
          <p className="text-gray-600 dark:text-gray-400">Rank #{crypto.rank}</p>
        </div>
        <div className="mt-2 md:mt-0">
          <span className="text-xl font-bold">{formatCurrency(crypto.price)}</span>
          <span className={`ml-2 font-medium ${crypto.changePercentage24h >= 0 ? 'text-green-600' : 'text-red-600'}`}>
            {crypto.changePercentage24h >= 0 ? '+' : ''}{crypto.changePercentage24h}%
          </span>
        </div>
      </div>
      
      {/* Price & Market Info */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
          <h3 className="text-lg font-medium mb-2">Market Cap</h3>
          <p className="text-xl font-semibold">{formatCurrency(crypto.marketCap, 0)}</p>
        </div>
        <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
          <h3 className="text-lg font-medium mb-2">24h Volume</h3>
          <p className="text-xl font-semibold">{formatCurrency(crypto.volume24h, 0)}</p>
        </div>
        <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
          <h3 className="text-lg font-medium mb-2">Circulating Supply</h3>
          <p className="text-xl font-semibold">{crypto.circulatingSupply?.toLocaleString()} {crypto.symbol}</p>
        </div>
      </div>
      
      {/* Price Chart */}
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-4">
        <div className="flex justify-between items-center mb-4">
          <h3 className="text-lg font-medium">Price Chart</h3>
          <div className="flex space-x-2">
            <button 
              onClick={() => setTimeframe('24h')}
              className={`px-3 py-1 rounded ${timeframe === '24h' ? 'bg-blue-600 text-white' : 'bg-gray-200 text-gray-800'}`}>
              24h
            </button>
            <button 
              onClick={() => setTimeframe('7d')}
              className={`px-3 py-1 rounded ${timeframe === '7d' ? 'bg-blue-600 text-white' : 'bg-gray-200 text-gray-800'}`}>
              7d
            </button>
            <button 
              onClick={() => setTimeframe('30d')}
              className={`px-3 py-1 rounded ${timeframe === '30d' ? 'bg-blue-600 text-white' : 'bg-gray-200 text-gray-800'}`}>
              30d
            </button>
          </div>
        </div>
        
        {priceLoading ? (
          <div className="h-64 flex justify-center items-center">Loading chart data...</div>
        ) : (
          <div className="h-64">
            {/* Chart would go here - for simplicity, I'm just showing a placeholder */}
            <div className="h-full flex justify-center items-center text-gray-500 bg-gray-100 dark:bg-gray-700 rounded">
              Chart would render here using react-chartjs-2 with {priceHistory?.data?.points?.length || 0} data points
            </div>
          </div>
        )}
      </div>
      
      {/* Analysis Tabs */}
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow">
        <div className="border-b border-gray-200 dark:border-gray-700">
          <div className="flex">
            <button 
              onClick={() => setAnalysisType('technical')}
              className={`py-4 px-6 border-b-2 font-medium text-sm ${
                analysisType === 'technical' 
                  ? 'border-blue-500 text-blue-600 dark:border-blue-400 dark:text-blue-400'
                  : 'border-transparent text-gray-600 dark:text-gray-300'
              }`}>
              Technical Analysis
            </button>
            <button 
              onClick={() => setAnalysisType('fundamental')}
              className={`py-4 px-6 border-b-2 font-medium text-sm ${
                analysisType === 'fundamental' 
                  ? 'border-blue-500 text-blue-600 dark:border-blue-400 dark:text-blue-400'
                  : 'border-transparent text-gray-600 dark:text-gray-300'
              }`}>
              Fundamental Analysis
            </button>
            <button 
              onClick={() => setAnalysisType('combined')}
              className={`py-4 px-6 border-b-2 font-medium text-sm ${
                analysisType === 'combined' 
                  ? 'border-blue-500 text-blue-600 dark:border-blue-400 dark:text-blue-400'
                  : 'border-transparent text-gray-600 dark:text-gray-300'
              }`}>
              Combined Analysis
            </button>
          </div>
        </div>
        <div className="p-6">
          {renderAnalysis()}
        </div>
      </div>
    </div>
  );
};

export default CryptoDetail;
