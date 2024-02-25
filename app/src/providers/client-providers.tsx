'use client';

import { useState } from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { Session } from 'next-auth';
import { SessionProvider } from 'next-auth/react';

import RefreshTokenHandler from '@/components/refresh-token-handler';
import { ToastProvider } from '@/components/ui/toast';
import { TooltipProvider } from '@/components/ui/tooltip';
import { useMediaQuery } from '@/hooks/use-media-query';

export function ClientProviders({
  children,
  session,
}: {
  children: React.ReactNode;
  session: Session | null;
}) {
  const [queryClient] = useState(
    () =>
      new QueryClient({
        defaultOptions: {
          queries: {
            refetchOnWindowFocus: false,
            staleTime: 6 * 1000,
            refetchInterval: 30 * 1000,
            retry: 2,
            retryDelay: (attemptIndex) =>
              Math.min(1000 * 2 ** attemptIndex, 10000),
          },
        },
      })
  );
  const [refetchInterval, setRefetchInterval] = useState(0);
  const isSmallDevice = useMediaQuery('only screen and (max-width : 768px)');

  return (
    <>
      <SessionProvider session={session} refetchInterval={refetchInterval}>
        <QueryClientProvider client={queryClient}>
          <ReactQueryDevtools initialIsOpen={false} />
          <ToastProvider swipeDirection={isSmallDevice ? 'down' : 'right'}>
            <TooltipProvider delayDuration={270}>{children}</TooltipProvider>
          </ToastProvider>
          <RefreshTokenHandler setRefetchInterval={setRefetchInterval} />
        </QueryClientProvider>
      </SessionProvider>
    </>
  );
}
