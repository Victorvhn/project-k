'use server';

import { makeRequest } from '@/lib/api-client';
import {
  EditTransactionRequest,
  ProblemDetails,
  TransactionDto,
} from '@/types';

export const editTransaction = async (
  transactionId: string,
  request: EditTransactionRequest
) => {
  const url = `v1/transactions/${transactionId}`;

  const result = await makeRequest(url, {
    method: 'PUT',
    body: JSON.stringify(request),
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return (await result.json()) as TransactionDto;
};
