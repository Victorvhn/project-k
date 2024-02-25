'use client';

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslations } from 'next-intl';

import { useToast } from '@/components/ui/use-toast';
import { deleteTransaction } from '@/services/transactions/delete-transaction';
import { getApiErrorTitle } from '@/shared/get-api-client-error';
import {
  DeleteTransactionRequest,
  MonthlyPlannedTransactionDto,
  MonthlyTransactionDto,
} from '@/types';

export function useDeleteTransaction() {
  const queryClient = useQueryClient();
  const t = useTranslations(
    'Pages.Dashboard.PlanedTransactionsList.ClickableBadge'
  );
  const tShared = useTranslations('Shared');
  const { toast } = useToast();

  return useMutation({
    mutationFn: async (request: DeleteTransactionRequest) =>
      deleteTransaction(request),
    onSuccess: (_, { transactionId, monthlyRequest }) => {
      queryClient.setQueryData(
        [
          'monthly',
          'planned-transactions',
          { year: monthlyRequest.year, month: monthlyRequest.month },
        ],
        (oldData: MonthlyPlannedTransactionDto[] | undefined) => {
          if (!oldData) {
            return undefined;
          }

          return [
            ...oldData.map((f) => {
              if (f.paidTransactionId === transactionId) {
                return {
                  ...f,
                  paid: false,
                  isChanging: true,
                };
              }

              return f;
            }),
          ];
        }
      );

      queryClient.setQueryData(
        [
          'monthly',
          'transactions',
          { year: monthlyRequest.year, month: monthlyRequest.month },
        ],
        (oldData: MonthlyTransactionDto[] | undefined) => {
          if (!oldData) {
            return undefined;
          }

          return [
            ...oldData.map((f) => {
              if (f.id === transactionId) {
                return {
                  ...f,
                  isChanging: true,
                };
              }

              return f;
            }),
          ];
        }
      );

      toast({
        title: tShared('Success'),
        description: t('SuccessCancelling'),
      });
    },
    onError: async (error) => {
      toast({
        title: tShared('ApiCallError.Title'),
        description: tShared('ApiCallError.ErrorMessage', {
          error: getApiErrorTitle(error?.message),
        }),
      });
    },
    onSettled: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['monthly'],
      });
    },
  });
}
