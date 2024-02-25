'use client';

import { useMutation } from '@tanstack/react-query';
import { signOut, useSession } from 'next-auth/react';
import { useTranslations } from 'next-intl';

import { useToast } from '@/components/ui/use-toast';
import { deleteUser } from '@/services/users/delete';
import { getApiErrorTitle } from '@/shared/get-api-client-error';

export function useDeleteUser() {
  const { toast } = useToast();
  const { data } = useSession();
  const tShared = useTranslations('Shared');

  return useMutation({
    mutationFn: async () => deleteUser(data!.user_id),
    onSuccess: async () => {
      await signOut();
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
