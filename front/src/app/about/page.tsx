import React from 'react';

export default function AboutPage() {
  return (
    <div className="container mx-auto py-8 px-4">
      <h1 className="text-3xl font-bold mb-6">About Us</h1>
      <div className="bg-white rounded-lg shadow p-6">
        <h2 className="text-2xl font-semibold mb-4">CrypAI</h2>
        <p className="mb-4">
          CrypAI is an advanced cryptocurrency analysis platform that uses artificial intelligence
          to provide technical and fundamental analyses for investors.
        </p>
        <p className="mb-4">
          This platform leverages the Google Gemini API and market data from reliable sources
          to deliver accurate and up-to-date analyses.
        </p>
        <h3 className="text-xl font-semibold mt-6 mb-3">Key Features:</h3>
        <ul className="list-disc pl-6 mb-6">
          <li className="mb-2">Automated technical analysis using AI</li>
          <li className="mb-2">Fundamental analysis based on news and economic data</li>
          <li className="mb-2">Combined technical and fundamental analysis</li>
          <li className="mb-2">Access to cryptocurrency price and volume data</li>
          <li className="mb-2">Simple and user-friendly interface</li>
        </ul>
        <p>
          Our team consists of blockchain specialists, AI experts, and software engineers who developed
          this platform with the aim of simplifying investment decisions in the cryptocurrency market.
        </p>
      </div>
    </div>
  );
}
