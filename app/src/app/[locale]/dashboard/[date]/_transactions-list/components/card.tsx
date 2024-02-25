'use client';

import { PlusIcon } from 'lucide-react';
import { useTranslations } from 'next-intl';

import { Button } from '@/components/ui/button';
import {
  Card,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { useFetchTransactions } from '@/data/monthly/fetch-transactions';
import { Link } from '@/lib/navigation';
import { GetTransactionsRequest } from '@/types';

import TransactionsCardError from './error';
import TransactionsList from './list';

export default function TransactionsCard({
  request,
}: {
  request: GetTransactionsRequest;
}) {
  const t = useTranslations('Pages.Dashboard.TransactionsList');
  const { data, isSuccess, failureReason } = useFetchTransactions(request);

  if (!isSuccess) {
    return <TransactionsCardError failureReason={failureReason} />;
  }

  return (
    <Card>
      <CardHeader className='flex flex-row items-center justify-between'>
        <div>
          <CardTitle className='text-lg'>{t('Title')}</CardTitle>
          <CardDescription>
            {t('Description', { count: data.length })}
          </CardDescription>
        </div>
        <Button
          variant='outline'
          className='ml-2'
          aria-label='Create transaction button'
          asChild
        >
          <Link
            href={{
              pathname: '/transactions/add',
              query: {
                year: request.year,
                month: request.month,
              },
            }}
            className='h-10 px-4 py-2'
            aria-label='Go to create transaction page'
          >
            <PlusIcon />
          </Link>
        </Button>
      </CardHeader>
      <TransactionsList
        transactions={data}
        monthlyRequest={{ year: request.year, month: request.month }}
      />
    </Card>
  );
}
