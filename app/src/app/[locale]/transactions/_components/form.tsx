'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { format, parseISO } from 'date-fns';
import { enUS, ptBR } from 'date-fns/locale';
import { CalendarIcon, Loader2 } from 'lucide-react';
import { useLocale, useTranslations } from 'next-intl';
import { z } from 'zod';

import BackButton from '@/components/back-button';
import { CategoryCombo } from '@/components/category-combo';
import { CurrencyInput } from '@/components/currency-input';
import DefaultError from '@/components/default-error';
import { Button } from '@/components/ui/button';
import { Calendar } from '@/components/ui/calendar';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover';
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group';
import { useCreateTransaction } from '@/data/transactions/create-transaction';
import { useEditTransaction } from '@/data/transactions/edit-transaction';
import { useFetchTransaction } from '@/data/transactions/fetch-transaction';
import { cn } from '@/lib/utils';
import { TransactionsFormSchema } from '@/schemas';
import {
  CreateTransactionRequest,
  MonthlyRequest,
  PlannedTransactionDto,
  TransactionType,
} from '@/types';

import { PlannedTransactionsCombo } from './planned-transacitons-combo';
import TransactionsFormSkeleton from './skeleton';

type TransactionsFormProps = {
  action: 'create' | 'update';
  monthlyRequest: MonthlyRequest;
  transactionId?: string;
};

export default function TransactionsForm({
  action,
  monthlyRequest,
  transactionId,
}: TransactionsFormProps) {
  const [isDatePickerOpen, setIsDatePickerOpen] = useState(false);

  const t = useTranslations('Pages.Transactions.Form');
  const tShared = useTranslations('Shared');
  const locale = useLocale();

  const {
    data: foundTransaction,
    isSuccess,
    isLoading,
    isError,
    failureReason,
  } = useFetchTransaction(transactionId);
  const createMutation = useCreateTransaction(monthlyRequest);
  const editMutation = useEditTransaction(transactionId!, monthlyRequest);

  const form = useForm<z.infer<typeof TransactionsFormSchema>>({
    resolver: zodResolver(TransactionsFormSchema),
    defaultValues: {
      plannedTransactionId: null,
      description: '',
      amount: undefined,
      type: TransactionType.Expense,
      categoryId: null,
      paidAt: new Date(),
    },
  });

  useEffect(() => {
    const formData = sessionStorage.getItem('form-data');

    if (!formData) {
      return;
    }

    form.reset(JSON.parse(formData));

    sessionStorage.removeItem('form-data');
  }, [form]);

  useEffect(() => {
    if (isSuccess) {
      form.reset({
        plannedTransactionId: foundTransaction.plannedTransactionId,
        description: foundTransaction.description,
        amount: foundTransaction.amount,
        type: foundTransaction.type,
        categoryId: foundTransaction.categoryId,
        paidAt: parseISO(foundTransaction.paidAt),
      });
    }
  }, [isSuccess, foundTransaction, form, tShared]);

  const onSubmit = async (values: z.infer<typeof TransactionsFormSchema>) => {
    const data = {
      description: values.description,
      amount: values.amount,
      type: values.type,
      categoryId: values.categoryId,
      paidAt: format(values.paidAt, 'yyyy-MM-dd'),
      plannedTransactionId: values.plannedTransactionId,
    };

    if (action === 'create') {
      await createMutation.mutateAsync(data as CreateTransactionRequest);
    } else {
      await editMutation.mutateAsync(data as CreateTransactionRequest);
    }
  };

  const handleChangePlannedTransaction = (
    plannedTransaction?: PlannedTransactionDto
  ) => {
    if (!plannedTransaction) {
      return;
    }

    form.setValue('description', plannedTransaction.description);
    form.setValue('amount', plannedTransaction.amount);
    form.setValue('type', plannedTransaction.type);
    form.setValue('categoryId', plannedTransaction.categoryId ?? null);
  };

  if (isLoading) {
    return <TransactionsFormSkeleton />;
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
      <form onSubmit={form.handleSubmit(onSubmit)} className='mb-6 '>
        <div className='mb-8 flex flex-col space-y-6 md:grid md:grid-cols-4 md:gap-4'>
          <FormField
            control={form.control}
            name='plannedTransactionId'
            render={({ field }) => (
              <FormItem className='flex flex-col justify-end md:col-span-2'>
                <FormLabel
                  className={cn(action === 'update' && 'text-muted-foreground')}
                >
                  {t('PlannedTransaction')}
                </FormLabel>
                <PlannedTransactionsCombo
                  fieldValue={field.value}
                  onChange={field.onChange}
                  setFormValue={handleChangePlannedTransaction}
                  className='w-full'
                  disabled={action === 'update'}
                />
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name='description'
            render={({ field }) => (
              <FormItem className='md:col-span-4'>
                <FormLabel>{t('Description')}</FormLabel>
                <FormControl>
                  <Input {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name='amount'
            render={({ field }) => (
              <FormItem className='md:col-span-3'>
                <FormLabel>{t('Amount')}</FormLabel>
                <FormControl>
                  <CurrencyInput {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name='type'
            render={({ field }) => (
              <FormItem className='space-y-3 md:col-span-1'>
                <p className='text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70'>
                  {t('Type')}
                </p>
                <FormControl>
                  <RadioGroup
                    onValueChange={field.onChange}
                    defaultValue={field.value}
                    className='flex flex-col space-y-1'
                  >
                    <FormItem className='flex items-center space-x-3 space-y-0'>
                      <FormControl>
                        <RadioGroupItem
                          value={TransactionType.Income}
                          checked={field.value === TransactionType.Income}
                        />
                      </FormControl>
                      <FormLabel className='font-normal'>
                        {t('Income')}
                      </FormLabel>
                    </FormItem>
                    <FormItem className='flex items-center space-x-3 space-y-0'>
                      <FormControl>
                        <RadioGroupItem
                          value={TransactionType.Expense}
                          checked={field.value === TransactionType.Expense}
                        />
                      </FormControl>
                      <FormLabel className='font-normal'>
                        {t('Expense')}
                      </FormLabel>
                    </FormItem>
                  </RadioGroup>
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name='categoryId'
            render={({ field }) => (
              <FormItem className='flex flex-col justify-end md:col-span-2'>
                <FormLabel>{t('Category')}</FormLabel>
                <CategoryCombo
                  fieldValue={field.value}
                  onChange={field.onChange}
                  className='w-full'
                />
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name='paidAt'
            render={({ field }) => (
              <FormItem className='flex flex-col md:col-span-2'>
                <FormLabel>{t('Paid at')}</FormLabel>
                <Popover
                  open={isDatePickerOpen}
                  onOpenChange={setIsDatePickerOpen}
                >
                  <PopoverTrigger asChild>
                    <FormControl>
                      <Button
                        variant={'outline'}
                        className={cn(
                          'pl-3 text-left font-normal',
                          !field.value && 'text-muted-foreground'
                        )}
                      >
                        {field.value ? (
                          format(field.value, 'PPP', {
                            locale: locale === 'en' ? enUS : ptBR,
                          })
                        ) : (
                          <span>{t('Select date')}</span>
                        )}
                        <CalendarIcon className='ml-auto h-4 w-4 opacity-50' />
                      </Button>
                    </FormControl>
                  </PopoverTrigger>
                  <PopoverContent className='w-auto p-0' align='start'>
                    <Calendar
                      mode='single'
                      selected={field.value}
                      onSelect={(date) => {
                        field.onChange(date);
                        setIsDatePickerOpen(false);
                      }}
                    />
                  </PopoverContent>
                </Popover>
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
            {action === 'create' ? t('Create') : t('Edit')}
          </Button>
        </div>
      </form>
    </Form>
  );
}
