import '../styles/globals.css'
import { Inter } from 'next/font/google'
import type { Metadata } from 'next'
import Header from '../components/Header'
import Footer from '../components/Footer'

const inter = Inter({ subsets: ['latin'], variable: '--font-inter' })

export const metadata: Metadata = {
  title: 'CrypAI - Advanced Crypto Analysis',
  description: 'AI-powered cryptocurrency analysis platform combining technical and fundamental data',
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html lang="en" className="scroll-smooth">
      <body className={`${inter.variable} font-sans bg-gray-50 dark:bg-dark flex flex-col min-h-screen`}>
        <Header />
        <main className="flex-grow container-custom mx-auto px-4 py-8">
          {children}
        </main>
        <Footer />  
      </body>
    </html>
  )
}
