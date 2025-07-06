"use client";

import { useState } from 'react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';

export default function Header() {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const pathname = usePathname();
  
  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  return (
    <header className="bg-white dark:bg-gray-800 shadow-sm sticky top-0 z-50">
      <div className="container-custom mx-auto">
        <div className="flex justify-between items-center py-4">
          {/* Logo */}
          <Link href="/" className="flex items-center">
            <span className="text-primary-600 dark:text-primary-400 font-bold text-2xl">CrypAI</span>
          </Link>

          {/* Desktop Navigation */}
          <nav className="hidden md:flex space-x-10">
            <NavLink href="/" label="Home" isActive={pathname === '/'} />
            <NavLink href="/analysis" label="Analysis" isActive={pathname === '/analysis'} />
            <NavLink href="/dashboard" label="Dashboard" isActive={pathname === '/dashboard'} />
            <NavLink href="/about" label="About" isActive={pathname === '/about'} />
          </nav>

          {/* Auth Buttons - Desktop */}
          <div className="hidden md:flex items-center space-x-4">
            <button className="text-gray-600 dark:text-gray-300 hover:text-primary-600 dark:hover:text-primary-400 font-medium">
              Sign In
            </button>
            <button className="btn-primary">
              Sign Up Free
            </button>
          </div>

          {/* Mobile Menu Button */}
          <button
            className="md:hidden text-gray-600 dark:text-gray-300 hover:text-primary-600 dark:hover:text-primary-400"
            onClick={toggleMenu}
          >
            {isMenuOpen ? (
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" className="w-6 h-6">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
              </svg>
            ) : (
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" className="w-6 h-6">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
              </svg>
            )}
          </button>
        </div>

        {/* Mobile Navigation Menu */}
        {isMenuOpen && (
          <div className="md:hidden py-4 border-t border-gray-100 dark:border-gray-700">
            <nav className="flex flex-col space-y-4">
              <NavLink href="/" label="Home" isActive={pathname === '/'} />
              <NavLink href="/analysis" label="Analysis" isActive={pathname === '/analysis'} />
              <NavLink href="/dashboard" label="Dashboard" isActive={pathname === '/dashboard'} />
              <NavLink href="/about" label="About" isActive={pathname === '/about'} />
              <div className="flex flex-col space-y-2 pt-4 border-t border-gray-100 dark:border-gray-700">
                <button className="text-gray-600 dark:text-gray-300 hover:text-primary-600 dark:hover:text-primary-400 font-medium">
                  Sign In
                </button>
                <button className="btn-primary">
                  Sign Up Free
                </button>
              </div>
            </nav>
          </div>
        )}
      </div>
    </header>
  );
}

interface NavLinkProps {
  href: string;
  label: string;
  isActive: boolean;
}

function NavLink({ href, label, isActive }: NavLinkProps) {
  return (
    <Link 
      href={href} 
      className={`${
        isActive 
          ? 'text-primary-600 dark:text-primary-400 font-medium' 
          : 'text-gray-600 dark:text-gray-300 hover:text-primary-600 dark:hover:text-primary-400'
      } transition-colors duration-200`}
    >
      {label}
    </Link>
  );
}
