import {
  dehydrate,
  HydrationBoundary,
  QueryClient,
} from '@tanstack/react-query';
import { Metadata } from 'next';
import { getTranslations, unstable_setRequestLocale } from 'next-intl/server';

import { getCategory } from '@/services/categories/get-category';
import { FormActionType, Params } from '@/types';

import { CategoriesForm } from '../../_components/form';

export async function generateMetadata({
  params: { locale },
}: {
  params: Params;
}) {
  const t = await getTranslations({
    locale,
    namespace: 'Metadata.Categories.Edit',
  });

  return {
    title: t('Title'),
  } as Metadata;
}

export default async function EditCategory({
  params: { locale, categoryId },
}: {
  params: Params & { categoryId: string };
}) {
  unstable_setRequestLocale(locale);

  const t = await getTranslations('Pages.Categories.Edit');

  const queryClient = new QueryClient();

  await queryClient.prefetchQuery({
    queryKey: ['category', categoryId],
    queryFn: () => getCategory(categoryId),
  });

  return (
    <>
      <h1 className='mb-2 scroll-m-20 text-2xl font-semibold tracking-tight'>
        {t('Title')}
      </h1>
      <p className='mb-8 leading-7'>{t('Description')}</p>
      <HydrationBoundary state={dehydrate(queryClient)}>
        <CategoriesForm
          formActionType={FormActionType.Update}
          categoryId={categoryId}
        />
      </HydrationBoundary>
    </>
  );
}
