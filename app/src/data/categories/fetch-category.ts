import { useQuery } from '@tanstack/react-query';

import { getCategory } from '@/services/categories/get-category';

export function useFetchCategory(categoryId: string) {
  return useQuery({
    queryFn: async () => getCategory(categoryId),
    queryKey: ['category', categoryId],
    enabled: !!categoryId,
    refetchInterval: false,
  });
}
