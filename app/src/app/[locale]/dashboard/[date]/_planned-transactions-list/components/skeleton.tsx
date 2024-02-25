'use client';

import React from 'react';

import { Card, CardContent, CardHeader } from '@/components/ui/card';
import { Separator } from '@/components/ui/separator';
import { Skeleton } from '@/components/ui/skeleton';

export default function PlannedTransactionCardSkeleton() {
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
        <div className='flex h-full flex-col'>
          <div>
            <div className='mb-1 flex flex-row items-center justify-end space-x-2'>
              <Skeleton className='size-4' />
              <Skeleton className='size-4' />
            </div>
            <div className='mb-1 flex flex-row items-center justify-between'>
              <div className='flex flex-col space-y-1'>
                <Skeleton className='h-6 w-32' />
                <Skeleton className='h-4 w-36' />
              </div>
              <div className='flex flex-col space-y-1'>
                <Skeleton className='h-6 w-24' />
                <Skeleton className='h-4 w-14 self-end' />
              </div>
            </div>
            <div className='mt-2 flex w-full flex-row justify-between'>
              <div className='mr-4'>
                <Skeleton className='inline-flex h-5 w-14 rounded-full' />
                <Skeleton className='inline-flex h-5 w-12 rounded-full' />
                <Skeleton className='inline-flex h-5 w-20 rounded-full' />
              </div>
              <div className='flex inline-flex items-center'>
                <Skeleton className='inline-flex h-5 w-12 rounded-full' />
              </div>
            </div>
          </div>
          <Separator className='my-3' />
          <div>
            <div className='mb-1 flex flex-row items-center justify-end space-x-2'>
              <Skeleton className='size-4' />
              <Skeleton className='size-4' />
            </div>
            <div className='mb-1 flex flex-row items-center justify-between'>
              <div className='flex flex-col space-y-1'>
                <Skeleton className='h-6 w-28' />
                <Skeleton className='h-4 w-36' />
              </div>
              <div className='flex flex-col space-y-1'>
                <Skeleton className='h-6 w-20' />
                <Skeleton className='h-4 w-12 self-end' />
              </div>
            </div>
            <div className='mt-2 flex w-full flex-row justify-between'>
              <div className='mr-4 space-x-1'>
                <Skeleton className='inline-flex h-5 w-14 rounded-full' />
                <Skeleton className='inline-flex h-5 w-12 rounded-full' />
              </div>
              <div className='flex inline-flex items-center'>
                <Skeleton className='inline-flex h-5 w-12 rounded-full' />
              </div>
            </div>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
