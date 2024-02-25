'use client';

import { LogOutIcon } from 'lucide-react';
import { signOut } from 'next-auth/react';
import { useTranslations } from 'next-intl';

import { Button } from './ui/button';
import { Tooltip, TooltipContent, TooltipTrigger } from './ui/tooltip';

export default function SignOutButton() {
  const t = useTranslations('Components.SignOutButton');

  return (
    <Tooltip>
      <TooltipTrigger asChild>
        <Button
          variant='ghost'
          size='icon'
          onClick={() => signOut({ callbackUrl: '/' })}
          aria-label='Sign out button'
        >
          <LogOutIcon />
        </Button>
      </TooltipTrigger>
      <TooltipContent align='end'>
        <p>{t('Tooltip')}</p>
      </TooltipContent>
    </Tooltip>
  );
}
