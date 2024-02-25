'use server';

import { makeRequest } from '@/lib/api-client';
import { PlannedTransactionDto, ProblemDetails } from '@/types';
import { EditPlannedTransactionRequest } from '@/types/requests/planned-transactions/edit-planned-transaction-request';

export const updatePlannedTransaction = async (
  plannedTransactionId: string,
  request: EditPlannedTransactionRequest
) => {
  const url = `v1/planned-transactions/${plannedTransactionId}`;

  const result = await makeRequest(url, {
    method: 'PUT',
    body: JSON.stringify(request),
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return (await result.json()) as PlannedTransactionDto;
};
