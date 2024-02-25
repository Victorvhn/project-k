import {
  dehydrate,
  HydrationBoundary,
  QueryClient,
} from '@tanstack/react-query';

import { getMonthlyPlannedTransactions } from '@/services/monthly/get-monthly-planned-transactions';
import { MonthlyRequest } from '@/types';

import PlannedTransactionsCard from './components/card';

export default async function PlannedTransactionsList({
  year,
  month,
}: MonthlyRequest) {
  const queryClient = new QueryClient();

  const getMonthlyPlannedTransactionsRequest = {
    year,
    month,
  };

  await queryClient.prefetchQuery({
    queryKey: [
      'monthly',
      'planned-transactions',
      getMonthlyPlannedTransactionsRequest,
    ],
    queryFn: () =>
      getMonthlyPlannedTransactions(getMonthlyPlannedTransactionsRequest),
  });

  return (
    <HydrationBoundary state={dehydrate(queryClient)}>
      <PlannedTransactionsCard
        request={getMonthlyPlannedTransactionsRequest}
        monthlyRequest={{ year, month }}
      />
    </HydrationBoundary>
  );
}
