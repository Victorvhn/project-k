'use server';

import { makeRequest } from '@/lib/api-client';
import { MonthlyRequest, PlannedTransactionDto, ProblemDetails } from '@/types';

export const getMonthlyPlannedTransaction = async (
  plannedTransactionId: string,
  monthlyRequest: MonthlyRequest
) => {
  const url = `v1/monthly/${monthlyRequest.year}/${monthlyRequest.month}/${plannedTransactionId}`;

  const result = await makeRequest(url, {
    method: 'GET',
    next: {
      tags: [
        'planned-transaction',
        plannedTransactionId,
        monthlyRequest.year.toString(),
        monthlyRequest.month.toString(),
      ],
    },
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return (await result.json()) as PlannedTransactionDto;
};
