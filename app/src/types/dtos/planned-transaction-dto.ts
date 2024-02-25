import { AmountType, Recurrence, TransactionType } from '..';

export type PlannedTransactionDto = {
  id: string;
  amount: number;
  description: string;
  amountType: AmountType;
  type: TransactionType;
  recurrence: Recurrence;
  startsAt: string;
  endsAt?: string;
  categoryId?: string;
  isDataFromCustomPlannedTransaction: boolean;
  isThereAnyCustomPlannedTransaction: boolean;
};
