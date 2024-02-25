'use client';

import { useEffect, useState } from 'react';
import { MoonIcon, SunIcon, SunMoonIcon } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { useTheme } from 'next-themes';

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

export default function ThemeSwitch() {
  const t = useTranslations('Components.ThemeSwitch');
  const [mounted, setMounted] = useState(false);
  const { theme, setTheme } = useTheme();

  useEffect(() => {
    setMounted(true);
  }, []);

  if (!mounted) {
    return <Skeleton className='h-10 w-10' />;
  }

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild className='h-10 w-10'>
        <Button variant='ghost' size='icon' aria-label='Switch theme button'>
          {theme === 'system' ? (
            <SunMoonIcon />
          ) : theme === 'dark' ? (
            <MoonIcon />
          ) : (
            <SunIcon />
          )}
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent>
        <DropdownMenuLabel>{t('Title')}</DropdownMenuLabel>
        <DropdownMenuSeparator />
        <DropdownMenuRadioGroup value={theme} onValueChange={setTheme}>
          <DropdownMenuRadioItem value='system'>
            {t('System')}
          </DropdownMenuRadioItem>
          <DropdownMenuRadioItem value='light'>
            {t('Light')}
          </DropdownMenuRadioItem>
          <DropdownMenuRadioItem value='dark'>
            {t('Dark')}
          </DropdownMenuRadioItem>
        </DropdownMenuRadioGroup>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
