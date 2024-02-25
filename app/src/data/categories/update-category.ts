'use client';

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslations } from 'next-intl';

import { useToast } from '@/components/ui/use-toast';
import { useRouter } from '@/lib/navigation';
import { updateCategory } from '@/services/categories/update-category';
import { getApiErrorTitle } from '@/shared/get-api-client-error';
import { CategoryDto, PaginatedData, UpdateCategoryRequest } from '@/types';

export function useUpdateCategory(categoryId: string) {
  const queryClient = useQueryClient();
  const router = useRouter();
  const t = useTranslations('Pages.Categories.Form');
  const tShared = useTranslations('Shared');
  const { toast } = useToast();

  return useMutation({
    mutationFn: async (data: UpdateCategoryRequest) =>
      await updateCategory(categoryId, data),
    onMutate: (variables) => {
      const optimisticCategory = {
        id: categoryId,
        ...variables,
      };

      let oldData: CategoryDto | undefined;
      queryClient.setQueryData(
        ['categories'],
        (old: PaginatedData<CategoryDto> | undefined) => {
          if (!old) {
            return undefined;
          }

          return {
            ...old,
            data: old.data.map((category) => {
              if (category.id === categoryId) {
                oldData = category;
                return optimisticCategory;
              }

              return category;
            }),
          };
        }
      );

      return { optimisticCategory, oldData };
    },
    onSuccess: (result, _, context) => {
      queryClient.setQueryData(
        ['categories'],
        (old: PaginatedData<CategoryDto> | undefined) => {
          if (!old) {
            return undefined;
          }

          return {
            ...old,
            data: old.data.map((category) =>
              category.id === context.optimisticCategory.id ? result : category
            ),
          };
        }
      );

      toast({
        title: t('ToastSuccessTitle'),
        description: t('ToastSuccessUpdate', { name: result.name }),
      });

      router.back();
    },
    onError: async (error, _, context) => {
      queryClient.setQueryData(
        ['categories'],
        (old: PaginatedData<CategoryDto> | undefined) => {
          if (!old) {
            return undefined;
          }

          return {
            ...old,
            data: old.data.map((category) =>
              category.id === categoryId ? context?.oldData : category
            ),
          };
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
      await queryClient.invalidateQueries({ queryKey: ['categories'] });
      await queryClient.invalidateQueries({
        queryKey: ['category', categoryId],
      });
    },
  });
}
