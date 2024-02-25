'use server';

import { makeRequest } from '@/lib/api-client';

export async function deleteUser(userId: string) {
  await makeRequest(`v1/users/${userId}`, {
    method: 'DELETE',
  });

  return null;
}
