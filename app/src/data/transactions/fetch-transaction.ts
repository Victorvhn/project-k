import { useQuery } from '@tanstack/react-query';

import { getTransactionById } from '@/services/transactions/get-transaction-by-id';

export function useFetchTransaction(id: string | undefined) {
  return useQuery({
    queryFn: async () => getTransactionById(id!),
    queryKey: ['transaction', id],
    enabled: !!id,
    refetchInterval: false,
  });
}
