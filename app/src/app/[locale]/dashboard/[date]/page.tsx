import { Suspense } from 'react';
import { Metadata } from 'next';
import { getTranslations, unstable_setRequestLocale } from 'next-intl/server';

import { DashboardParams, Params } from '@/types';

import MonthlyChardCardSkeleton from './_monthly-charts/components/card-skeleton';
import PlannedTransactionCardSkeleton from './_planned-transactions-list/components/skeleton';
import MonthlySummaryLoading from './_summary/loading';
import TransactionCardSkeleton from './_transactions-list/components/skeleton';
import MonthlyCharts from './_monthly-charts';
import PlannedTransactionsList from './_planned-transactions-list';
import MonthlySummary from './_summary';
import TransactionsList from './_transactions-list';

export async function generateMetadata({
  params: { locale },
}: {
  params: Params;
}) {
  const t = await getTranslations({ locale, namespace: 'Metadata.Dashboard' });

  return {
    title: t('Title'),
  } as Metadata;
}

export default async function Dashboard({
  params: { date, locale },
}: {
  params: DashboardParams;
}) {
  unstable_setRequestLocale(locale);

  const year = parseInt(date.split('-')[0]);
  const month = parseInt(date.split('-')[1]);

  return (
    <>
      <Suspense fallback={<MonthlySummaryLoading />}>
        <MonthlySummary year={year} month={month} />
      </Suspense>
      <Suspense fallback={<PlannedTransactionCardSkeleton />}>
        <PlannedTransactionsList year={year} month={month} />
      </Suspense>
      <Suspense fallback={<TransactionCardSkeleton />}>
        <TransactionsList year={year} month={month} />
      </Suspense>
      <Suspense fallback={<MonthlyChardCardSkeleton />}>
        <MonthlyCharts year={year} month={month} />
      </Suspense>
    </>
  );
}
