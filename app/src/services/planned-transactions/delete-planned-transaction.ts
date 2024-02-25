'use server';

import { makeRequest } from '@/lib/api-client';
import { DeletePlannedTransactionRequest, ProblemDetails } from '@/types';

export const deletePlannedTransaction = async (
  request: DeletePlannedTransactionRequest
) => {
  const url = `v1/planned-transactions/${request.plannedTransactionId}?actionType=${request.actionType}&year=${request.monthlyRequest.year}&month=${request.monthlyRequest.month}`;

  const result = await makeRequest(url, {
    method: 'DELETE',
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return null;
};
