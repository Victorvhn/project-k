'use server';

import { makeRequest } from '@/lib/api-client';
import { CategoryDto, CreateCategoryRequest, ProblemDetails } from '@/types';

export const createCategory = async (request: CreateCategoryRequest) => {
  const url = `v1/categories`;

  const result = await makeRequest(url, {
    method: 'POST',
    body: JSON.stringify(request),
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return (await result.json()) as CategoryDto;
};
