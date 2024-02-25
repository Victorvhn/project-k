import {
  dehydrate,
  HydrationBoundary,
  QueryClient,
} from '@tanstack/react-query';
import { Metadata } from 'next';
import { getTranslations, unstable_setRequestLocale } from 'next-intl/server';

import { fetchCategories } from '@/services/categories/get-all-paginated';
import { MonthlyRequest, Params } from '@/types';

import PlannedTransactionsForm from '../_components/form';

export async function generateMetadata({
  params: { locale },
}: {
  params: Params;
}) {
  const t = await getTranslations({
    locale,
    namespace: 'Metadata.PlannedTransactions.Add',
  });

  return {
    title: t('Title'),
  } as Metadata;
}

export default async function AddPlannedTransaction({
  params: { locale },
  searchParams,
}: {
  params: Params;
  searchParams: MonthlyRequest;
}) {
  unstable_setRequestLocale(locale);
  const t = await getTranslations('Pages.PlannedTransactions.Add');

  const queryClient = new QueryClient();

  await Promise.all([
    queryClient.prefetchQuery({
      queryKey: ['categories'],
      queryFn: () => fetchCategories(),
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
          action='create'
          monthlyRequest={searchParams}
        />
      </HydrationBoundary>
    </>
  );
}
