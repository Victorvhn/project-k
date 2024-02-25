'use client';

import React from 'react';

import { Card, CardContent, CardHeader } from '@/components/ui/card';
import { Skeleton } from '@/components/ui/skeleton';

export default function SummaryCardSkeleton() {
  return (
    <Card>
      <CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
        <Skeleton className='h-6 w-32' />
        <Skeleton className='h-4 w-4' />
      </CardHeader>
      <CardContent>
        <Skeleton className='mb-[4px] h-[calc(2rem-4px)] w-24' />
        <Skeleton className='mb-[2px] h-[calc(1.25rem-2px)] w-36' />
        <Skeleton className='mb-[2px] h-[calc(1.25rem-2px)] w-40' />
      </CardContent>
    </Card>
  );
}
