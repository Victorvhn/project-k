'use server';

import { makeRequest } from '@/lib/api-client';
import { UpdateCurrencyRequest, UserDto } from '@/types';

export async function updateCurrency(
  userId: string,
  request: UpdateCurrencyRequest
) {
  const response = await makeRequest(`v1/users/${userId}`, {
    method: 'PATCH',
    body: JSON.stringify(request),
  });

  return (await response.json()) as UserDto;
}
