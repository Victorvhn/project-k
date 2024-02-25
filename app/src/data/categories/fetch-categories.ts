import { useQuery } from '@tanstack/react-query';

import { fetchCategories } from '@/services/categories/get-all-paginated';

export function useFetchCategories() {
  return useQuery({
    queryFn: async () => fetchCategories(),
    queryKey: ['categories'],
  });
}
