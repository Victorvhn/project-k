'use server';

import { makeRequest } from '@/lib/api-client';
import { DeleteTransactionRequest, ProblemDetails } from '@/types';

export const deleteTransaction = async (request: DeleteTransactionRequest) => {
  const url = `v1/transactions/${request.transactionId}`;

  const result = await makeRequest(url, {
    method: 'DELETE',
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return null;
};
