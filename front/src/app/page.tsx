"use client";

import { useState } from 'react';
import Image from 'next/image';
import Link from 'next/link';
import CryptoAnalysisCard from '@/components/CryptoAnalysisCard';
import MarketOverview from '@/components/MarketOverview';

export default function Home() {
  const [activeTab, setActiveTab] = useState<'technical' | 'fundamental'>('technical');
  
  return (
    <>
      <div>
        {/* Hero Section */}
        <section className="bg-gradient-to-r from-primary-600 to-secondary-600 text-white py-20">
          <div className="container-custom">
            <div className="flex flex-col md:flex-row items-center justify-between">
              <div className="md:w-1/2 mb-10 md:mb-0">
                <h1 className="text-4xl md:text-5xl lg:text-6xl font-extrabold mb-4">
                  CrypAI
                </h1>
                <p className="text-xl md:text-2xl mb-8">
                  Advanced Cryptocurrency Analysis Powered by Artificial Intelligence
                </p>
                <div className="flex flex-col sm:flex-row gap-4">
                  <button className="btn-primary bg-white text-primary-600 hover:bg-gray-100">
                    Get Started
                  </button>
                  <button className="btn-secondary bg-transparent border-2 border-white hover:bg-white/10">
                    Learn More
                  </button>
                </div>
              </div>
              <div className="md:w-1/2 flex justify-center">
                <div className="relative w-72 h-72 md:w-96 md:h-96">
                  {/* Replace with your hero image */}
                  <div className="w-full h-full rounded-full bg-white/20 flex items-center justify-center">
                    <div className="text-6xl font-bold">CrypAI</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>

        {/* Market Overview Section */}
        <section className="py-16 bg-gray-50 dark:bg-gray-900">
          <div className="container-custom">
            <h2 className="text-3xl font-bold mb-10 text-center">Market Overview</h2>
            <MarketOverview />
          </div>
        </section>

        {/* Analysis Types Section */}
        <section className="py-16">
          <div className="container-custom">
            <h2 className="text-3xl font-bold mb-10 text-center">AI-Powered Analysis</h2>
            
            {/* Tab Navigation */}
            <div className="flex justify-center mb-10">
              <div className="inline-flex rounded-md shadow-sm" role="group">
                <button 
                  type="button" 
                  className={`px-6 py-3 text-sm font-medium rounded-l-lg ${
                    activeTab === 'technical' 
                      ? 'bg-primary-600 text-white' 
                      : 'bg-white text-gray-700 hover:bg-gray-50'
                  }`}
                  onClick={() => setActiveTab('technical')}
                >
                  Technical Analysis
                </button>
                <button 
                  type="button" 
                  className={`px-6 py-3 text-sm font-medium rounded-r-lg ${
                    activeTab === 'fundamental' 
                      ? 'bg-primary-600 text-white' 
                      : 'bg-white text-gray-700 hover:bg-gray-50'
                  }`}
                  onClick={() => setActiveTab('fundamental')}
                >
                  Fundamental Analysis
                </button>
              </div>
            </div>
            
            {/* Tab Content */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
              {activeTab === 'technical' ? (
                <>
                  <CryptoAnalysisCard 
                    title="Price Pattern Recognition" 
                    description="Our AI identifies common chart patterns and predicts potential price movements based on historical data."
                    icon="📈"
                  />
                  <CryptoAnalysisCard 
                    title="Trend Analysis" 
                    description="Get insights on current market trends, support and resistance levels, and momentum indicators."
                    icon="📊"
                  />
                  <CryptoAnalysisCard 
                    title="Volatility Prediction" 
                    description="AI models that forecast potential market volatility and suggest risk management strategies."
                    icon="🔮"
                  />
                </>
              ) : (
                <>
                  <CryptoAnalysisCard 
                    title="Project Evaluation" 
                    description="In-depth analysis of cryptocurrency projects, assessing team, technology, and market potential."
                    icon="🔍"
                  />
                  <CryptoAnalysisCard 
                    title="Market Sentiment" 
                    description="AI-powered analysis of social media, news, and community sentiment affecting cryptocurrency prices."
                    icon="📰"
                  />
                  <CryptoAnalysisCard 
                    title="On-Chain Analytics" 
                    description="Detailed examination of blockchain data to identify whale movements, network activity, and adoption metrics."
                    icon="⛓️"
                  />
                </>
              )}
            </div>
          </div>
        </section>

        {/* Features Section */}
        <section className="py-16 bg-gray-50 dark:bg-gray-900">
          <div className="container-custom">
            <h2 className="text-3xl font-bold mb-10 text-center">Why Choose CrypAI</h2>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
              <div className="card p-6 text-center hover:shadow-xl transition-shadow duration-300">
                <div className="bg-primary-100 dark:bg-primary-900 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4">
                  <span className="text-2xl">🧠</span>
                </div>
                <h3 className="text-xl font-bold mb-2">Advanced AI</h3>
                <p className="text-gray-600 dark:text-gray-300">Powered by cutting-edge machine learning algorithms</p>
              </div>
              
              <div className="card p-6 text-center hover:shadow-xl transition-shadow duration-300">
                <div className="bg-primary-100 dark:bg-primary-900 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4">
                  <span className="text-2xl">⚡</span>
                </div>
                <h3 className="text-xl font-bold mb-2">Real-Time Data</h3>
                <p className="text-gray-600 dark:text-gray-300">Instant analysis of market movements as they happen</p>
              </div>
              
              <div className="card p-6 text-center hover:shadow-xl transition-shadow duration-300">
                <div className="bg-primary-100 dark:bg-primary-900 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4">
                  <span className="text-2xl">🔒</span>
                </div>
                <h3 className="text-xl font-bold mb-2">Secure Platform</h3>
                <p className="text-gray-600 dark:text-gray-300">Your data is protected with enterprise-grade security</p>
              </div>
              
              <div className="card p-6 text-center hover:shadow-xl transition-shadow duration-300">
                <div className="bg-primary-100 dark:bg-primary-900 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4">
                  <span className="text-2xl">🌐</span>
                </div>
                <h3 className="text-xl font-bold mb-2">Global Coverage</h3>
                <p className="text-gray-600 dark:text-gray-300">Analysis of cryptocurrencies from all major exchanges</p>
              </div>
            </div>
          </div>
        </section>

        {/* CTA Section */}
        <section className="py-16 bg-gradient-to-r from-secondary-600 to-primary-600 text-white">
          <div className="container-custom text-center">
            <h2 className="text-3xl md:text-4xl font-bold mb-4">Ready to Level Up Your Crypto Analysis?</h2>
            <p className="text-xl mb-8 max-w-3xl mx-auto">
              Join thousands of traders who are already using CrypAI to make smarter investment decisions
            </p>
            <button className="btn-primary bg-white text-primary-600 hover:bg-gray-100 px-8 py-3 text-lg">
              Get Started For Free
            </button>
          </div>
        </section>
      </div>
    </>
  );
}
