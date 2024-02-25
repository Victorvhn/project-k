import { Metadata, type Viewport } from 'next';
import { Params } from 'next/dist/shared/lib/router/utils/route-matcher';
import { Inter } from 'next/font/google';
import { getServerSession } from 'next-auth';
import { getTranslations, unstable_setRequestLocale } from 'next-intl/server';
import NextTopLoader from 'nextjs-toploader';

import { Header } from '@/components/header';
import { Toaster } from '@/components/ui/toaster';
import { authOptions } from '@/lib/auth';
import { cn } from '@/lib/utils';
import { ServerProviders } from '@/providers';
import { locales } from '@/shared/supported-languages';

const inter = Inter({ subsets: ['latin'], variable: '--font-sans' });

export const viewport: Viewport = {
  width: 'device-width',
  initialScale: 1,
  maximumScale: 1,
  userScalable: false,
  viewportFit: 'cover',
  themeColor: [
    { media: '(prefers-color-scheme: dark)', color: '#27272A' },
    { media: '(prefers-color-scheme: light)', color: '#FFFFFF' },
  ],
};

export async function generateMetadata({
  params: { locale },
}: {
  params: Params;
}) {
  const t = await getTranslations({ locale, namespace: 'Metadata' });

  return {
    manifest: '/manifest.json',
    title: t('Title'),
    description: t('Description'),
    appleWebApp: {
      capable: true,
      title: t('Title'),
      statusBarStyle: 'default',
      startupImage: [
        {
          url: '/splash_screens/iPhone_15_Pro_Max__iPhone_15_Plus__iPhone_14_Pro_Max_landscape.png',
          media:
            'screen and (device-width: 430px) and (device-height: 932px) and (-webkit-device-pixel-ratio: 3) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/iPhone_15_Pro__iPhone_15__iPhone_14_Pro_landscape.png',
          media:
            'screen and (device-width: 393px) and (device-height: 852px) and (-webkit-device-pixel-ratio: 3) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/iPhone_14_Plus__iPhone_13_Pro_Max__iPhone_12_Pro_Max_landscape.png',
          media:
            'screen and (device-width: 428px) and (device-height: 926px) and (-webkit-device-pixel-ratio: 3) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/iPhone_14__iPhone_13_Pro__iPhone_13__iPhone_12_Pro__iPhone_12_landscape.png',
          media:
            'screen and (device-width: 390px) and (device-height: 844px) and (-webkit-device-pixel-ratio: 3) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/iPhone_13_mini__iPhone_12_mini__iPhone_11_Pro__iPhone_XS__iPhone_X_landscape.png',
          media:
            'screen and (device-width: 375px) and (device-height: 812px) and (-webkit-device-pixel-ratio: 3) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/iPhone_11_Pro_Max__iPhone_XS_Max_landscape.png',
          media:
            'screen and (device-width: 414px) and (device-height: 896px) and (-webkit-device-pixel-ratio: 3) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/iPhone_11__iPhone_XR_landscape.png',
          media:
            'screen and (device-width: 414px) and (device-height: 896px) and (-webkit-device-pixel-ratio: 2) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/iPhone_8_Plus__iPhone_7_Plus__iPhone_6s_Plus__iPhone_6_Plus_landscape.png',
          media:
            'screen and (device-width: 414px) and (device-height: 736px) and (-webkit-device-pixel-ratio: 3) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/iPhone_8__iPhone_7__iPhone_6s__iPhone_6__4.7__iPhone_SE_landscape.png',
          media:
            'screen and (device-width: 375px) and (device-height: 667px) and (-webkit-device-pixel-ratio: 2) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/4__iPhone_SE__iPod_touch_5th_generation_and_later_landscape.png',
          media:
            'screen and (device-width: 320px) and (device-height: 568px) and (-webkit-device-pixel-ratio: 2) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/12.9__iPad_Pro_landscape.png',
          media:
            'screen and (device-width: 1024px) and (device-height: 1366px) and (-webkit-device-pixel-ratio: 2) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/11__iPad_Pro__10.5__iPad_Pro_landscape.png',
          media:
            'screen and (device-width: 834px) and (device-height: 1194px) and (-webkit-device-pixel-ratio: 2) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/10.9__iPad_Air_landscape.png',
          media:
            'screen and (device-width: 820px) and (device-height: 1180px) and (-webkit-device-pixel-ratio: 2) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/10.5__iPad_Air_landscape.png',
          media:
            'screen and (device-width: 834px) and (device-height: 1112px) and (-webkit-device-pixel-ratio: 2) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/10.2__iPad_landscape.png',
          media:
            'screen and (device-width: 810px) and (device-height: 1080px) and (-webkit-device-pixel-ratio: 2) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/9.7__iPad_Pro__7.9__iPad_mini__9.7__iPad_Air__9.7__iPad_landscape.png',
          media:
            'screen and (device-width: 768px) and (device-height: 1024px) and (-webkit-device-pixel-ratio: 2) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/8.3__iPad_Mini_landscape.png',
          media:
            'screen and (device-width: 744px) and (device-height: 1133px) and (-webkit-device-pixel-ratio: 2) and (orientation: landscape)',
        },
        {
          url: '/splash_screens/iPhone_15_Pro_Max__iPhone_15_Plus__iPhone_14_Pro_Max_portrait.png',
          media:
            'screen and (device-width: 430px) and (device-height: 932px) and (-webkit-device-pixel-ratio: 3) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/iPhone_15_Pro__iPhone_15__iPhone_14_Pro_portrait.png',
          media:
            'screen and (device-width: 393px) and (device-height: 852px) and (-webkit-device-pixel-ratio: 3) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/iPhone_14_Plus__iPhone_13_Pro_Max__iPhone_12_Pro_Max_portrait.png',
          media:
            'screen and (device-width: 428px) and (device-height: 926px) and (-webkit-device-pixel-ratio: 3) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/iPhone_14__iPhone_13_Pro__iPhone_13__iPhone_12_Pro__iPhone_12_portrait.png',
          media:
            'screen and (device-width: 390px) and (device-height: 844px) and (-webkit-device-pixel-ratio: 3) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/iPhone_13_mini__iPhone_12_mini__iPhone_11_Pro__iPhone_XS__iPhone_X_portrait.png',
          media:
            'screen and (device-width: 375px) and (device-height: 812px) and (-webkit-device-pixel-ratio: 3) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/iPhone_11_Pro_Max__iPhone_XS_Max_portrait.png',
          media:
            'screen and (device-width: 414px) and (device-height: 896px) and (-webkit-device-pixel-ratio: 3) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/iPhone_11__iPhone_XR_portrait.png',
          media:
            'screen and (device-width: 414px) and (device-height: 896px) and (-webkit-device-pixel-ratio: 2) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/iPhone_8_Plus__iPhone_7_Plus__iPhone_6s_Plus__iPhone_6_Plus_portrait.png',
          media:
            'screen and (device-width: 414px) and (device-height: 736px) and (-webkit-device-pixel-ratio: 3) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/iPhone_8__iPhone_7__iPhone_6s__iPhone_6__4.7__iPhone_SE_portrait.png',
          media:
            'screen and (device-width: 375px) and (device-height: 667px) and (-webkit-device-pixel-ratio: 2) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/4__iPhone_SE__iPod_touch_5th_generation_and_later_portrait.png',
          media:
            'screen and (device-width: 320px) and (device-height: 568px) and (-webkit-device-pixel-ratio: 2) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/12.9__iPad_Pro_portrait.png',
          media:
            'screen and (device-width: 1024px) and (device-height: 1366px) and (-webkit-device-pixel-ratio: 2) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/11__iPad_Pro__10.5__iPad_Pro_portrait.png',
          media:
            'screen and (device-width: 834px) and (device-height: 1194px) and (-webkit-device-pixel-ratio: 2) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/10.9__iPad_Air_portrait.png',
          media:
            'screen and (device-width: 820px) and (device-height: 1180px) and (-webkit-device-pixel-ratio: 2) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/10.5__iPad_Air_portrait.png',
          media:
            'screen and (device-width: 834px) and (device-height: 1112px) and (-webkit-device-pixel-ratio: 2) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/10.2__iPad_portrait.png',
          media:
            'screen and (device-width: 810px) and (device-height: 1080px) and (-webkit-device-pixel-ratio: 2) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/9.7__iPad_Pro__7.9__iPad_mini__9.7__iPad_Air__9.7__iPad_portrait.png',
          media:
            'screen and (device-width: 768px) and (device-height: 1024px) and (-webkit-device-pixel-ratio: 2) and (orientation: portrait)',
        },
        {
          url: '/splash_screens/8.3__iPad_Mini_portrait.png',
          media:
            'screen and (device-width: 744px) and (device-height: 1133px) and (-webkit-device-pixel-ratio: 2) and (orientation: portrait)',
        },
      ],
    },
    icons: {
      icon: [
        {
          rel: 'apple-touch-icon',
          url: '/icons/maskable-192.png',
          sizes: '192x192',
          type: 'image/png',
        },
        {
          rel: 'apple-touch-icon',
          url: '/icons/maskable-256.png',
          sizes: '256x256',
          type: 'image/png',
        },
        {
          rel: 'apple-touch-icon',
          url: '/icons/maskable-384.png',
          sizes: '384x384',
          type: 'image/png',
        },
        {
          rel: 'apple-touch-icon',
          url: '/icons/maskable-512.png',
          sizes: '512x512',
          type: 'image/png',
        },
      ],
    },
  } as Metadata;
}

export function generateStaticParams() {
  return locales.map((locale) => ({ locale }));
}

export default async function RootLayout({
  children,
  params: { locale },
}: {
  children: React.ReactNode;
  params: Params;
}) {
  unstable_setRequestLocale(locale);
  const session = await getServerSession(authOptions);

  return (
    <html lang={locale} suppressHydrationWarning>
      <body
        className={cn(
          'min-h-screen bg-background font-sans antialiased',
          inter.variable
        )}
      >
        <ServerProviders locale={locale} session={session}>
          <NextTopLoader />
          <Header />
          <main className='p-12 pt-10'>{children}</main>
          <Toaster />
        </ServerProviders>
      </body>
    </html>
  );
}
