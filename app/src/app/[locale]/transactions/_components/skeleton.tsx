import { Skeleton } from '@/components/ui/skeleton';

export default function TransactionsFormSkeleton() {
  return (
    <>
      <div className='mb-4 flex flex-col space-y-8 md:grid md:grid-cols-4 md:gap-4'>
        <div className='flex flex-col space-y-2 md:col-span-2'>
          <Skeleton className='h-[17px] w-2/3 md:w-1/3' />
          <Skeleton className='h-10 w-full' />
        </div>
        <div className='flex flex-col space-y-2 md:col-span-4'>
          <Skeleton className='h-[17px] w-2/3 md:w-1/6' />
          <Skeleton className='h-10 w-full' />
        </div>
        <div className='flex flex-col space-y-2 md:col-span-3'>
          <Skeleton className='h-[17px] w-2/3 md:w-1/6' />
          <Skeleton className='h-10 w-full' />
        </div>
        <div className='flex h-[80px] flex-col space-y-2 md:col-span-1'>
          <Skeleton className='h-[17px] w-2/3 md:w-2/4' />
          <Skeleton className='h-[16px] w-1/4' />
          <Skeleton className='h-[16px] w-1/4' />
        </div>
        <div className='flex flex-col space-y-2 md:col-span-2'>
          <Skeleton className='h-[17px] w-2/3 md:w-1/3' />
          <Skeleton className='h-10 w-full' />
        </div>
        <div className='flex flex-col space-y-2 md:col-span-2'>
          <Skeleton className='h-[17px] w-2/3 md:w-1/3' />
          <Skeleton className='h-10 w-full' />
        </div>
      </div>
      <div className='flex w-full flex-row justify-end space-x-2'>
        <Skeleton className='h-[40px] w-[130px]' />
        <Skeleton className='h-[40px] w-[100px]' />
      </div>
    </>
  );
}
