"use client";

import { useEffect, useState } from 'react';
import { motion } from 'framer-motion';
import { Line } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  Filler
} from 'chart.js';

// Register ChartJS components
ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  Filler
);

// Mock data for demonstration
const mockCryptoData = [
  { id: 1, name: 'Bitcoin', symbol: 'BTC', price: 65432.10, change24h: 2.45, marketCap: 1243000000000, volume24h: 32100000000 },
  { id: 2, name: 'Ethereum', symbol: 'ETH', price: 3521.75, change24h: -1.23, marketCap: 423000000000, volume24h: 15700000000 },
  { id: 3, name: 'Binance Coin', symbol: 'BNB', price: 432.67, change24h: 0.87, marketCap: 72300000000, volume24h: 2150000000 },
  { id: 4, name: 'Solana', symbol: 'SOL', price: 145.23, change24h: 5.67, marketCap: 61500000000, volume24h: 3720000000 },
  { id: 5, name: 'Cardano', symbol: 'ADA', price: 0.54, change24h: -0.34, marketCap: 19200000000, volume24h: 890000000 },
];

// Mock chart data
const mockChartData = {
  labels: ['7d', '6d', '5d', '4d', '3d', '2d', '1d', 'Today'],
  datasets: [
    {
      label: 'Market Cap (Trillion USD)',
      data: [2.31, 2.28, 2.35, 2.42, 2.38, 2.41, 2.45, 2.47],
      borderColor: 'rgba(14, 165, 233, 1)', // primary-500
      backgroundColor: 'rgba(14, 165, 233, 0.1)',
      fill: true,
      tension: 0.4,
    },
  ],
};

const chartOptions = {
  responsive: true,
  plugins: {
    legend: {
      display: false,
    },
    tooltip: {
      mode: 'index',
      intersect: false,
    },
  },
  scales: {
    y: {
      beginAtZero: false,
    },
  },
  interaction: {
    mode: 'nearest',
    axis: 'x',
    intersect: false,
  },
};

export default function MarketOverview() {
  const [marketData, setMarketData] = useState({
    totalMarketCap: '2.47T',
    totalVolume24h: '98.7B',
    btcDominance: '43.2%',
    activeCryptocurrencies: '12,563',
  });

  return (
    <div>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-10">
        {[
          { label: 'Total Market Cap', value: marketData.totalMarketCap, icon: '💰' },
          { label: '24h Trading Volume', value: marketData.totalVolume24h, icon: '📊' },
          { label: 'BTC Dominance', value: marketData.btcDominance, icon: '🏆' },
          { label: 'Active Cryptocurrencies', value: marketData.activeCryptocurrencies, icon: '🪙' },
        ].map((item, index) => (
          <motion.div 
            key={index} 
            className="card p-6"
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.3, delay: index * 0.1 }}
          >
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-500 dark:text-gray-400 text-sm">{item.label}</p>
                <h3 className="text-2xl font-bold">{item.value}</h3>
              </div>
              <div className="text-3xl">{item.icon}</div>
            </div>
          </motion.div>
        ))}
      </div>

      {/* Market Chart */}
      <div className="card p-6 mb-10">
        <h3 className="text-xl font-bold mb-4">Global Market Cap Last 7 Days</h3>
        <div className="h-72">
          <Line data={mockChartData} options={chartOptions} />
        </div>
      </div>

      {/* Top Cryptocurrencies */}
      <div className="card p-6">
        <h3 className="text-xl font-bold mb-4">Top Cryptocurrencies</h3>
        <div className="overflow-x-auto">
          <table className="min-w-full">
            <thead>
              <tr className="border-b border-gray-200 dark:border-gray-700">
                <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Name</th>
                <th className="px-4 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Price (USD)</th>
                <th className="px-4 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">24h Change</th>
                <th className="px-4 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Market Cap</th>
                <th className="px-4 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Volume (24h)</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 dark:divide-gray-700">
              {mockCryptoData.map((crypto) => (
                <tr key={crypto.id} className="hover:bg-gray-50 dark:hover:bg-gray-800">
                  <td className="px-4 py-4 whitespace-nowrap">
                    <div className="flex items-center">
                      <div className="w-8 h-8 rounded-full bg-gray-200 dark:bg-gray-700 flex items-center justify-center mr-3">
                        <span className="text-xs font-bold">{crypto.symbol.substring(0, 2)}</span>
                      </div>
                      <div>
                        <div className="font-medium">{crypto.name}</div>
                        <div className="text-gray-500 dark:text-gray-400 text-sm">{crypto.symbol}</div>
                      </div>
                    </div>
                  </td>
                  <td className="px-4 py-4 whitespace-nowrap text-right font-medium">
                    ${crypto.price.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                  </td>
                  <td className="px-4 py-4 whitespace-nowrap text-right">
                    <span className={crypto.change24h >= 0 ? 'text-green-600 dark:text-green-400' : 'text-red-600 dark:text-red-400'}>
                      {crypto.change24h >= 0 ? '+' : ''}{crypto.change24h}%
                    </span>
                  </td>
                  <td className="px-4 py-4 whitespace-nowrap text-right">
                    ${(crypto.marketCap / 1000000000).toFixed(2)}B
                  </td>
                  <td className="px-4 py-4 whitespace-nowrap text-right">
                    ${(crypto.volume24h / 1000000000).toFixed(2)}B
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        <div className="mt-6 text-center">
          <button className="btn-secondary">View All Cryptocurrencies</button>
        </div>
      </div>
    </div>
  );
}
