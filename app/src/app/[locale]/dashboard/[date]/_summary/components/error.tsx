'use client';

import React from 'react';
import { useTranslations } from 'next-intl';

import DefaultError from '@/components/default-error';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';

type SummaryCardErrorProps = {
  failureReason: Error | null;
  title: 'ExpensesTitle' | 'IncomeTitle';
  icon: React.ReactNode;
};

export default function SummaryCardError({
  failureReason,
  title,
  icon,
}: SummaryCardErrorProps) {
  const t = useTranslations('Pages.Dashboard.MonthlySummary');

  return (
    <Card>
      <CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
        <CardTitle className='text-md font-medium'>{t(title)}</CardTitle>
        {icon}
      </CardHeader>
      <CardContent>
        <DefaultError failureReason={failureReason} className='h-24' />
      </CardContent>
    </Card>
  );
}
