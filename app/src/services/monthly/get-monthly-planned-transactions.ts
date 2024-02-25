'use server';

import { makeRequest } from '@/lib/api-client';
import {
  GetPlannedTransactionsRequest,
  MonthlyPlannedTransactionDto,
  ProblemDetails,
} from '@/types';

export const getMonthlyPlannedTransactions = async (
  request: GetPlannedTransactionsRequest
) => {
  const url = `v1/monthly/${request.year}/${request.month}/planned-transactions`;

  const result = await makeRequest(url, {
    method: 'GET',
    next: {
      tags: ['monthly'],
    },
  });

  if (!result.ok) {
    const problemDetails = (await result.json()) as ProblemDetails;
    throw new Error(JSON.stringify(problemDetails));
  }

  return (await result.json()) as MonthlyPlannedTransactionDto[];
};
