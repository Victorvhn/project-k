import { AmountType, Recurrence, TransactionType } from '@/types';

export type CreatePlannedTransactionRequest = {
  description: string;
  amount: number;
  amountType: AmountType;
  type: TransactionType;
  recurrence: Recurrence;
  startsAt: string;
  endsAt: string | null;
  categoryId: string | null;
};
