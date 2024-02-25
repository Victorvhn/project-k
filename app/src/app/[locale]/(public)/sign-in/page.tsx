import { Metadata } from 'next';
import { getTranslations, unstable_setRequestLocale } from 'next-intl/server';

import SignInButton from '@/components/sign-in-button';
import { Params } from '@/types';

import SignInError from './_components/sign-in-error';

export async function generateMetadata({
  params: { locale },
}: {
  params: Params;
}) {
  const t = await getTranslations({
    locale,
    namespace: 'Metadata.SignIn',
  });

  return {
    title: t('Title'),
  } as Metadata;
}

export default async function SignIn({
  params: { locale },
  searchParams: { callbackUrl, error },
}: {
  params: Params;
  searchParams: { callbackUrl?: string; error?: string };
}) {
  unstable_setRequestLocale(locale);

  const t = await getTranslations('Pages.SignIn');

  return (
    <div className='mx-auto flex h-[calc(100vh-12rem)] flex-col items-center justify-center'>
      <h1 className='mb-2 text-2xl font-semibold tracking-tight'>
        {t('Greeting')}
      </h1>
      <div className='flex flex-col space-y-2'>
        <SignInButton provider='google' callbackUrl={callbackUrl ?? '/'} />
        <SignInButton provider='github' callbackUrl={callbackUrl ?? '/'} />
      </div>
      {error && <SignInError />}
    </div>
  );
}
