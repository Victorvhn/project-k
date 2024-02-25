import { Card, CardContent, CardHeader } from '@/components/ui/card';
import { Skeleton } from '@/components/ui/skeleton';

import ChartSkeleton from './chart-skeleton';

export default function MonthlyChardCardSkeleton() {
  return (
    <Card className='min-h-[448px] lg:min-h-[428px]'>
      <CardHeader className='mb-2 flex flex-row items-center justify-between space-y-0'>
        <div>
          <Skeleton className='mb-1 mt-1 h-6 w-32' />
          <Skeleton className='h-6 w-40' />
        </div>
        <Skeleton className='h-10 w-14' />
      </CardHeader>
      <CardContent className='h-[330px]'>
        <ChartSkeleton />
      </CardContent>
    </Card>
  );
}
