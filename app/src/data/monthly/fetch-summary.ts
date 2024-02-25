import { useQuery } from '@tanstack/react-query';

import { getSummary } from '@/services/monthly/get-summary';
import { GetSummaryRequest } from '@/types';

export function useFetchSummary(getSummaryRequest: GetSummaryRequest) {
  return useQuery({
    queryFn: async () => getSummary(getSummaryRequest),
    queryKey: ['monthly', 'summary', getSummaryRequest],
  });
}
