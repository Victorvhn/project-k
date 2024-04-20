'use client';

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { compareAsc, parseISO } from 'date-fns';
import { useTranslations } from 'next-intl';

import { useToast } from '@/components/ui/use-toast';
import { useRouter } from '@/lib/navigation';
import { createPlannedTransaction } from '@/services/planned-transactions/create-planned-transaction';
import { getApiErrorTitle } from '@/shared/get-api-client-error';
import { MonthlyPlannedTransactionDto, MonthlyRequest } from '@/types';
import { CreatePlannedTransactionRequest } from '@/types/requests/planned-transactions/create-planned-transaction-request';

export function useCreatePlannedTransaction(monthlyRequest: MonthlyRequest) {
  const queryClient = useQueryClient();
  const t = useTranslations('Pages.PlannedTransactions.Add');
  const tShared = useTranslations('Shared');
  const { toast } = useToast();
  const router = useRouter();

  return useMutation({
    mutationFn: async (request: CreatePlannedTransactionRequest) =>
      createPlannedTransaction(request),
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
          const created: MonthlyPlannedTransactionDto = {
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
            paid: false,
            paidAmount: 0,
            tags: [],
            ignore: false,
            isChanging: true,
          };

          if (!old) {
            return [created];
          }

          const resultStartsAt = parseISO(result.startsAt.toLocaleString());

          const idx = old.findIndex((transaction) => {
            const paidAt = parseISO(transaction.startsAt.toLocaleString());
            const compare = compareAsc(paidAt, resultStartsAt);

            return compare !== 1;
          });

          old.splice(idx, 0, created)

          return old;
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
