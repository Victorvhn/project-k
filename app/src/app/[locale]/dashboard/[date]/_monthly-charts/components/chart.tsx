'use client';

import { useEffect, useMemo, useState } from 'react';
import { AxisOptions, Chart as ReactCharts } from 'react-charts';
import { useSession } from 'next-auth/react';
import { useFormatter } from 'next-intl';
import { useTheme } from 'next-themes';
import { darken } from 'polished';

import ResizableBox from '@/components/resizable-box';

import ChartSkeleton from './chart-skeleton';

export default function Chart({
  data,
}: {
  data: {
    label: string;
    data: { category: string; amount: number; color: string }[];
  }[];
}) {
  const [mounted, setMounted] = useState(false);
  const session = useSession();
  const formatter = useFormatter();
  const { theme, systemTheme } = useTheme();

  useEffect(() => {
    setMounted(true);
  }, []);

  const primaryAxis = useMemo<
    AxisOptions<(typeof data)[number]['data'][number]>
  >(
    () => ({
      position: 'left',
      getValue: (datum) => datum.category,
    }),
    []
  );

  const secondaryAxes = useMemo<
    AxisOptions<(typeof data)[number]['data'][number]>[]
  >(
    () => [
      {
        position: 'bottom',
        getValue: (datum) => datum.amount,
        min: 0,
        formatters: {
          tooltip: (value: number) =>
            formatter.number(value, {
              style: 'currency',
              currency: session?.data?.preferred_currency,
            }),
        },
      },
    ],
    [formatter, session?.data?.preferred_currency]
  );

  if (!mounted) {
    return <ChartSkeleton />;
  }

  return (
    <ResizableBox>
      <ReactCharts
        options={{
          dark: theme !== 'system' ? theme === 'dark' : systemTheme === 'dark',
          useIntersectionObserver: true,
          data,
          primaryCursor: false,
          secondaryCursor: false,
          primaryAxis,
          secondaryAxes,
          getDatumStyle(datum, status) {
            if (status === 'focused') {
              return {
                fill: darken(0.1, datum.originalDatum.color),
              };
            }

            return {
              fill: datum.originalDatum.color,
            };
          },
        }}
      />
    </ResizableBox>
  );
}
