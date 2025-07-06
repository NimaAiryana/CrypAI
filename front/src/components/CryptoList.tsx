import React, { useState } from 'react';
import { useCryptoList } from '../lib/hooks';

const CryptoList = () => {
  const [page, setPage] = useState(1);
  const [limit] = useState(20);
  const [sortBy, setSortBy] = useState('marketCap');
  const [sortDirection, setSortDirection] = useState('desc');
  
  const { data, loading, error } = useCryptoList(page, limit, sortBy, sortDirection);
  
  if (loading) {
    return <div className="flex justify-center items-center p-8">Loading cryptocurrencies...</div>;
  }
  
  if (error) {
    return <div className="text-red-500 p-4">Error: {error}</div>;
  }
  
  // Handle sort change
  const handleSortChange = (newSortBy: string) => {
    if (sortBy === newSortBy) {
      // Toggle direction if clicking the same column
      setSortDirection(sortDirection === 'asc' ? 'desc' : 'asc');
    } else {
      // Default to descending for new column
      setSortBy(newSortBy);
      setSortDirection('desc');
    }
  };
  
  // Format numbers with commas and appropriate decimals
  const formatNumber = (num: number, decimals = 2) => {
    if (num === undefined || num === null) return 'N/A';
    return new Intl.NumberFormat('en-US', {
      minimumFractionDigits: decimals,
      maximumFractionDigits: decimals
    }).format(num);
  };
  
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
  
  return (
    <div className="overflow-x-auto">
      <table className="min-w-full bg-white dark:bg-gray-800">
        <thead>
          <tr className="bg-gray-100 dark:bg-gray-700">
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider cursor-pointer" 
                onClick={() => handleSortChange('rank')}>
              Rank {sortBy === 'rank' && (sortDirection === 'asc' ? '↑' : '↓')}
            </th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider cursor-pointer" 
                onClick={() => handleSortChange('name')}>
              Name {sortBy === 'name' && (sortDirection === 'asc' ? '↑' : '↓')}
            </th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider cursor-pointer" 
                onClick={() => handleSortChange('price')}>
              Price {sortBy === 'price' && (sortDirection === 'asc' ? '↑' : '↓')}
            </th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider cursor-pointer" 
                onClick={() => handleSortChange('marketCap')}>
              Market Cap {sortBy === 'marketCap' && (sortDirection === 'asc' ? '↑' : '↓')}
            </th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider cursor-pointer" 
                onClick={() => handleSortChange('volume24h')}>
              Volume (24h) {sortBy === 'volume24h' && (sortDirection === 'asc' ? '↑' : '↓')}
            </th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider cursor-pointer" 
                onClick={() => handleSortChange('changePercentage24h')}>
              Change (24h) {sortBy === 'changePercentage24h' && (sortDirection === 'asc' ? '↑' : '↓')}
            </th>
          </tr>
        </thead>
        <tbody className="bg-white dark:bg-gray-800 divide-y divide-gray-200 dark:divide-gray-700">
          {data?.data?.map((crypto: any) => (
            <tr key={crypto.id} className="hover:bg-gray-50 dark:hover:bg-gray-700">
              <td className="px-6 py-4 whitespace-nowrap">{crypto.rank}</td>
              <td className="px-6 py-4 whitespace-nowrap">
                <div className="flex items-center">
                  {crypto.symbol && (
                    <span className="font-medium mr-2">{crypto.symbol}</span>
                  )}
                  <span className="text-gray-700 dark:text-gray-300">{crypto.name}</span>
                </div>
              </td>
              <td className="px-6 py-4 whitespace-nowrap">{formatCurrency(crypto.price)}</td>
              <td className="px-6 py-4 whitespace-nowrap">{formatCurrency(crypto.marketCap, 0)}</td>
              <td className="px-6 py-4 whitespace-nowrap">{formatCurrency(crypto.volume24h, 0)}</td>
              <td className={`px-6 py-4 whitespace-nowrap ${crypto.changePercentage24h >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                {crypto.changePercentage24h >= 0 ? '+' : ''}{formatNumber(crypto.changePercentage24h)}%
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      
      {/* Pagination */}
      <div className="px-4 py-3 flex items-center justify-between border-t border-gray-200 dark:border-gray-700">
        <div>
          <p className="text-sm text-gray-700 dark:text-gray-300">
            Showing page {page} of {data?.pagination?.totalPages || 1}
          </p>
        </div>
        <div>
          <button 
            onClick={() => setPage(Math.max(1, page - 1))}
            disabled={page <= 1}
            className={`px-4 py-2 border rounded-md mr-2 ${page <= 1 ? 'bg-gray-100 text-gray-400 cursor-not-allowed' : 'bg-white text-blue-600 hover:bg-blue-50'}`}>
            Previous
          </button>
          <button 
            onClick={() => setPage(Math.min(data?.pagination?.totalPages || 1, page + 1))}
            disabled={page >= (data?.pagination?.totalPages || 1)}
            className={`px-4 py-2 border rounded-md ${page >= (data?.pagination?.totalPages || 1) ? 'bg-gray-100 text-gray-400 cursor-not-allowed' : 'bg-white text-blue-600 hover:bg-blue-50'}`}>
            Next
          </button>
        </div>
      </div>
    </div>
  );
};

export default CryptoList;
