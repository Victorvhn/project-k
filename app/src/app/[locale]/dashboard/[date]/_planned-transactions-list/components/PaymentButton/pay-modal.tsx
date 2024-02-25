'use client';

import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Loader2 } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { z } from 'zod';

import { ButtonBadge } from '@/components/button-badge';
import { ByDeviceModal } from '@/components/by-device-modal';
import { CurrencyInput } from '@/components/currency-input';
import { Button } from '@/components/ui/button';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { usePayPlannedTransaction } from '@/data/monthly/pay-planned-transaction';
import { PayPlannedTransactionsFormSchema } from '@/schemas';
import {
  MonthlyPlannedTransactionDto,
  MonthlyRequest,
  TransactionType,
} from '@/types';

export default function PayModal({
  plannedTransaction,
  monthlyRequest,
}: {
  plannedTransaction: MonthlyPlannedTransactionDto;
  monthlyRequest: MonthlyRequest;
}) {
  const form = useForm<z.infer<typeof PayPlannedTransactionsFormSchema>>({
    resolver: zodResolver(PayPlannedTransactionsFormSchema),
    defaultValues: {
      amount: plannedTransaction.amount,
    },
  });

  const t = useTranslations(
    'Pages.Dashboard.PlanedTransactionsList.ClickableBadge'
  );
  const payMutation = usePayPlannedTransaction();

  const onSubmit = async (
    values: z.infer<typeof PayPlannedTransactionsFormSchema>
  ) => {
    await payMutation.mutateAsync({
      plannedTransactionId: plannedTransaction.id,
      monthlyRequest: monthlyRequest,
      amount: values.amount,
    });
  };

  return (
    <Tooltip>
      <ByDeviceModal
        title={
          plannedTransaction.type === TransactionType.Income
            ? t('Receive')
            : t('Pay')
        }
        description={
          plannedTransaction.type === TransactionType.Income
            ? t('ReceiveDialogDescription')
            : t('PayDialogDescription')
        }
        trigger={
          <TooltipTrigger className='flex flex-row' asChild>
            <ButtonBadge aria-label={t('Pending')}>{t('Pending')}</ButtonBadge>
          </TooltipTrigger>
        }
      >
        <div className='flex flex-col'>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)}>
              <div className='mb-6 flex flex-col'>
                <FormField
                  control={form.control}
                  name='amount'
                  render={({ field }) => (
                    <FormItem className='w-full'>
                      <FormLabel>{t('Amount')}</FormLabel>
                      <FormControl>
                        <CurrencyInput {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
              <Button
                type='submit'
                disabled={form.formState.isSubmitting}
                className='w-full'
                aria-label={t('Pay')}
              >
                {form.formState.isSubmitting && (
                  <Loader2 className='mr-2 h-4 w-4 animate-spin' />
                )}
                {t('Conclude')}
              </Button>
            </form>
          </Form>
        </div>
      </ByDeviceModal>
      <TooltipContent side='bottom'>
        <p>
          {plannedTransaction.type === TransactionType.Income
            ? t('Receive')
            : t('Pay')}
        </p>
      </TooltipContent>
    </Tooltip>
  );
}
