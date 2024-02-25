'use server';

import { makeRequest } from '@/lib/api-client';
import { DeleteCategoryRequest, ProblemDetails } from '@/types';

export const deleteCategory = async (request: DeleteCategoryRequest) => {
  const url = `v1/categories/${request.id}`;

  const result = await makeRequest(url, {
    method: 'DELETE',
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return null;
};
