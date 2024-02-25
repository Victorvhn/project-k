import { Skeleton } from '@/components/ui/skeleton';

import PlannedTransactionsFormSkeleton from './_components/skeleton';

export default function Loading() {
  return (
    <div>
      <Skeleton className='my-4 h-[32px] w-2/3 md:w-1/4' />
      <PlannedTransactionsFormSkeleton />
    </div>
  );
}
