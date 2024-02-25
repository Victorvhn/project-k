import {
  dehydrate,
  HydrationBoundary,
  QueryClient,
} from '@tanstack/react-query';
import { TrendingDownIcon, TrendingUpIcon } from 'lucide-react';

import { getSummary } from '@/services/monthly/get-summary';
import { TransactionType } from '@/types';

import SummaryCard from './components/card';

export default async function MonthlySummary({
  year,
  month,
}: {
  year: number;
  month: number;
}) {
  const queryClient = new QueryClient();

  const getExpenseSummaryRequest = {
    monthlyRequest: { year, month },
    transactionType: TransactionType.Expense,
  };
  const getIncomeSummaryRequest = {
    monthlyRequest: { year, month },
    transactionType: TransactionType.Income,
  };

  await Promise.all([
    queryClient.prefetchQuery({
      queryKey: ['monthly', 'summary', getExpenseSummaryRequest],
      queryFn: () => getSummary(getExpenseSummaryRequest),
    }),
    queryClient.prefetchQuery({
      queryKey: ['monthly', 'summary', getIncomeSummaryRequest],
      queryFn: () => getSummary(getIncomeSummaryRequest),
    }),
  ]);

  return (
    <HydrationBoundary state={dehydrate(queryClient)}>
      <SummaryCard
        request={getExpenseSummaryRequest}
        title='ExpensesTitle'
        percentageMessage='ExpensesPercentage'
        icon={<TrendingDownIcon className='h-4 w-4 text-muted-foreground' />}
      />
      <SummaryCard
        request={getIncomeSummaryRequest}
        title='IncomeTitle'
        percentageMessage='IncomePercentage'
        icon={<TrendingUpIcon className='h-4 w-4 text-muted-foreground' />}
      />
    </HydrationBoundary>
  );
}
