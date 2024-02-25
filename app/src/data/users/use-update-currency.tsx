'use client';

import { useMutation } from '@tanstack/react-query';
import { useSession } from 'next-auth/react';
import { useTranslations } from 'next-intl';

import { useToast } from '@/components/ui/use-toast';
import { updateCurrency } from '@/services/users/update-currency';
import { getApiErrorTitle } from '@/shared/get-api-client-error';
import { UpdateCurrencyRequest } from '@/types';

export function useUpdateCurrency(userId: string) {
  const { toast } = useToast();
  const { data, update } = useSession();
  const tShared = useTranslations('Shared');

  return useMutation({
    mutationFn: async (request: UpdateCurrencyRequest) =>
      updateCurrency(userId, request),
    onSuccess: async (result) => {
      await update({
        ...data,
        preferred_currency: result.currency.toUpperCase(),
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
  });
}
