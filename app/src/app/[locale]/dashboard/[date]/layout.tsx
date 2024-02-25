import { getTranslations, unstable_setRequestLocale } from 'next-intl/server';

import { MonthPicker } from '@/components/month-picker';
import { Params } from '@/types';

export const dynamicParams = true;

export function generateStaticParams() {
  const availableMonths = [];
  for (let index = 1; index <= 12; index++) {
    availableMonths.push(index.toString().padStart(2, '0'));
  }

  const yearNow = new Date().getFullYear();
  const availableYears = [];
  for (let index = yearNow - 5; index < yearNow + 10; index++) {
    availableYears.push(index);
  }

  const params = [];
  for (const year of availableYears) {
    for (const month of availableMonths) {
      params.push({ date: `${year}-${month}` });
    }
  }

  return params;
}

export default async function DashboardLayout({
  children,
  params: { locale },
}: {
  children: React.ReactNode;
  params: Params;
}) {
  unstable_setRequestLocale(locale);
  const t = await getTranslations('Pages.Dashboard');

  return (
    <div className='flex-col md:flex'>
      <div className='flex-1 space-y-4'>
        <div className='flex flex-col items-center justify-center space-y-2 md:flex-row md:justify-between'>
          <h2 className='text-3xl font-bold tracking-tight'>{t('Title')}</h2>
          <div className='flex flex-row items-center space-x-2'>
            <MonthPicker />
          </div>
        </div>
        <div className='grid grid-cols-1 gap-4 md:grid-cols-2'>{children}</div>
      </div>
    </div>
  );
}
