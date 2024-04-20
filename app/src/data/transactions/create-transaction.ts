'use client';

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { compareAsc, parseISO } from 'date-fns';
import { useTranslations } from 'next-intl';

import { useToast } from '@/components/ui/use-toast';
import { useRouter } from '@/lib/navigation';
import { createTransaction } from '@/services/transactions/create-transaction';
import { getApiErrorTitle } from '@/shared/get-api-client-error';
import {
  CreateTransactionRequest,
  MonthlyRequest,
  MonthlyTransactionDto,
} from '@/types';

export function useCreateTransaction(monthlyRequest: MonthlyRequest) {
  const queryClient = useQueryClient();
  const t = useTranslations('Pages.Transactions.Add');
  const tShared = useTranslations('Shared');
  const { toast } = useToast();
  const router = useRouter();

  return useMutation({
    mutationFn: async (request: CreateTransactionRequest) =>
      createTransaction(request),
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

          const created: MonthlyTransactionDto = {
            id: result.id,
            description: result.description,
            amount: result.amount,
            type: result.type,
            paidAt: result.paidAt,
            categoryId: result.categoryId,
            plannedTransactionId: result.plannedTransactionId,
            isChanging: true,
          };

          const resultPaidAt = parseISO(result.paidAt.toLocaleString());
          let added = false;

          return [
            ...old.map((transaction) => {
              if (added) {
                return transaction;
              }

              const paidAt = parseISO(transaction.paidAt.toLocaleString());
              const compare = compareAsc(paidAt, resultPaidAt);

              if (compare === 1) {
                return transaction;
              } else {
                added = true;
                return created;
              }
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
      queryClient.invalidateQueries({
        queryKey: ['monthly'],
      });
    },
  });
}
