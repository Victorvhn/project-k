'use server';

import { makeRequest } from '@/lib/api-client';
import {
  GetTransactionsRequest,
  MonthlyTransactionDto,
  ProblemDetails,
} from '@/types';

export const getMonthlyTransactions = async (
  request: GetTransactionsRequest
) => {
  const url = `v1/monthly/${request.year}/${request.month}/transactions`;

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

  return (await result.json()) as MonthlyTransactionDto[];
};
