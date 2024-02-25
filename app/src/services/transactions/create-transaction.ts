'use server';

import { makeRequest } from '@/lib/api-client';
import {
  CreateTransactionRequest,
  ProblemDetails,
  TransactionDto,
} from '@/types';

export const createTransaction = async (request: CreateTransactionRequest) => {
  const url = `v1/transactions`;

  const result = await makeRequest(url, {
    method: 'POST',
    body: JSON.stringify(request),
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return (await result.json()) as TransactionDto;
};
