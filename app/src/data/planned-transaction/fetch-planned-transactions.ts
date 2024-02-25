import { useQuery } from '@tanstack/react-query';

import { getAllPlannedTransactions } from '@/services/planned-transactions/get-all';

export function useFetchPlannedTransactionsPaginated() {
  return useQuery({
    queryFn: async () => getAllPlannedTransactions(),
    queryKey: ['planned-transactions'],
  });
}
