'use server';

import { makeRequest } from '@/lib/api-client';
import {
  GetMonthlyOverviewRequest,
  MonthlyExpensesOverviewResponse,
  ProblemDetails,
} from '@/types';

export const getMonthlyOverview = async (
  request: GetMonthlyOverviewRequest
) => {
  const url = `v1/monthly/${request.year}/${request.month}/expenses-overview`;

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

  return (await result.json()) as MonthlyExpensesOverviewResponse[];
};
