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
import { useFetchPlannedTransactions } from '@/data/monthly/fetch-planned-transactions';
import { Link } from '@/lib/navigation';
import { GetPlannedTransactionsRequest, MonthlyRequest } from '@/types';

import PlannedTransactionsCardError from './error';
import PlannedTransactionsList from './list';

export default function PlannedTransactionsCard({
  request,
  monthlyRequest,
}: {
  request: GetPlannedTransactionsRequest;
  monthlyRequest: MonthlyRequest;
}) {
  const t = useTranslations('Pages.Dashboard.PlanedTransactionsList');
  const { data, isSuccess, failureReason } =
    useFetchPlannedTransactions(request);

  if (!isSuccess) {
    return <PlannedTransactionsCardError failureReason={failureReason} />;
  }

  return (
    <Card>
      <CardHeader className='flex flex-row items-center justify-between'>
        <div>
          <CardTitle className='text-lg'>{t('Title')}</CardTitle>
          <CardDescription>
            {t('Description', {
              count: data.filter((t) => t.ignore !== true).length,
            })}
          </CardDescription>
        </div>
        <Button
          variant='outline'
          className='ml-2'
          aria-label={t('CreateTransactionButtonDescription')}
          asChild
        >
          <Link
            href={{
              pathname: '/planned-transactions/add',
              query: {
                year: monthlyRequest.year,
                month: monthlyRequest.month,
              },
            }}
            className='h-10 px-4 py-2'
            aria-label={t('CreateTransactionLinkDescription')}
          >
            <PlusIcon />
          </Link>
        </Button>
      </CardHeader>
      <PlannedTransactionsList
        plannedTransactions={data}
        monthlyRequest={{ year: request.year, month: request.month }}
      />
    </Card>
  );
}
