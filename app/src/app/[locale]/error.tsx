'use client';

import { useTranslations } from 'next-intl';

import { Button } from '@/components/ui/button';

export default function Error({ reset }: { reset: () => void }) {
  const t = useTranslations('Pages.Error');

  return (
    <div className='flex h-[calc(100vh-12rem)] flex-col'>
      <div className='py-4 sm:py-6 lg:py-8 xl:py-12'>
        <div className='container px-4 text-center'>
          <h1 className='text-3xl font-bold tracking-tighter sm:text-4xl'>
            {t('Title')}
          </h1>
        </div>
      </div>
      <div className='flex items-center justify-center'>
        <Button onClick={() => reset()}>{t('TryAgain')}</Button>
      </div>
    </div>
  );
}
