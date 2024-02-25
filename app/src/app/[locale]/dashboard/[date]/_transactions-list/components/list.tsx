import { parseISO } from 'date-fns';
import { useSession } from 'next-auth/react';
import { useFormatter, useTranslations } from 'next-intl';

import { CardContent } from '@/components/ui/card';
import { ScrollArea } from '@/components/ui/scroll-area';
import { Separator } from '@/components/ui/separator';
import { cn } from '@/lib/utils';
import {
  MonthlyRequest,
  MonthlyTransactionDto,
  TransactionType,
} from '@/types';

import { DeleteButton } from './delete-button';
import { EditButton } from './edit-button';

export default function TransactionsList({
  transactions,
  monthlyRequest,
}: {
  transactions: MonthlyTransactionDto[];
  monthlyRequest: MonthlyRequest;
}) {
  const t = useTranslations('Pages.Dashboard.TransactionsList');
  const session = useSession();
  const format = useFormatter();

  return (
    <ScrollArea className='h-[330px]'>
      <CardContent>
        {transactions.length === 0 && (
          <div className='flex h-[306px] items-center justify-center'>
            <p className='text-center text-sm text-muted-foreground'>
              {t('Nothing')}
            </p>
          </div>
        )}
        {transactions.map((transaction, index) => (
          <div
            key={transaction.id}
            className={cn(
              'flex h-full flex-col',
              transaction.isChanging && 'opacity-25'
            )}
          >
            <div>
              <div className='flex flex-row items-center justify-end space-x-2'>
                <EditButton
                  transactionId={transaction.id}
                  monthlyRequest={monthlyRequest}
                />
                <DeleteButton
                  transaction={transaction}
                  monthlyRequest={monthlyRequest}
                />
              </div>
              <div className='flex flex-row items-center justify-between'>
                <div>
                  <p className='text-md font-medium'>
                    {transaction.description}
                  </p>
                  <p className='text-xs text-muted-foreground'>
                    {format.dateTime(parseISO(transaction.paidAt), {
                      day: 'numeric',
                      month: 'long',
                      year: 'numeric',
                    })}
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
                    {format.number(transaction.amount, {
                      style: 'currency',
                      currency: session.data?.preferred_currency,
                    })}
                  </p>
                  {transaction.plannedTransaction && (
                    <p className='text-right text-xs text-muted-foreground'>
                      {format.number(transaction.plannedTransaction.amount, {
                        style: 'currency',
                        currency: session.data?.preferred_currency,
                      })}
                    </p>
                  )}
                </div>
              </div>
            </div>
            {index !== transactions.length - 1 && (
              <Separator className='my-3' />
            )}
          </div>
        ))}
      </CardContent>
    </ScrollArea>
  );
}
