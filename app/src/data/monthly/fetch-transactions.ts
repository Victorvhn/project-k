import { useQuery } from '@tanstack/react-query';

import { getMonthlyTransactions } from '@/services/monthly/get-monthly-transactions';
import { GetTransactionsRequest } from '@/types';

export function useFetchTransactions(request: GetTransactionsRequest) {
  return useQuery({
    queryFn: async () => getMonthlyTransactions(request),
    queryKey: ['monthly', 'transactions', request],
  });
}
