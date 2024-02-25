'use client';

import { useMemo } from 'react';
import { useTranslations } from 'next-intl';
import { adjustHue } from 'polished';

import DefaultError from '@/components/default-error';
import { useFetchMonthlyOverview } from '@/data/monthly/fetch-monthly-overview';
import { GetMonthlyOverviewRequest } from '@/types';

import Chart from './chart';
import ChartSkeleton from './chart-skeleton';

type CardProps = {
  request: GetMonthlyOverviewRequest;
};

export default function ChartWrapper({ request }: CardProps) {
  const t = useTranslations('Pages.Dashboard.MonthlyCharts');
  const { data, isSuccess, isLoading, failureReason } =
    useFetchMonthlyOverview(request);

  const formattedData = useMemo(() => {
    if (!data) {
      return null;
    }

    return data.reduce(
      (acc, current) => {
        if (current.plannedAmount === 0 && current.currentAmount === 0) {
          return acc;
        }

        acc[0].data.push({
          category: current.categoryName,
          amount: current.plannedAmount,
          color: current.categoryHexColor,
        });
        acc[1].data.push({
          category: current.categoryName,
          amount: current.currentAmount,
          color: adjustHue(20, current.categoryHexColor),
        });

        return acc;
      },
      [
        {
          label: t('Planned'),
          data: Array<{ category: string; amount: number; color: string }>(),
        },
        {
          label: t('Current'),
          data: Array<{ category: string; amount: number; color: string }>(),
        },
      ]
    );
  }, [data, t]);

  if (isLoading) {
    return <ChartSkeleton />;
  }

  if (!isSuccess) {
    return <DefaultError failureReason={failureReason} className='h-[330px]' />;
  }

  if (!formattedData || formattedData.every((d) => d.data.length === 0)) {
    return (
      <div className='flex h-[330px] items-center justify-center'>
        <p className='text-sm text-muted-foreground'>{t('Nothing')}</p>
      </div>
    );
  }

  return <Chart data={formattedData} />;
}
