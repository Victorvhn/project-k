import { z } from 'zod';

import { transformCurrencyToNumber } from '@/shared/transform-currency-to-number';
import { AmountType, Recurrence, TransactionType } from '@/types';

export const PlannedTransactionsFormSchema = z.object({
  description: z
    .string()
    .min(2, {
      message:
        'Pages.PlannedTransactions.Form.Validations.DescriptionMinLength',
    })
    .max(150, {
      message:
        'Pages.PlannedTransactions.Form.Validations.DescriptionMaxLength',
    }),
  amount: z.coerce
    .string({
      required_error:
        'Pages.PlannedTransactions.Form.Validations.AmountRequired',
    })
    .transform(transformCurrencyToNumber)
    .refine((value) => value > 0, {
      message: 'Pages.PlannedTransactions.Form.Validations.AmountPositive',
    }),
  amountType: z.nativeEnum(AmountType, {
    required_error:
      'Pages.PlannedTransactions.Form.Validations.AmountTypeRequired',
  }),
  type: z.nativeEnum(TransactionType, {
    required_error: 'Pages.PlannedTransactions.Form.Validations.TypeRequired',
  }),
  recurrence: z.nativeEnum(Recurrence, {
    required_error:
      'Pages.PlannedTransactions.Form.Validations.RecurrenceRequired',
  }),
  startsAt: z.coerce.date({
    required_error:
      'Pages.PlannedTransactions.Form.Validations.StartsAtRequired',
  }),
  endsAt: z.coerce.date().optional(),
  categoryId: z
    .string()
    .ulid('Pages.PlannedTransactions.Form.Validations.InvalidCategory')
    .nullable(),
});
