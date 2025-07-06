import React from 'react';
import CryptoDetail from '@/components/CryptoDetail';
import { Suspense } from 'react';

export default function AnalysisPage() {
  return (
    <div className="container mx-auto py-8 px-4">
      <h1 className="text-3xl font-bold mb-6">Cryptocurrency Analysis</h1>
      <div className="bg-white rounded-lg shadow p-6">
        <Suspense fallback={<div className="text-center py-8">Loading...</div>}>
          <CryptoDetail />
        </Suspense>
      </div>
    </div>
  );
}
