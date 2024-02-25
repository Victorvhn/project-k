'use client';

import { useEffect, useState, useTransition } from 'react';
import { useSearchParams } from 'next/navigation';
import { useLocale, useTranslations } from 'next-intl';
import * as NProgress from 'nprogress';

import { BrazilFlagIcon, UsaFlagIcon } from '@/assets';
import { usePathname, useRouter } from '@/lib/navigation';

import { Button } from './ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuRadioGroup,
  DropdownMenuRadioItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from './ui/dropdown-menu';
import { Skeleton } from './ui/skeleton';

export default function LanguageSwitch() {
  const t = useTranslations('Components.LanguageSwitch');
  const [mounted, setMounted] = useState(false);
  const pathname = usePathname();
  const searchParams = useSearchParams();
  const locale = useLocale();
  const router = useRouter();
  const [isPending, startTransition] = useTransition();

  useEffect(() => {
    setMounted(true);
  }, []);

  useEffect(() => {
    if (NProgress.isStarted()) {
      NProgress.done();
    }
  }, [pathname, searchParams]);

  function onSelectChange(value: string) {
    if (locale === value) {
      return;
    }
    startTransition(() => {
      NProgress.start();
      router.push(pathname, { locale: value });
    });
  }

  if (!mounted) {
    return <Skeleton className='h-10 w-10' />;
  }

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild className='h-10 w-10'>
        <Button
          variant='ghost'
          size='icon'
          disabled={isPending}
          aria-label='Switch language button'
        >
          {locale === 'en' ? <UsaFlagIcon /> : <BrazilFlagIcon />}
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent>
        <DropdownMenuLabel>{t('Title')}</DropdownMenuLabel>
        <DropdownMenuSeparator />
        <DropdownMenuRadioGroup value={locale} onValueChange={onSelectChange}>
          <DropdownMenuRadioItem value='en'>
            {t('English')}
          </DropdownMenuRadioItem>
          <DropdownMenuRadioItem value='pt'>
            {t('Portuguese')}
          </DropdownMenuRadioItem>
        </DropdownMenuRadioGroup>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
