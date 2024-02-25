'use client';

import { useState } from 'react';
import { Loader2, TrashIcon } from 'lucide-react';
import { useTranslations } from 'next-intl';

import { ByDeviceModal } from '@/components/by-device-modal';
import { Button } from '@/components/ui/button';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { useDeleteTransaction } from '@/data/transactions/delete-transaction';
import { MonthlyRequest, MonthlyTransactionDto } from '@/types';

export function DeleteButton({
  transaction,
  monthlyRequest,
}: {
  transaction: MonthlyTransactionDto;
  monthlyRequest: MonthlyRequest;
}) {
  const [isLoading, setIsLoading] = useState(false);
  const t = useTranslations('Pages.Dashboard.TransactionsList.DeleteButton');

  const deleteMutation = useDeleteTransaction();

  const handleClick = () => {
    setIsLoading(true);
    deleteMutation.mutate(
      {
        transactionId: transaction.id,
        monthlyRequest: monthlyRequest,
      },
      { onSettled: () => setIsLoading(false) }
    );
  };

  return (
    <Tooltip>
      <ByDeviceModal
        title={t('Title')}
        description={t('Description')}
        trigger={
          <TooltipTrigger className='flex flex-row' asChild>
            <Button
              variant='ghost'
              size='xs'
              aria-label='Delete transaction button'
            >
              <TrashIcon className='h-4 w-4' />
            </Button>
          </TooltipTrigger>
        }
      >
        <div className='flex flex-col space-y-2'>
          <Button
            disabled={isLoading}
            aria-label='Delete button'
            onClick={handleClick}
          >
            {t('Delete')}
          </Button>
          {isLoading && (
            <div className='mt-2 flex flex-row items-center justify-center text-muted-foreground'>
              <Loader2 className='mr-2 h-4 w-4 animate-spin' />
              <p>{t('Deleting')}</p>
            </div>
          )}
        </div>
      </ByDeviceModal>
      <TooltipContent side='bottom'>
        <p>{t('Tooltip')}</p>
      </TooltipContent>
    </Tooltip>
  );
}
