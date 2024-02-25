'use client';

import { ColumnDef } from '@tanstack/react-table';
import { PencilIcon } from 'lucide-react';

import { Button } from '@/components/ui/button';
import { Link } from '@/lib/navigation';
import { CategoryDto } from '@/types';

import { DeleteButton } from './delete-button';

export const columns: ColumnDef<CategoryDto>[] = [
  {
    accessorKey: 'name',
    header: 'Name',
  },
  {
    accessorKey: 'hexColor',
    header: 'Color',
    cell: ({ row }) => {
      return (
        <div
          className='h-4 w-8 rounded-full md:w-12'
          style={{
            backgroundColor: row.original.hexColor,
          }}
        />
      );
    },
  },
  {
    id: 'actions',
    header: 'Actions',
    cell: ({ row }) => {
      const category = row.original;

      return (
        <div>
          <Button variant='ghost' className='size-8 p-0' asChild>
            <Link
              href={{
                pathname: `/categories/${category.id}/edit`,
              }}
            >
              <PencilIcon className='size-4' />
            </Link>
          </Button>
          <DeleteButton categoryId={category.id} />
        </div>
      );
    },
  },
];
