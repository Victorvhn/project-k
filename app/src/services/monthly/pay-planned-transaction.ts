'use server';

import { makeRequest } from '@/lib/api-client';
import { PayPlannedTransactionRequest, ProblemDetails } from '@/types';

export const payPlannedTransaction = async (
  request: PayPlannedTransactionRequest
) => {
  const url = `v1/monthly/${request.monthlyRequest.year}/${request.monthlyRequest.month}/planned-transaction/${request.plannedTransactionId}/pay`;

  const result = await makeRequest(url, {
    method: 'POST',
    body: JSON.stringify({
      amount: request.amount,
    }),
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return null;
};
