import { Session } from 'next-auth';
import { NextIntlClientProvider, useMessages } from 'next-intl';

import { ThemeProvider } from '@/providers/theme-provider';
import { Locale } from '@/shared/supported-languages';

import { ClientProviders } from './client-providers';

export function ServerProviders({
  children,
  locale,
  session,
}: {
  children: React.ReactNode;
  locale: Locale;
  session: Session | null;
}) {
  const messages = useMessages();

  return (
    <>
      <NextIntlClientProvider locale={locale} messages={messages}>
        <ThemeProvider
          attribute='class'
          defaultTheme='system'
          enableSystem
          disableTransitionOnChange
        >
          <ClientProviders session={session}>{children}</ClientProviders>
        </ThemeProvider>
      </NextIntlClientProvider>
    </>
  );
}
