import { z } from 'zod';

import { transformCurrencyToNumber } from '@/shared/transform-currency-to-number';

export const PayPlannedTransactionsFormSchema = z.object({
  amount: z.coerce
    .string({
      required_error:
        'Pages.Dashboard.ClickableBadge.Validations.AmountRequired',
    })
    .transform(transformCurrencyToNumber)
    .refine((value) => value > 0, {
      message: 'Pages.Dashboard.ClickableBadge.Validations.AmountPositive',
    }),
});
