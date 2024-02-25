import {
  dehydrate,
  HydrationBoundary,
  QueryClient,
} from '@tanstack/react-query';

import { getMonthlyTransactions } from '@/services/monthly/get-monthly-transactions';

import TransactionsCard from './components/card';

export default async function TransactionsList({
  year,
  month,
}: {
  year: number;
  month: number;
}) {
  const queryClient = new QueryClient();

  const getMonthlyTransactionsRequest = {
    year,
    month,
  };

  await queryClient.prefetchQuery({
    queryKey: ['monthly', 'transactions', getMonthlyTransactionsRequest],
    queryFn: () => getMonthlyTransactions(getMonthlyTransactionsRequest),
  });

  return (
    <HydrationBoundary state={dehydrate(queryClient)}>
      <TransactionsCard request={getMonthlyTransactionsRequest} />
    </HydrationBoundary>
  );
}
