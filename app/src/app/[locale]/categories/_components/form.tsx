'use client';

import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Loader2 } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { z } from 'zod';

import BackButton from '@/components/back-button';
import DefaultError from '@/components/default-error';
import { Button } from '@/components/ui/button';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { useCreateCategory } from '@/data/categories/create-category';
import { useFetchCategory } from '@/data/categories/fetch-category';
import { useUpdateCategory } from '@/data/categories/update-category';
import { CategoryFormSchema } from '@/schemas';
import { FormActionType } from '@/types';

import CategoriesFormSkeleton from './skeleton';

type CategoriesFormProps = {
  formActionType: FormActionType;
  categoryId?: string;
};

export function CategoriesForm({
  formActionType,
  categoryId,
}: CategoriesFormProps) {
  const t = useTranslations('Pages.Categories.Form');

  const form = useForm<z.infer<typeof CategoryFormSchema>>({
    resolver: zodResolver(CategoryFormSchema),
    defaultValues: {
      name: '',
      color: '#E6FDC4',
    },
  });

  const {
    data: foundCategory,
    isSuccess,
    isLoading,
    isError,
    failureReason,
  } = useFetchCategory(categoryId!);

  const createMutation = useCreateCategory();
  const updateMutation = useUpdateCategory(categoryId!);

  const onSubmit = async (values: z.infer<typeof CategoryFormSchema>) => {
    const request = {
      name: values.name,
      hexColor: values.color,
    };

    if (formActionType === FormActionType.Create) {
      await createMutation.mutateAsync(request);
    } else {
      await updateMutation.mutateAsync(request);
    }
  };

  useEffect(() => {
    if (isSuccess) {
      form.reset({
        name: foundCategory.name,
        color: foundCategory.hexColor,
      });
    }
  }, [isSuccess, foundCategory, form]);

  if (isLoading) {
    return <CategoriesFormSkeleton />;
  }

  if (isError) {
    return (
      <DefaultError
        failureReason={failureReason}
        className='h-[calc(100vh-24rem)]'
      />
    );
  }

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)}>
        <div className='mb-6 flex flex-col space-y-4 md:flex-row md:space-x-4 md:space-y-0'>
          <FormField
            control={form.control}
            name='name'
            render={({ field }) => (
              <FormItem className='flex flex-col md:w-3/4'>
                <FormLabel>{t('Name')}</FormLabel>
                <FormControl>
                  <Input placeholder={t('NamePlaceholder')} {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name='color'
            render={({ field }) => (
              <FormItem className='flex flex-col md:w-1/4'>
                <FormLabel>{t('Color')}</FormLabel>
                <FormControl>
                  <Input
                    type='color'
                    placeholder={t('ColorPlaceholder')}
                    {...field}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>

        <div className='flex justify-end space-x-2'>
          <BackButton
            type='button'
            variant='outline'
            disabled={form.formState.isSubmitting}
          />
          <Button type='submit' disabled={form.formState.isSubmitting}>
            {form.formState.isSubmitting && (
              <Loader2 className='mr-2 h-4 w-4 animate-spin' />
            )}
            {formActionType === FormActionType.Create
              ? t('Create')
              : t('Update')}
          </Button>
        </div>
      </form>
    </Form>
  );
}
