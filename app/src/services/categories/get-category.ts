'use server';

import { makeRequest } from '@/lib/api-client';
import { CategoryDto, ProblemDetails } from '@/types';

export const getCategory = async (categoryId: string) => {
  const url = `v1/categories/${categoryId}`;

  const result = await makeRequest(url, {
    method: 'GET',
    next: {
      tags: ['categories', categoryId],
    },
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return (await result.json()) as CategoryDto;
};
