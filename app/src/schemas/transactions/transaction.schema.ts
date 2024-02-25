import { z } from 'zod';

import { transformCurrencyToNumber } from '@/shared/transform-currency-to-number';
import { TransactionType } from '@/types';

export const TransactionsFormSchema = z.object({
  plannedTransactionId: z
    .string()
    .ulid('Pages.Transactions.Form.Validations.InvalidPlannedTransaction')
    .nullable(),
  description: z
    .string()
    .min(2, {
      message: 'Pages.Transactions.Form.Validations.DescriptionMinLength',
    })
    .max(150, {
      message: 'Pages.Transactions.Form.Validations.DescriptionMaxLength',
    }),
  amount: z.coerce
    .string({
      required_error: 'Pages.Transactions.Form.Validations.AmountRequired',
    })
    .transform(transformCurrencyToNumber)
    .refine((value) => value > 0, {
      message: 'Pages.Transactions.Form.Validations.AmountPositive',
    }),
  type: z.nativeEnum(TransactionType, {
    required_error: 'Pages.Transactions.Form.Validations.TypeRequired',
  }),
  categoryId: z
    .string()
    .ulid('Pages.Transactions.Form.Validations.InvalidCategory')
    .nullable(),
  paidAt: z.coerce.date({
    required_error: 'Pages.Transactions.Form.Validations.PaidAtRequired',
  }),
});
