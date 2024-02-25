import {
  dehydrate,
  HydrationBoundary,
  QueryClient,
} from '@tanstack/react-query';
import { Metadata } from 'next';
import { getTranslations, unstable_setRequestLocale } from 'next-intl/server';

import { fetchCategories } from '@/services/categories/get-all-paginated';
import { getAllPlannedTransactions } from '@/services/planned-transactions/get-all';
import { getTransactionById } from '@/services/transactions/get-transaction-by-id';
import { MonthlyRequest, Params } from '@/types';

import TransactionsForm from '../../_components/form';

export async function generateMetadata({
  params: { locale },
}: {
  params: Params;
}) {
  const t = await getTranslations({
    locale,
    namespace: 'Metadata.Transactions.Edit',
  });

  return {
    title: t('Title'),
  } as Metadata;
}

export default async function EditTransaction({
  params: { locale, transactionId },
  searchParams,
}: {
  params: Params & { transactionId: string };
  searchParams: MonthlyRequest;
}) {
  unstable_setRequestLocale(locale);
  const t = await getTranslations('Pages.Transactions.Edit');

  const queryClient = new QueryClient();

  await Promise.all([
    queryClient.prefetchQuery({
      queryKey: ['categories'],
      queryFn: () => fetchCategories(),
    }),
    queryClient.prefetchQuery({
      queryKey: ['planned-transactions'],
      queryFn: () => getAllPlannedTransactions(),
    }),
    queryClient.prefetchQuery({
      queryKey: ['transaction', transactionId],
      queryFn: () => getTransactionById(transactionId),
    }),
  ]);

  return (
    <>
      <h1 className='mb-2 scroll-m-20 text-2xl font-semibold tracking-tight'>
        {t('Title')}
      </h1>
      <p className='mb-8 leading-7'>{t('Description')}</p>
      <HydrationBoundary state={dehydrate(queryClient)}>
        <TransactionsForm
          action='update'
          transactionId={transactionId}
          monthlyRequest={searchParams}
        />
      </HydrationBoundary>
    </>
  );
}
