import { QueryClient } from '@tanstack/react-query';
import { PlusIcon } from 'lucide-react';
import { Metadata } from 'next';
import { getTranslations, unstable_setRequestLocale } from 'next-intl/server';

import { Button } from '@/components/ui/button';
import { Link } from '@/lib/navigation';
import { fetchCategories } from '@/services/categories/get-all-paginated';
import { Params } from '@/types';

import { CategoriesList } from './_components';

export async function generateMetadata({
  params: { locale },
}: {
  params: Params;
}) {
  const t = await getTranslations({
    locale,
    namespace: 'Metadata.Categories.List',
  });

  return {
    title: t('Title'),
  } as Metadata;
}

export default async function Categories({
  params: { locale },
}: {
  params: Params;
}) {
  unstable_setRequestLocale(locale);
  const t = await getTranslations('Pages.Categories.List');

  const queryClient = new QueryClient();

  await queryClient.prefetchQuery({
    queryKey: ['categories'],
    queryFn: () => fetchCategories(),
  });

  return (
    <div className='flex-col md:flex'>
      <div className='mb-6 flex-1 space-y-4'>
        <div className='flex flex-col items-center justify-center space-y-2 md:flex-row md:justify-between'>
          <h2 className='text-3xl font-bold tracking-tight'>{t('Title')}</h2>
        </div>
      </div>
      <div className='mb-2 flex w-full justify-end'>
        <Button variant='outline' className='justify-self-end' asChild>
          <Link href='add'>
            <PlusIcon className='mr-2 size-4' />
            {t('CreateNew')}
          </Link>
        </Button>
      </div>
      <CategoriesList />
    </div>
  );
}
