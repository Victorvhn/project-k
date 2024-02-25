'use client';

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { compareAsc, format, parseISO } from 'date-fns';
import { useTranslations } from 'next-intl';
import { ulid } from 'ulid';

import { useToast } from '@/components/ui/use-toast';
import { payPlannedTransaction } from '@/services/monthly/pay-planned-transaction';
import { getApiErrorTitle } from '@/shared/get-api-client-error';
import {
  MonthlyPlannedTransactionDto,
  MonthlyTransactionDto,
  PayPlannedTransactionRequest,
} from '@/types';

export function usePayPlannedTransaction() {
  const queryClient = useQueryClient();
  const t = useTranslations(
    'Pages.Dashboard.PlanedTransactionsList.ClickableBadge'
  );
  const tShared = useTranslations('Shared');
  const { toast } = useToast();

  return useMutation({
    mutationFn: async (request: PayPlannedTransactionRequest) =>
      payPlannedTransaction(request),
    onSuccess: (_, variables) => {
      queryClient.setQueryData(
        [
          'monthly',
          'planned-transactions',
          {
            month: variables.monthlyRequest.month,
            year: variables.monthlyRequest.year,
          },
        ],
        (oldData: MonthlyPlannedTransactionDto[] | undefined) => {
          if (!oldData) {
            return undefined;
          }

          return oldData.map((f) => {
            if (f.id === variables.plannedTransactionId) {
              return {
                ...f,
                paid: true,
                paidAmount: variables.amount ?? f.amount,
              };
            }

            return f;
          });
        }
      );

      const paidPlannedTransaction = queryClient
        .getQueryData<MonthlyPlannedTransactionDto[] | undefined>([
          'monthly',
          'planned-transactions',
          {
            month: variables.monthlyRequest.month,
            year: variables.monthlyRequest.year,
          },
        ])
        ?.find((f) => f.id === variables.plannedTransactionId);

      if (!paidPlannedTransaction) {
        return;
      }

      queryClient.setQueryData(
        [
          'monthly',
          'transactions',
          {
            year: Number(variables.monthlyRequest.year),
            month: Number(variables.monthlyRequest.month),
          },
        ],
        (old: MonthlyTransactionDto[] | undefined) => {
          if (!old) {
            return undefined;
          }

          const paidDate = new Date(
            variables.monthlyRequest.year,
            variables.monthlyRequest.month - 1,
            new Date().getDate()
          );

          const created: MonthlyTransactionDto = {
            id: ulid(),
            description: paidPlannedTransaction.description,
            amount: paidPlannedTransaction.amount,
            type: paidPlannedTransaction.type,
            paidAt: format(paidDate, 'yyyy-MM-dd'),
            categoryId: paidPlannedTransaction.categoryId,
            plannedTransactionId: paidPlannedTransaction.id,
            isChanging: true,
          };

          let added = false;

          return [
            ...old.map((transaction) => {
              if (added) {
                return transaction;
              }

              const paidAt = parseISO(transaction.paidAt.toLocaleString());
              const compare = compareAsc(paidAt, paidDate);

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
        description: t('SuccessPaying'),
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
      await queryClient.invalidateQueries({ queryKey: ['monthly'] });
    },
  });
}
