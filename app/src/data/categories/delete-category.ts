'use client';

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslations } from 'next-intl';

import { useToast } from '@/components/ui/use-toast';
import { deleteCategory } from '@/services/categories/delete-category';
import { getApiErrorTitle } from '@/shared/get-api-client-error';
import { CategoryDto, DeleteCategoryRequest, PaginatedData } from '@/types';

export function useDeleteCategory() {
  const queryClient = useQueryClient();
  const t = useTranslations('Pages.Categories.List.DeleteButton');
  const tShared = useTranslations('Shared');
  const { toast } = useToast();

  return useMutation({
    mutationFn: async (data: DeleteCategoryRequest) =>
      await deleteCategory(data),
    onSuccess: (_, variables) => {
      queryClient.setQueryData(
        ['categories'],
        (old: PaginatedData<CategoryDto> | undefined) => {
          if (!old) {
            return undefined;
          }

          return {
            ...old,
            data: old.data.filter((category) => category.id !== variables.id),
          };
        }
      );

      toast({
        title: t('ToastSuccessTitle'),
        description: t('ToastSuccessDescription'),
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
      await queryClient.invalidateQueries({ queryKey: ['categories'] });
    },
  });
}
