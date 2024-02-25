'use client';

import { useEffect, useMemo, useState } from 'react';
import { Loader2 } from 'lucide-react';
import { useSession } from 'next-auth/react';
import { useTranslations } from 'next-intl';

import { BrazilFlagIcon, UsaFlagIcon } from '@/assets';
import { useUpdateCurrency } from '@/data/users/use-update-currency';
import { Currency } from '@/types/enums/currency';

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

export default function ChangeCurrency() {
  const [mounted, setMounted] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const { data } = useSession();
  const t = useTranslations('Components.CurrencySwitch');

  const mutate = useUpdateCurrency(data?.user_id!);

  useEffect(() => {
    if (data) {
      setMounted(true);
    }
  }, [data]);

  const buttonChildren = useMemo(() => {
    if (isLoading) {
      return (
        <>
          <Loader2 className='mr-2 h-4 w-4 animate-spin' />
          {t('Changing')}
        </>
      );
    }

    if (data!.preferred_currency.toUpperCase() === 'USD') {
      return (
        <>
          <UsaFlagIcon className='mr-2' />
          {t('USD')}
        </>
      );
    } else {
      return (
        <>
          <BrazilFlagIcon className='mr-2' />
          {t('BRL')}
        </>
      );
    }
  }, [data, isLoading, t]);

  if (!mounted) {
    return <Skeleton className='h-10 w-56' />;
  }

  const handleChangeCurrency = (value: string) => {
    const currency = value as Currency;
    if (currency.toUpperCase() === data!.preferred_currency) {
      return;
    }

    setIsLoading(true);
    mutate.mutate({ currency }, { onSettled: () => setIsLoading(false) });
  };

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant='outline' className='justify-start'>
          {buttonChildren}
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent side='right'>
        <DropdownMenuLabel>{t('Title')}</DropdownMenuLabel>
        <DropdownMenuSeparator />
        <DropdownMenuRadioGroup
          value={data!.preferred_currency}
          onValueChange={handleChangeCurrency}
        >
          <DropdownMenuRadioItem value={Currency.BRL.toUpperCase()}>
            {t('BRL')}
          </DropdownMenuRadioItem>
          <DropdownMenuRadioItem value={Currency.USD.toUpperCase()}>
            {t('USD')}
          </DropdownMenuRadioItem>
        </DropdownMenuRadioGroup>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
