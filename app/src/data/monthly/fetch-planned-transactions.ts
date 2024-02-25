import { useQuery } from '@tanstack/react-query';

import { getMonthlyPlannedTransactions } from '@/services/monthly/get-monthly-planned-transactions';
import { GetPlannedTransactionsRequest } from '@/types';

export function useFetchPlannedTransactions(
  request: GetPlannedTransactionsRequest
) {
  return useQuery({
    queryFn: async () => getMonthlyPlannedTransactions(request),
    queryKey: ['monthly', 'planned-transactions', request],
  });
}
