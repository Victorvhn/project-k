'use client';

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslations } from 'next-intl';
import { ulid } from 'ulid';

import { useToast } from '@/components/ui/use-toast';
import { useRouter } from '@/lib/navigation';
import { createCategory } from '@/services/categories/create-category';
import { getApiErrorTitle } from '@/shared/get-api-client-error';
import { CategoryDto, CreateCategoryRequest, PaginatedData } from '@/types';

export function useCreateCategory() {
  const queryClient = useQueryClient();
  const router = useRouter();
  const t = useTranslations('Pages.Categories.Form');
  const tShared = useTranslations('Shared');
  const { toast } = useToast();

  return useMutation({
    mutationFn: async (data: CreateCategoryRequest) =>
      await createCategory(data),
    onMutate: (variables) => {
      const optimisticCategory = {
        id: ulid(),
        ...variables,
      };

      queryClient.setQueryData(
        ['categories'],
        (old: PaginatedData<CategoryDto> | undefined) => {
          if (!old) {
            return undefined;
          }

          return {
            ...old,
            data: [...old.data, optimisticCategory],
            totalCount: old.totalCount + 1,
          };
        }
      );

      return { optimisticCategory };
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
        description: t('ToastSuccessCreate', { name: result.name }),
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
            data: old.data.filter(
              (category) => category.id !== context?.optimisticCategory.id
            ),
            totalCount: old.totalCount - 1,
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
    },
  });
}
