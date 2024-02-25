'use client';

import DefaultError from '@/components/default-error';
import { useFetchCategories } from '@/data/categories/fetch-categories';

import { columns } from './columns';
import { DataTable } from './data-table';
import { TableSkeleton } from './skeleton';

export function CategoriesList() {
  const {
    data: categories,
    isLoading,
    isError,
    failureReason,
  } = useFetchCategories();

  if (isLoading) {
    return <TableSkeleton />;
  }

  if (isError) {
    return (
      <DefaultError
        failureReason={failureReason}
        className='h-[calc(100vh-24rem)]'
      />
    );
  }

  return <DataTable columns={columns} data={categories?.data ?? []} />;
}
