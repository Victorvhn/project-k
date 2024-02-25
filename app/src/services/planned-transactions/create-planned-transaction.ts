'use server';

import { makeRequest } from '@/lib/api-client';
import { PlannedTransactionDto, ProblemDetails } from '@/types';
import { CreatePlannedTransactionRequest } from '@/types/requests/planned-transactions/create-planned-transaction-request';

export const createPlannedTransaction = async (
  request: CreatePlannedTransactionRequest
) => {
  const url = `v1/planned-transactions`;

  const result = await makeRequest(url, {
    method: 'POST',
    body: JSON.stringify(request),
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return (await result.json()) as PlannedTransactionDto;
};
