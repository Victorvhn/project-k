'use server';

import { makeRequest } from '@/lib/api-client';
import { PaginatedData, PlannedTransactionDto, ProblemDetails } from '@/types';

export const getAllPlannedTransactions = async () => {
  const url = `v1/planned-transactions?pageSize=100&currentPage=1`;

  const result = await makeRequest(url, {
    method: 'GET',
    next: {
      tags: ['planned-transactions'],
    },
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return (await result.json()) as PaginatedData<PlannedTransactionDto>;
};
