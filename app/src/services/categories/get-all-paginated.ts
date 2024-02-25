'use server';

import { makeRequest } from '@/lib/api-client';
import { CategoryDto, PaginatedData, ProblemDetails } from '@/types';

export const fetchCategories = async () => {
  const url = `v1/categories?pageSize=100&currentPage=1`;

  const result = await makeRequest(url, {
    method: 'GET',
    next: {
      tags: ['categories'],
    },
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return (await result.json()) as PaginatedData<CategoryDto>;
};
