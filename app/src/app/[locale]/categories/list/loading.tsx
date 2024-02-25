import { Skeleton } from '@/components/ui/skeleton';

import { TableSkeleton } from './_components/skeleton';

export default function Loading() {
  return (
    <div className='flex-col md:flex'>
      <div className='mb-6 flex-1 space-y-4'>
        <div className='flex flex-col items-center justify-center space-y-2 md:flex-row md:justify-between'>
          <Skeleton className='h-9 w-72' />
        </div>
      </div>
      <div className='mb-2 flex w-full justify-end'>
        <Skeleton className='h-10 w-48' />
      </div>
      <TableSkeleton />
    </div>
  );
}
