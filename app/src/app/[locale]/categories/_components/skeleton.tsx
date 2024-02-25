import { Skeleton } from '@/components/ui/skeleton';

export default function CategoriesFormSkeleton() {
  return (
    <>
      <div className='mb-6 flex flex-col space-y-4 md:flex-row md:space-x-4 md:space-y-0'>
        <div className='flex flex-col space-y-2 md:w-3/4'>
          <Skeleton className='h-[17px] w-2/3 rounded-full md:w-1/6' />
          <Skeleton className='h-10 w-full' />
        </div>
        <div className='flex flex-col space-y-2 md:w-1/4'>
          <Skeleton className='h-[17px] w-2/3 rounded-full md:w-1/6' />
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
