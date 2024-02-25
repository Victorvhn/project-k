import { Metadata } from 'next';
import { getTranslations, unstable_setRequestLocale } from 'next-intl/server';

import { FormActionType, Params } from '@/types';

import { CategoriesForm } from '../_components/form';

export async function generateMetadata({
  params: { locale },
}: {
  params: Params;
}) {
  const t = await getTranslations({
    locale,
    namespace: 'Metadata.Categories.Add',
  });

  return {
    title: t('Title'),
  } as Metadata;
}

export default async function CreateCategory({
  params: { locale },
}: {
  params: Params;
}) {
  unstable_setRequestLocale(locale);

  const t = await getTranslations('Pages.Categories.Add');

  return (
    <>
      <h1 className='mb-2 scroll-m-20 text-2xl font-semibold tracking-tight'>
        {t('Title')}
      </h1>
      <p className='mb-8 leading-7'>{t('Description')}</p>
      <CategoriesForm formActionType={FormActionType.Create} />
    </>
  );
}
