import { Skeleton } from '@/components/ui/skeleton';

export default async function ServerStatusLoading() {
  return (
    <div className='flex h-[calc(100vh-12rem)] flex-col'>
      <div className='py-4 sm:py-6 lg:py-8 xl:py-12'>
        <div className='container flex items-center justify-center px-4'>
          <Skeleton className='h-10 w-72' />
        </div>
      </div>
      <section className='py-6 sm:py-12'>
        <div className='container px-4'>
          <div className='grid gap-4 md:gap-6 lg:gap-8 xl:gap-10'>
            <div className='flex items-center space-x-4'>
              <Skeleton className='h-8 w-52' />
              <Skeleton className='size-3 rounded-full' />
              <Skeleton className='h-5 w-56' />
            </div>
            <div className='grid gap-4 md:gap-6 lg:gap-8 xl:gap-10'>
              <div className='flex items-center space-x-2'>
                <Skeleton className='size-4 rounded-full' />
                <Skeleton className='h-5 w-7' />
              </div>
              <div className='flex items-center space-x-2'>
                <Skeleton className='size-4 rounded-full' />
                <Skeleton className='h-5 w-16' />
              </div>
            </div>
          </div>
        </div>
      </section>
      <div className='flex items-center justify-center'>
        <Skeleton className='h-10 w-32' />
      </div>
    </div>
  );
}
