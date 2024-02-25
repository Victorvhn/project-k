'use client';

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslations } from 'next-intl';

import { useToast } from '@/components/ui/use-toast';
import { useRouter } from '@/lib/navigation';
import { editTransaction } from '@/services/transactions/edit-transaction';
import { getApiErrorTitle } from '@/shared/get-api-client-error';
import {
  CreateTransactionRequest,
  MonthlyRequest,
  MonthlyTransactionDto,
} from '@/types';

export function useEditTransaction(
  transactionId: string,
  monthlyRequest: MonthlyRequest
) {
  const queryClient = useQueryClient();
  const t = useTranslations('Pages.Transactions.Edit');
  const tShared = useTranslations('Shared');
  const { toast } = useToast();
  const router = useRouter();

  return useMutation({
    mutationFn: async (request: CreateTransactionRequest) =>
      editTransaction(transactionId!, request),
    onSuccess: (result) => {
      queryClient.setQueryData(
        [
          'monthly',
          'transactions',
          {
            year: Number(monthlyRequest.year),
            month: Number(monthlyRequest.month),
          },
        ],
        (old: MonthlyTransactionDto[] | undefined) => {
          if (!old) {
            return undefined;
          }

          const oldTransaction = old.find((f) => f.id === transactionId);

          if (!oldTransaction) {
            return old;
          }

          const updated: MonthlyTransactionDto = {
            id: result.id,
            description: result.description,
            amount: result.amount,
            type: result.type,
            paidAt: result.paidAt,
            categoryId: result.categoryId,
            plannedTransactionId: result.plannedTransactionId,
            isChanging: true,
          };

          return [
            ...old.map((plannedTransaction) => {
              if (plannedTransaction.id === transactionId) {
                return updated;
              }

              return plannedTransaction;
            }),
          ];
        }
      );

      toast({
        title: tShared('Success'),
        description: t('Success'),
      });

      router.back();
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
      await queryClient.invalidateQueries({
        queryKey: ['transaction', transactionId],
      });
    },
  });
}
