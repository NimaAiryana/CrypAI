import '../styles/globals.css'
import { Inter } from 'next/font/google'
import type { Metadata } from 'next'

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
      <body className={`${inter.variable} font-sans bg-gray-50 dark:bg-dark`}>
        {children}
      </body>
    </html>
  )
}
