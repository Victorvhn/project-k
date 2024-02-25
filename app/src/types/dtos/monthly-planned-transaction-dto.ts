import { AmountType, Recurrence, TransactionType } from '..';

export type MonthlyPlannedTransactionDto = {
  id: string;
  description: string;
  amount: number;
  amountType: AmountType;
  type: TransactionType;
  recurrence: Recurrence;
  referringDate: string;
  startsAt: string;
  endsAt?: string;
  categoryId?: string;
  paidTransactionId?: string;
  paid: boolean;
  paidAt?: string;
  paidAmount: number;
  tags: string[];
  ignore: boolean;

  // For optimistic updates
  isChanging?: boolean;
};
