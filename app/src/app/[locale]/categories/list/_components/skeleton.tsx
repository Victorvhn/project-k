import { Skeleton } from '@/components/ui/skeleton';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';

export function TableSkeleton() {
  return (
    <div className='rounded-md border'>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>
              <Skeleton className='h-6 w-12' />
            </TableHead>
            <TableHead>
              <Skeleton className='h-6 w-10' />
            </TableHead>
            <TableHead>
              <Skeleton className='h-6 w-14' />
            </TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow>
            <TableCell>
              <Skeleton className='h-6 w-24' />
            </TableCell>
            <TableCell>
              <Skeleton className='h-4 w-8 rounded-full md:w-12' />
            </TableCell>
            <TableCell className='flex space-x-2'>
              <Skeleton className='size-5' />
              <Skeleton className='size-5' />
            </TableCell>
          </TableRow>
          <TableRow>
            <TableCell>
              <Skeleton className='h-6 w-48' />
            </TableCell>
            <TableCell>
              <Skeleton className='h-4 w-8 rounded-full md:w-12' />
            </TableCell>
            <TableCell className='flex space-x-2'>
              <Skeleton className='size-5' />
              <Skeleton className='size-5' />
            </TableCell>
          </TableRow>
          <TableRow>
            <TableCell>
              <Skeleton className='h-6 w-14' />
            </TableCell>
            <TableCell>
              <Skeleton className='h-4 w-8 rounded-full md:w-12' />
            </TableCell>
            <TableCell className='flex space-x-2'>
              <Skeleton className='size-5' />
              <Skeleton className='size-5' />
            </TableCell>
          </TableRow>
          <TableRow>
            <TableCell>
              <Skeleton className='h-6 w-28' />
            </TableCell>
            <TableCell>
              <Skeleton className='h-4 w-8 rounded-full md:w-12' />
            </TableCell>
            <TableCell className='flex space-x-2'>
              <Skeleton className='size-5' />
              <Skeleton className='size-5' />
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </div>
  );
}
