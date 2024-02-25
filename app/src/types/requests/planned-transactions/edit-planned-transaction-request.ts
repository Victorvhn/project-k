import { ActionType, AmountType, Recurrence, TransactionType } from '@/types';

export type EditPlannedTransactionRequest = {
  actionType: ActionType;
  year: number;
  month: number;
  description: string;
  amount: number;
  amountType: AmountType;
  type: TransactionType;
  recurrence: Recurrence;
  startsAt: string;
  endsAt: string | null;
  categoryId: string | null;
};
