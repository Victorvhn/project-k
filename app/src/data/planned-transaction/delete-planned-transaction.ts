'use client';

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslations } from 'next-intl';

import { useToast } from '@/components/ui/use-toast';
import { deletePlannedTransaction } from '@/services/planned-transactions/delete-planned-transaction';
import { getApiErrorTitle } from '@/shared/get-api-client-error';
import {
  DeletePlannedTransactionRequest,
  MonthlyPlannedTransactionDto,
} from '@/types';

export function useDeletePlannedTransaction() {
  const queryClient = useQueryClient();
  const t = useTranslations(
    'Pages.Dashboard.PlanedTransactionsList.DeleteButton'
  );
  const tShared = useTranslations('Shared');
  const { toast } = useToast();

  return useMutation({
    mutationFn: async (request: DeletePlannedTransactionRequest) =>
      deletePlannedTransaction(request),
    onMutate: async (variables) => {
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
              return { ...f, isChanging: true };
            }

            return f;
          });
        }
      );
    },
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

          return oldData.filter((f) => f.id !== variables.plannedTransactionId);
        }
      );

      toast({
        title: tShared('Success'),
        description: t('SuccessDeleting'),
      });
    },
    onError: async (error, variables) => {
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
              return { ...f, isChanging: false };
            }

            return f;
          });
        }
      );

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
