/** @type {import('next').NextConfig} */

const isProd = process.env.NODE_ENV === 'production';

const withNextIntl = require('next-intl/plugin')();
const runtimeCaching = require('next-pwa/cache');
const withPWA = require('next-pwa')({
  dest: 'public',
  register: true,
  skipWaiting: true,
  runtimeCaching,
  disable: !isProd,
  reloadOnOnline: true,
  cacheOnFrontEndNav: true,
});

const nextConfig = {
  reactStrictMode: true,
};

module.exports = withPWA(withNextIntl(nextConfig));
