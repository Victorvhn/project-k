import { useQuery } from '@tanstack/react-query';

import { getMonthlyOverview } from '@/services/monthly/get-monthly-overview';
import { GetMonthlyOverviewRequest } from '@/types';

export function useFetchMonthlyOverview(request: GetMonthlyOverviewRequest) {
  return useQuery({
    queryFn: () => getMonthlyOverview(request),
    queryKey: ['monthly', 'overview', request],
  });
}
