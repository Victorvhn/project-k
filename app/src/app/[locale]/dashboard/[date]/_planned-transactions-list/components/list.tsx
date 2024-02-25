'use client';

import { parseISO } from 'date-fns';
import { useSession } from 'next-auth/react';
import { useFormatter, useTranslations } from 'next-intl';

import { Badge } from '@/components/ui/badge';
import { CardContent } from '@/components/ui/card';
import { ScrollArea } from '@/components/ui/scroll-area';
import { Separator } from '@/components/ui/separator';
import { cn } from '@/lib/utils';
import {
  AmountType,
  MonthlyPlannedTransactionDto,
  MonthlyRequest,
  TransactionType,
} from '@/types';

import { DeleteButton } from './delete-button';
import EditButton from './edit-button';
import { PaymentButton } from './PaymentButton';

export default function PlannedTransactionsList({
  plannedTransactions,
  monthlyRequest,
}: {
  plannedTransactions: MonthlyPlannedTransactionDto[];
  monthlyRequest: MonthlyRequest;
}) {
  const t = useTranslations('Pages.Dashboard.PlanedTransactionsList');
  const session = useSession();
  const format = useFormatter();

  return (
    <ScrollArea className='h-[330px]'>
      <CardContent>
        {plannedTransactions.filter((e) => !e.ignore).length === 0 && (
          <div className='flex h-[306px] items-center justify-center'>
            <p className='text-center text-sm text-muted-foreground'>
              {t('Nothing')}
            </p>
          </div>
        )}
        {plannedTransactions
          .filter((f) => f.ignore !== true)
          .map((transaction, index) => (
            <div
              key={transaction.id}
              className={cn(
                'flex flex-col',
                transaction.isChanging && 'opacity-25'
              )}
            >
              <div>
                <div className='flex flex-row items-center justify-end space-x-2'>
                  <EditButton
                    plannedTransaction={transaction}
                    monthlyRequest={monthlyRequest}
                  />
                  <DeleteButton
                    plannedTransaction={transaction}
                    monthlyRequest={monthlyRequest}
                  />
                </div>
                <div className='flex flex-row items-center justify-between'>
                  <div>
                    <p className='text-md font-medium'>
                      {transaction.description}
                    </p>
                    <p className='text-xs text-muted-foreground'>
                      {format.dateTime(
                        parseISO(transaction.referringDate.toLocaleString()),
                        {
                          day: 'numeric',
                          month: 'long',
                          year: 'numeric',
                        }
                      )}
                    </p>
                  </div>
                  <div>
                    <p
                      className={cn(
                        'text-md font-bold',
                        transaction.type === TransactionType.Income &&
                          'text-green-600 dark:text-green-500',
                        transaction.type === TransactionType.Expense &&
                          'text-red-600 dark:text-red-500'
                      )}
                    >
                      {transaction.type === TransactionType.Income && '+'}
                      {transaction.amountType === AmountType.Variable && '~'}
                      {format.number(transaction.amount, {
                        style: 'currency',
                        currency: session.data?.preferred_currency,
                      })}
                    </p>
                    {transaction.paid && (
                      <p className='text-right text-xs text-muted-foreground'>
                        {format.number(transaction.paidAmount, {
                          style: 'currency',
                          currency: session.data?.preferred_currency,
                        })}
                      </p>
                    )}
                  </div>
                </div>
                <div className='mt-1 flex flex-row justify-between'>
                  <div className='mr-4'>
                    {transaction.tags.map((tag) => (
                      <Badge key={tag} variant='secondary' className='mr-1'>
                        {tag}
                      </Badge>
                    ))}
                  </div>
                  <div className='flex items-center'>
                    <PaymentButton
                      plannedTransaction={transaction}
                      monthlyRequest={monthlyRequest}
                    />
                  </div>
                </div>
              </div>
              {index !== plannedTransactions.length - 1 && (
                <Separator className='my-3' />
              )}
            </div>
          ))}
      </CardContent>
    </ScrollArea>
  );
}
