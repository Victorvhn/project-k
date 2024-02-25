'use server';

import { makeRequest } from '@/lib/api-client';
import { GetSummaryRequest, ProblemDetails, SummaryDto } from '@/types';

export const getSummary = async (getSummaryRequest: GetSummaryRequest) => {
  const url = `v1/monthly/${getSummaryRequest.monthlyRequest.year}/${getSummaryRequest.monthlyRequest.month}/summary?transactionType=${getSummaryRequest.transactionType}`;

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

  return (await result.json()) as SummaryDto;
};
