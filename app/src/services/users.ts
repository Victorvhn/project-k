import { makeRequest } from '@/lib/api-client';
import { CreateUserRequest } from '@/types';

export async function userExists(email: string) {
  const response = await makeRequest(`v1/users/${email}/exists`, {
    method: 'GET',
  });

  return response;
}

export async function createUser(requestDto: CreateUserRequest) {
  const response = await makeRequest(`v1/users`, {
    method: 'POST',
    body: JSON.stringify(requestDto),
  });

  return response;
}
