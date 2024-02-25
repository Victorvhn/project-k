'use client';

import { useCallback, useState } from 'react';
import { Loader2 } from 'lucide-react';
import { useTranslations } from 'next-intl';

import { ButtonBadge } from '@/components/button-badge';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { useDeleteTransaction } from '@/data/transactions/delete-transaction';
import { MonthlyPlannedTransactionDto, MonthlyRequest } from '@/types';

export default function CancelPaymentButton({
  plannedTransaction,
  monthlyRequest,
}: {
  plannedTransaction: MonthlyPlannedTransactionDto;
  monthlyRequest: MonthlyRequest;
}) {
  const t = useTranslations(
    'Pages.Dashboard.PlanedTransactionsList.ClickableBadge'
  );
  const [isLoading, setIsLoading] = useState(false);

  const deleteMutation = useDeleteTransaction();

  const sendRequest = useCallback(() => {
    setIsLoading(true);

    deleteMutation.mutate(
      {
        transactionId: plannedTransaction.paidTransactionId!,
        monthlyRequest: monthlyRequest,
      },
      { onSettled: () => setIsLoading(false) }
    );
  }, [deleteMutation, plannedTransaction, monthlyRequest]);

  return (
    <Tooltip>
      <TooltipTrigger className='flex flex-row' asChild>
        <ButtonBadge
          disabled={isLoading}
          aria-label={t('Cancel')}
          onClick={sendRequest}
        >
          {isLoading && <Loader2 className='mr-2 h-4 w-4 animate-spin' />}
          {t('Paid')}
        </ButtonBadge>
      </TooltipTrigger>
      <TooltipContent side='bottom'>
        <p>{t('Cancel')}</p>
      </TooltipContent>
    </Tooltip>
  );
}
