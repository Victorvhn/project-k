'use server';

import { makeRequest } from '@/lib/api-client';
import { ProblemDetails, TransactionDto } from '@/types';

export const getTransactionById = async (id: string) => {
  const url = `v1/transactions/${id}`;

  const result = await makeRequest(url, {
    method: 'GET',
    next: {
      tags: ['transaction', id],
    },
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return (await result.json()) as TransactionDto;
};
