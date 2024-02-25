import { getServerSession } from 'next-auth';
import { getLocale } from 'next-intl/server';

import { authOptions } from './auth';

async function getLanguage(): Promise<string> {
  let locale = 'en';
  try {
    // TODO: Trying to understand why this is not working properly after packages update
    locale = await getLocale();
  } catch (error) {}

  let language;
  switch (locale) {
    case 'en':
      language = 'en-US';
      break;
    case 'pt':
      language = 'pt-BR';
      break;
    default:
      language = 'en-US';
      break;
  }

  return language;
}

async function getDefaultHeaders(): Promise<HeadersInit> {
  const defaultHeaders: Record<string, string> = {
    'Content-Type': 'application/json',
    'Accept-Language': await getLanguage(),
  };

  const session = await getServerSession(authOptions);
  if (session) {
    defaultHeaders['Authorization'] = `Bearer ${session.api_access_token}`;
  }

  return defaultHeaders as HeadersInit;
}

export async function makeRequest(
  path: string,
  init?: RequestInit
): Promise<Response> {
  const result = await fetch(`${process.env.API_URL}/${path}`, {
    ...init,
    headers: { ...init?.headers, ...(await getDefaultHeaders()) },
  });

  return result;
}
