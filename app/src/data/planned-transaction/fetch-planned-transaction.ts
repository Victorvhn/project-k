import { useQuery } from '@tanstack/react-query';

import { getMonthlyPlannedTransaction } from '@/services/planned-transactions/get-monthly-planned-transaction';
import { MonthlyRequest } from '@/types';

export function useFetchPlannedTransaction(
  plannedTransactionId?: string,
  monthlyRequest?: MonthlyRequest
) {
  return useQuery({
    queryFn: async () =>
      getMonthlyPlannedTransaction(plannedTransactionId!, monthlyRequest!),
    queryKey: ['planned-transaction', plannedTransactionId, monthlyRequest],
    enabled: !!plannedTransactionId && !!monthlyRequest,
    refetchInterval: false,
  });
}
