'use client';

import { useMemo, useState } from 'react';
import { GithubIcon, Loader2 } from 'lucide-react';
import { signIn } from 'next-auth/react';
import { useTranslations } from 'next-intl';

import { GoogleIcon } from '@/assets';

import { Button } from './ui/button';

export default function SignInButton({
  provider,
  callbackUrl,
}: {
  provider: 'google' | 'github';
  callbackUrl: string;
}) {
  const t = useTranslations('Components.SignInButton');
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const icon = useMemo(() => {
    if (isLoading) {
      return <Loader2 className='mr-2 h-4 w-4 animate-spin' />;
    }

    switch (provider) {
      case 'google':
        return <GoogleIcon />;
      case 'github':
        return <GithubIcon className='mr-2 h-4 w-4' />;
    }
  }, [provider, isLoading]);

  const text = useMemo(() => {
    switch (provider) {
      case 'google':
        return t('Message', { provider: 'Google' });
      case 'github':
        return t('Message', { provider: 'Github' });
    }
  }, [provider, t]);

  return (
    <Button
      variant='outline'
      onClick={async () => {
        setIsLoading(true);
        await signIn(provider, { redirect: true, callbackUrl });
      }}
      disabled={isLoading}
      aria-label={text}
    >
      {icon}
      {text}
    </Button>
  );
}
