import { CircleIcon } from 'lucide-react';
import { Metadata } from 'next';
import { getTranslations, unstable_setRequestLocale } from 'next-intl/server';

import { Button } from '@/components/ui/button';
import { Link } from '@/lib/navigation';
import { ApiHealthCheckResponse, ApiHealthCheckStatus, Params } from '@/types';

export async function generateMetadata({
  params: { locale },
}: {
  params: Params;
}) {
  const t = await getTranslations({
    locale,
    namespace: 'Metadata.ServerStatus',
  });

  return {
    title: t('Title'),
  } as Metadata;
}

async function getHealthCheck() {
  const result = await fetch(process.env.API_HEALTH_URL!, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
    next: {
      revalidate: 300,
      tags: ['health'],
    },
  });

  if (!result.ok) {
    return null;
  }

  const data = (await result.json()) as ApiHealthCheckResponse;

  return data;
}

export default async function ServerStatus({
  params: { locale },
}: {
  params: Params;
}) {
  unstable_setRequestLocale(locale);

  const t = await getTranslations('Pages.ServerStatus');

  const healthCheck = await getHealthCheck();

  const defaultStatus = () => {
    if (!healthCheck) {
      return (
        <>
          <CircleIcon className='size-3 fill-current text-red-500' />
          <span className='text-sm font-medium text-yellow-500'>
            {t('Unhealthy')}
          </span>
        </>
      );
    }

    switch (healthCheck.status) {
      case ApiHealthCheckStatus.Healthy:
        return (
          <>
            <CircleIcon className='size-3 fill-current text-green-500' />
            <span className='text-sm font-medium text-green-500'>
              {t('Healthy')}
            </span>
          </>
        );

      case ApiHealthCheckStatus.Degraded:
        return (
          <>
            <CircleIcon className='size-3 fill-current text-yellow-500' />
            <span className='text-sm font-medium text-yellow-500'>
              {t('Degraded')}
            </span>
          </>
        );

      case ApiHealthCheckStatus.Unhealthy:
        return (
          <>
            <CircleIcon className='size-3 fill-current text-red-500' />
            <span className='text-sm font-medium text-red-500'>
              {t('Unhealthy')}
            </span>
          </>
        );
    }
  };

  const getStatusIcon = (status: ApiHealthCheckStatus | undefined) => {
    if (!status) {
      return <CircleIcon className='size-4 fill-current text-red-500' />;
    }

    switch (status) {
      case ApiHealthCheckStatus.Healthy:
        return <CircleIcon className='size-4 fill-current text-green-500' />;

      case ApiHealthCheckStatus.Degraded:
        return <CircleIcon className='size-4 fill-current text-yellow-500' />;

      case ApiHealthCheckStatus.Unhealthy:
        return <CircleIcon className='size-4 fill-current text-red-500' />;
    }
  };

  return (
    <div className='flex h-[calc(100vh-12rem)] flex-col'>
      <div className='py-4 sm:py-6 lg:py-8 xl:py-12'>
        <div className='container px-4 text-center'>
          <h1 className='text-3xl font-bold tracking-tighter sm:text-4xl'>
            {t('Title')}
          </h1>
        </div>
      </div>
      <section className='py-6 sm:py-12'>
        <div className='container px-4'>
          <div className='grid gap-4 md:gap-6 lg:gap-8 xl:gap-10'>
            <div className='flex items-center space-x-4'>
              <h2 className='text-2xl font-semibold tracking-tight'>
                {t('Title')}
              </h2>
              <div className='flex items-center space-x-1'>
                {defaultStatus()}
              </div>
            </div>
            <div className='grid gap-4 md:gap-6 lg:gap-8 xl:gap-10'>
              <div className='flex items-center space-x-2'>
                {getStatusIcon(healthCheck?.status)}
                <div className='text-sm font-medium'>api</div>
              </div>
              <div className='flex items-center space-x-2'>
                {getStatusIcon(healthCheck?.entries.npgsql.status)}
                <div className='text-sm font-medium'>database</div>
              </div>
            </div>
          </div>
        </div>
      </section>
      <div className='flex items-center justify-center'>
        <Button size='default' variant='link' asChild>
          <Link href='/' className='font-bold'>
            {t('BackToHome')}
          </Link>
        </Button>
      </div>
    </div>
  );
}
