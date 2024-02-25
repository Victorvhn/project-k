import {
  dehydrate,
  HydrationBoundary,
  QueryClient,
} from '@tanstack/react-query';
import { Metadata } from 'next';
import { getTranslations, unstable_setRequestLocale } from 'next-intl/server';

import { fetchCategories } from '@/services/categories/get-all-paginated';
import { getMonthlyPlannedTransaction } from '@/services/planned-transactions/get-monthly-planned-transaction';
import { ActionType, MonthlyRequest, Params } from '@/types';

import PlannedTransactionsForm from '../../_components/form';

export async function generateMetadata({
  params: { locale },
}: {
  params: Params;
}) {
  const t = await getTranslations({
    locale,
    namespace: 'Metadata.PlannedTransactions.Edit',
  });

  return {
    title: t('Title'),
  } as Metadata;
}

export default async function EditPlannedTransaction({
  params: { locale, plannedTransactionId },
  searchParams: { year, month, actionType },
}: {
  params: Params & { plannedTransactionId: string };
  searchParams: MonthlyRequest & { actionType: ActionType };
}) {
  unstable_setRequestLocale(locale);
  const t = await getTranslations('Pages.PlannedTransactions.Edit');

  const queryClient = new QueryClient();
  const monthlyRequest = { year, month } as MonthlyRequest;

  await Promise.all([
    queryClient.prefetchQuery({
      queryKey: ['categories'],
      queryFn: () => fetchCategories(),
    }),
    queryClient.prefetchQuery({
      queryKey: ['planned-transaction', plannedTransactionId, monthlyRequest],
      queryFn: () =>
        getMonthlyPlannedTransaction(plannedTransactionId, monthlyRequest),
    }),
  ]);

  return (
    <>
      <h1 className='mb-2 scroll-m-20 text-2xl font-semibold tracking-tight'>
        {t('Title')}
      </h1>
      <p className='mb-8 leading-7'>{t('Description')}</p>
      <HydrationBoundary state={dehydrate(queryClient)}>
        <PlannedTransactionsForm
          action='update'
          plannedTransactionId={plannedTransactionId}
          monthlyRequest={monthlyRequest}
          updateType={actionType}
        />
      </HydrationBoundary>
    </>
  );
}
