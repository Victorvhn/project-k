'use client';

import { useEffect, useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import {
  format,
  getDaysInMonth,
  lastDayOfMonth,
  parseISO,
  subDays,
  subMonths,
} from 'date-fns';
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
import { useCreatePlannedTransaction } from '@/data/planned-transaction/create-planned-transaction';
import { useEditPlannedTransaction } from '@/data/planned-transaction/edit-planned-transaction';
import { useFetchPlannedTransaction } from '@/data/planned-transaction/fetch-planned-transaction';
import { cn } from '@/lib/utils';
import { PlannedTransactionsFormSchema } from '@/schemas';
import {
  ActionType,
  AmountType,
  MonthlyRequest,
  Recurrence,
  TransactionType,
} from '@/types';

import PlannedTransactionsFormSkeleton from './skeleton';

type PlannedTransactionsFormProps = {
  action: 'create' | 'update';
  monthlyRequest: MonthlyRequest;
  plannedTransactionId?: string;
  updateType?: ActionType;
};

export default function PlannedTransactionsForm({
  action,
  plannedTransactionId,
  monthlyRequest,
  updateType,
}: PlannedTransactionsFormProps) {
  const [isStartsAtDatePickerOpen, setIsStartsAtDatePickerOpen] =
    useState(false);
  const [isEndsAtDatePickerOpen, setIsEndsAtDatePickerOpen] = useState(false);

  const t = useTranslations('Pages.PlannedTransactions.Form');
  const locale = useLocale();

  const {
    data: foundPlannedTransaction,
    isSuccess,
    isLoading,
    isError,
    failureReason,
  } = useFetchPlannedTransaction(plannedTransactionId, monthlyRequest);

  const createMutation = useCreatePlannedTransaction(monthlyRequest);
  const editMutation = useEditPlannedTransaction(
    plannedTransactionId!,
    monthlyRequest
  );

  const form = useForm<z.infer<typeof PlannedTransactionsFormSchema>>({
    resolver: zodResolver(PlannedTransactionsFormSchema),
    defaultValues: {
      description: '',
      amount: undefined,
      amountType: AmountType.Fixed,
      type: TransactionType.Expense,
      recurrence: Recurrence.Monthly,
      startsAt: new Date(),
      categoryId: null,
    },
  });

  const changeEndsAtToEnsureDayIsTheSame = (date: Date | undefined) => {
    const endsAt = form.getValues('endsAt');

    if (!endsAt || !date) {
      return;
    }

    const daysInMonth = getDaysInMonth(endsAt);
    const newEndsAt = new Date(
      endsAt.getFullYear(),
      endsAt.getMonth(),
      date.getDate() > daysInMonth ? daysInMonth : date.getDate()
    );

    form.setValue('endsAt', newEndsAt);
  };

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
        description: foundPlannedTransaction.description,
        amount: foundPlannedTransaction.amount,
        amountType: foundPlannedTransaction.amountType,
        type: foundPlannedTransaction.type,
        recurrence: foundPlannedTransaction.recurrence,
        startsAt: parseISO(foundPlannedTransaction.startsAt),
        categoryId: foundPlannedTransaction.categoryId ?? undefined,
        endsAt: foundPlannedTransaction.endsAt
          ? parseISO(foundPlannedTransaction.endsAt)
          : undefined,
      });
    }
  }, [isSuccess, foundPlannedTransaction, form]);

  const onSubmit = async (
    values: z.infer<typeof PlannedTransactionsFormSchema>
  ) => {
    const data = {
      description: values.description,
      amount: values.amount,
      amountType: values.amountType,
      type: values.type,
      recurrence: values.recurrence,
      categoryId: values.categoryId,
      startsAt: format(values.startsAt, 'yyyy-MM-dd'),
      endsAt: values.endsAt ? format(values.endsAt, 'yyyy-MM-dd') : null,
    };

    if (action === 'create') {
      await createMutation.mutateAsync(data);
    } else {
      const updateData = {
        ...data,
        actionType: updateType!,
        year: monthlyRequest!.year,
        month: monthlyRequest!.month,
      };

      await editMutation.mutateAsync(updateData);
    }
  };

  const disableStartsAtDates = useMemo(() => {
    if (action === 'create') {
      return undefined;
    }

    if (
      updateType === ActionType.FromNowOn ||
      updateType === ActionType.JustOne
    ) {
      const providedDate = subMonths(
        new Date(monthlyRequest.year, monthlyRequest.month, 1),
        1
      );
      const endOfTheMonth = lastDayOfMonth(providedDate);
      return {
        before: providedDate,
        after: endOfTheMonth,
      };
    }

    if (foundPlannedTransaction?.isThereAnyCustomPlannedTransaction) {
      const currentDate = form.getValues('startsAt');
      const startOfTheMonth = parseISO(format(currentDate, 'yyyy-MM-01'));
      const endOfTheMonth = lastDayOfMonth(currentDate);

      return {
        before: startOfTheMonth,
        after: endOfTheMonth,
      };
    }
  }, [action, updateType, form, foundPlannedTransaction, monthlyRequest]);

  if (isLoading) {
    return <PlannedTransactionsFormSkeleton />;
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
            name='amountType'
            render={({ field }) => (
              <FormItem className='space-y-3 md:col-span-1'>
                <p className='text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70'>
                  {t('AmountType')}
                </p>
                <FormControl>
                  <RadioGroup
                    onValueChange={field.onChange}
                    defaultValue={field.value}
                    className='flex flex-col space-y-1'
                    disabled={
                      action === 'update' && updateType === ActionType.JustOne
                    }
                  >
                    <FormItem className='flex items-center space-x-3 space-y-0'>
                      <FormControl>
                        <RadioGroupItem
                          value={AmountType.Fixed}
                          checked={field.value === AmountType.Fixed}
                        />
                      </FormControl>
                      <FormLabel className='font-normal'>
                        {t('Fixed')}
                      </FormLabel>
                    </FormItem>
                    <FormItem className='flex items-center space-x-3 space-y-0'>
                      <FormControl>
                        <RadioGroupItem
                          value={AmountType.Variable}
                          checked={field.value === AmountType.Variable}
                        />
                      </FormControl>
                      <FormLabel className='font-normal'>
                        {t('Variable')}
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
                    disabled={
                      action === 'update' && updateType === ActionType.JustOne
                    }
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
            name='recurrence'
            render={({ field }) => (
              <FormItem className='space-y-3 md:col-span-1'>
                <p className='text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70'>
                  {t('Recurrence')}
                </p>
                <FormControl>
                  <RadioGroup
                    onValueChange={field.onChange}
                    defaultValue={field.value}
                    className='flex flex-col space-y-1'
                    disabled={
                      action === 'update' && updateType === ActionType.JustOne
                    }
                  >
                    <FormItem className='flex items-center space-x-3 space-y-0'>
                      <FormControl>
                        <RadioGroupItem
                          value={Recurrence.Monthly}
                          checked={field.value === Recurrence.Monthly}
                        />
                      </FormControl>
                      <FormLabel className='font-normal'>
                        {t('Monthly')}
                      </FormLabel>
                    </FormItem>
                    <FormItem className='flex items-center space-x-3 space-y-0'>
                      <FormControl>
                        <RadioGroupItem
                          value={Recurrence.Annual}
                          checked={field.value === Recurrence.Annual}
                        />
                      </FormControl>
                      <FormLabel className='font-normal'>
                        {t('Annual')}
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
                  disabled={
                    action === 'update' && updateType === ActionType.JustOne
                  }
                />
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name='startsAt'
            render={({ field }) => (
              <FormItem className='flex flex-col md:col-span-2'>
                <FormLabel>{t('StartsAt')}</FormLabel>
                <Popover
                  open={isStartsAtDatePickerOpen}
                  onOpenChange={setIsStartsAtDatePickerOpen}
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
                      required
                      disabled={disableStartsAtDates}
                      defaultMonth={subDays(
                        new Date(monthlyRequest.year, monthlyRequest.month, 1),
                        1
                      )}
                      selected={field.value}
                      onSelect={(date) => {
                        field.onChange(date);
                        changeEndsAtToEnsureDayIsTheSame(date);
                        setIsStartsAtDatePickerOpen(false);
                      }}
                      footer={
                        action === 'update' && (
                          <div className='mt-2 w-[252px]'>
                            <p className='break-words text-xs text-muted-foreground'>
                              {/* TODO: TRANSLATE */}
                              You cannot change the month/year that the planned
                              transaction should start but you can change the
                              day
                            </p>
                          </div>
                        )
                      }
                    />
                  </PopoverContent>
                </Popover>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name='endsAt'
            render={({ field }) => (
              <FormItem className='flex flex-col md:col-span-2'>
                <FormLabel>{t('EndsAt')}</FormLabel>
                <Popover
                  open={isEndsAtDatePickerOpen}
                  onOpenChange={setIsEndsAtDatePickerOpen}
                >
                  <PopoverTrigger asChild>
                    <FormControl>
                      <Button
                        variant={'outline'}
                        className={cn(
                          'pl-3 text-left font-normal',
                          !field.value && 'text-muted-foreground'
                        )}
                        disabled={
                          action === 'update' &&
                          updateType === ActionType.JustOne
                        }
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
                      disabled={(date: Date) => {
                        return (
                          date.getDate() !==
                          form.getValues('startsAt').getDate()
                        );
                      }}
                      selected={field.value}
                      onSelect={(date) => {
                        field.onChange(date);
                        setIsEndsAtDatePickerOpen(false);
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
