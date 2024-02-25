'use client';

import React from 'react';
import { useSession } from 'next-auth/react';
import { useFormatter, useTranslations } from 'next-intl';

import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { useFetchSummary } from '@/data/monthly/fetch-summary';
import { GetSummaryRequest } from '@/types';

import SummaryCardError from './error';

type SummaryCardProps = {
  request: GetSummaryRequest;
  title: 'ExpensesTitle' | 'IncomeTitle';
  percentageMessage: 'ExpensesPercentage' | 'IncomePercentage';
  icon: React.ReactNode;
};

export default function SummaryCard({
  request,
  title,
  percentageMessage,
  icon,
}: SummaryCardProps) {
  const session = useSession();
  const { data, isSuccess, failureReason } = useFetchSummary(request);
  const t = useTranslations('Pages.Dashboard.MonthlySummary');
  const format = useFormatter();

  if (!isSuccess) {
    return (
      <SummaryCardError
        title={title}
        icon={icon}
        failureReason={failureReason as Error}
      />
    );
  }

  const currentAmountFomatted = format.number(data.current, {
    style: 'currency',
    currency: session.data?.preferred_currency,
  });

  const budgetFomatted = format.number(data.expected, {
    style: 'currency',
    currency: session.data?.preferred_currency,
  });

  const percentageFomatted = format.number(
    data.percentage / 100 < 0
      ? (data.percentage / 100) * -1
      : data.percentage / 100,
    {
      style: 'percent',
      maximumFractionDigits: 2,
    }
  );

  return (
    <Card>
      <CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
        <CardTitle className='text-md font-medium'>{t(title)}</CardTitle>
        {icon}
      </CardHeader>
      <CardContent>
        <div className='text-2xl font-bold'>{currentAmountFomatted}</div>
        <p className='text-sm text-muted-foreground'>
          {t('BudgetMessage', {
            budget: budgetFomatted,
          })}
        </p>
        <p className='text-sm text-muted-foreground'>
          {t(percentageMessage, {
            percentage: percentageFomatted,
            difference: data.difference,
          })}
        </p>
      </CardContent>
    </Card>
  );
}
