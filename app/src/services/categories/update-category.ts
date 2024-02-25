'use server';

import { makeRequest } from '@/lib/api-client';
import { CategoryDto, ProblemDetails, UpdateCategoryRequest } from '@/types';

export const updateCategory = async (
  categoryId: string,
  request: UpdateCategoryRequest
) => {
  const url = `v1/categories/${categoryId}`;

  const result = await makeRequest(url, {
    method: 'PUT',
    body: JSON.stringify(request),
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return (await result.json()) as CategoryDto;
};
