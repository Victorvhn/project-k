'use client';

import React from 'react';
import { useTranslations } from 'next-intl';

import DefaultError from '@/components/default-error';
import { Card, CardHeader, CardTitle } from '@/components/ui/card';

type TransactionsCardError = {
  failureReason: Error | null;
};

export default function TransactionsCardError({
  failureReason,
}: TransactionsCardError) {
  const t = useTranslations('Pages.Dashboard.TransactionsList');

  return (
    <Card>
      <CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
        <CardTitle className='text-md font-medium'>{t('Title')}</CardTitle>
      </CardHeader>
      <DefaultError failureReason={failureReason} className='h-[330px]' />
    </Card>
  );
}
