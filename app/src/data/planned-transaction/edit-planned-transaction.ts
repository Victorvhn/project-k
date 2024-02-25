'use client';

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslations } from 'next-intl';

import { useToast } from '@/components/ui/use-toast';
import { useRouter } from '@/lib/navigation';
import { updatePlannedTransaction } from '@/services/planned-transactions/update-planned-transaction';
import { getApiErrorTitle } from '@/shared/get-api-client-error';
import { MonthlyPlannedTransactionDto, MonthlyRequest } from '@/types';
import { EditPlannedTransactionRequest } from '@/types/requests/planned-transactions/edit-planned-transaction-request';

export function useEditPlannedTransaction(
  plannedTransactionId: string,
  monthlyRequest: MonthlyRequest
) {
  const queryClient = useQueryClient();
  const t = useTranslations('Pages.PlannedTransactions.Edit');
  const tShared = useTranslations('Shared');
  const { toast } = useToast();
  const router = useRouter();

  return useMutation({
    mutationFn: async (request: EditPlannedTransactionRequest) =>
      updatePlannedTransaction(plannedTransactionId, request),
    onSuccess: (result) => {
      queryClient.setQueryData(
        [
          'monthly',
          'planned-transactions',
          {
            year: Number(monthlyRequest.year),
            month: Number(monthlyRequest.month),
          },
        ],
        (old: MonthlyPlannedTransactionDto[] | undefined) => {
          if (!old) {
            return undefined;
          }

          const oldPlannedTransaction = old.find(
            (f) => f.id === plannedTransactionId
          );

          if (!oldPlannedTransaction) {
            return old;
          }

          const updated: MonthlyPlannedTransactionDto = {
            id: result.id,
            description: result.description,
            amount: result.amount,
            amountType: result.amountType,
            type: result.type,
            recurrence: result.recurrence,
            referringDate: result.startsAt,
            startsAt: result.startsAt,
            endsAt: result.endsAt,
            categoryId: result.categoryId,
            paid: oldPlannedTransaction.paid,
            paidAmount: oldPlannedTransaction.paidAmount,
            tags: oldPlannedTransaction.tags,
            ignore: oldPlannedTransaction.ignore,
            isChanging: true,
          };

          return [
            ...old.map((plannedTransaction) => {
              if (plannedTransaction.id === plannedTransactionId) {
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
        queryKey: ['planned-transaction', plannedTransactionId, monthlyRequest],
      });
    },
  });
}
