import {
  dehydrate,
  HydrationBoundary,
  QueryClient,
} from '@tanstack/react-query';
import { BarChart3Icon } from 'lucide-react';
import { getTranslations } from 'next-intl/server';

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { ScrollArea, ScrollBar } from '@/components/ui/scroll-area';
import { getMonthlyOverview } from '@/services/monthly/get-monthly-overview';

import ChartWrapper from './components/chart-wrapper';

export default async function MonthlyCharts({
  year,
  month,
}: {
  year: number;
  month: number;
}) {
  const t = await getTranslations('Pages.Dashboard.MonthlyCharts');

  const queryClient = new QueryClient();

  const request = {
    year,
    month,
  };

  await queryClient.prefetchQuery({
    queryKey: ['monthly', 'overview', request],
    queryFn: () => getMonthlyOverview(request),
  });

  return (
    <HydrationBoundary state={dehydrate(queryClient)}>
      <Card>
        <CardHeader>
          <div className='flex items-center justify-between'>
            <CardTitle className='text-md font-medium'>{t('Title')}</CardTitle>
            <BarChart3Icon className='size-4 text-muted-foreground' />
          </div>
          <CardDescription>{t('Description')}</CardDescription>
        </CardHeader>
        <ScrollArea>
          <CardContent className='min-h-[330px]'>
            <ChartWrapper request={request} />
          </CardContent>
          <ScrollBar orientation='horizontal' />
        </ScrollArea>
      </Card>
    </HydrationBoundary>
  );
}
